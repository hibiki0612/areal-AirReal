using System; //構造体のシリアライズ化で必要
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks; //UniTaskの使用に必要

/// <summary>
/// 地理座標
/// </summary>
[Serializable]
public struct GeoCoord
{
    /// <summary>
    /// 緯度
    /// </summary>
    public double Latitude;

    /// <summary>
    /// 経度
    /// </summary>
    public double Longitude;

    /// <summary>
    /// 標高
    /// </summary>
    public double Elevation;

    /// <summary>
    /// ジオイド高
    /// </summary>
    public double Geoid;

    /// <summary>
    /// 高度(楕円体高)
    /// </summary>
    public double Altitude => Elevation + Geoid;
}

/// <summary>
/// 国土地理院ジオイド高APIのレスポンス
/// </summary>
[Serializable]
public struct GsiGeoidHeightJson
{
    public OutputData OutputData;
}

[Serializable]
public struct OutputData
{
    public double latitude;
    public double longitude;
    public double geoidHeight;
}

/// <summary>
/// 建物群
/// </summary>
public class BuildingGroup : MonoBehaviour
{
    /// <summary>
    /// 基準の地理座標
    /// </summary>
    public GeoCoord Origin => _origin;

    [SerializeField] private GeoCoord _origin;

    /// <summary>
    /// objファイルに埋め込まれた地理座標を取得するためのモデルのアセット
    /// Originの座標は同じ区画内で共通なのでどれか1つだけ使用する
    /// </summary>
    [SerializeField] private GameObject _originBuilding;

    /// <summary>
    /// 国土地理院のジオイド高取得API
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    private static string GsiGeoidApi(double latitude, double longitude) => $"https://vldb.gsi.go.jp/sokuchi/surveycalc/geoid/calcgh/cgi/geoidcalc.pl?outputType=json&latitude={latitude}&longitude={longitude}";

    [Header("動的にマテリアルを切り替え")]
    [SerializeField] private Material _replaceTarget;
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
    [SerializeField] private List<Material> _originalMaterials = new List<Material>();

    private bool _isMaterialOrigin = true;

    /// <summary>
    /// マテリアル切り替え
    /// </summary>
    public void SwitchMaterial()
    {
        if (_isMaterialOrigin)
        {
            ReplaceMaterial();
        }
        else
        {
            RestoreMaterial();
        }
    }

    private void ReplaceMaterial()
    {
        if (_replaceTarget == null) return;
        foreach (var renderer in _renderers)
        {
            renderer.material = _replaceTarget;
        }
        _isMaterialOrigin = false;
    }

    private void RestoreMaterial()
    {
        if (_renderers.Count != _originalMaterials.Count) return;
        for (var i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material = _originalMaterials[i];
        }
        _isMaterialOrigin = true;
    }


    #if UNITY_EDITOR

    [ContextMenu(nameof(Initialize))]
    private void Initialize()
    {
        SetOriginAsync().Forget();

        // デバッグ用に動的にマテリアルを切り替える準備
        _renderers = GetComponentsInChildren<Renderer>().ToList();
        _originalMaterials.Clear();
        foreach (var renderer in _renderers)
        {
            _originalMaterials.Add(renderer.sharedMaterial);
        }
    }

    /// <summary>
    /// 子の建物のOriginを初期化
    /// </summary>
    private async UniTask SetOriginAsync()
    {
        // 元のアセット(objファイル)のパスを取得
        var result = GetAssetPath(_originBuilding);
        if (!result.ok) return;
        // Originの地理座標を読み込み
        await LoadGeoCoordOriginAsync(result.path);
        await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.Log("Save Assets");
    }

    /// <summary>
    /// <see cref="target"/>の元のアセットのパスを取得
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static (bool ok, string path) GetAssetPath(GameObject target)
    {
        var original = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(target);
        var ok = original != null;
        return (ok, ok ? UnityEditor.AssetDatabase.GetAssetPath(original) : string.Empty);
    }

    /// <summary>
    /// gmlから変換したobjのヘッダーから地理座標を読み込む
    /// </summary>
    /// <param name="filePath"></param>
    /// <exception cref="FileNotFoundException"></exception>
    private async UniTask LoadGeoCoordOriginAsync(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException();
        // Originは1行目にあるので、ファイルの1行目だけ読み込めばよい
        using var reader = new StreamReader(filePath, System.Text.Encoding.GetEncoding("UTF-8"));
        var header = await reader.ReadLineAsync();
        var geoCoord = header?.Split(' ')
            .LastOrDefault()?
            .Split(',')
            .Select(x => (ok: double.TryParse(x, out var coord), coord: coord))
            .Where(x => x.ok)
            .Select(x => x.coord)
            .ToArray();
        
        if (geoCoord == null || geoCoord.Length < 3) return;

        // ジオイド高を取得
        var geoid = await RequestGeoidAsync(geoCoord[0], geoCoord[1]);

        _origin = new GeoCoord
        {
            Latitude = geoCoord[0],
            Longitude = geoCoord[1],
            Elevation = geoCoord[2],
            Geoid = geoid,
        };
    }

    /// <summary>
    /// ジオイド高をリクエスト
    /// </summary>
    /// <param name="lat">緯度</param>
    /// <param name="lng">経度</param>
    /// <returns></returns>
    private async UniTask<double> RequestGeoidAsync(double lat, double lng)
    {
        var request = UnityWebRequest.Get(GsiGeoidApi(lat, lng));
        await request.SendWebRequest();
        if (request.error != null)
        {
            Debug.LogError(request.error);
            return double.NaN;
        }

        var json = request.downloadHandler.text;
        var response = JsonUtility.FromJson<GsiGeoidHeightJson>(json);
        return response.OutputData.geoidHeight;
    }
    #endif

}
