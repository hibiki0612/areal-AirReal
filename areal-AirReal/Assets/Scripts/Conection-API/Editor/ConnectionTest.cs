using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
public class ConnectionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UPloadFile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator UPloadFile(){
        string fileName = "test.png";
        string filePath = Application.dataPath + "/" + fileName;
        // 画像ファイルをbyte配列に格納
        byte[] img = File.ReadAllBytes (filePath);
        // formにバイナリデータを追加
        WWWForm form = new WWWForm ();
        form.AddBinaryData ("image", img, fileName, "image/png");
        // HTTPリクエストを送る
        UnityWebRequest request = UnityWebRequest.Post("https://withar-2022-2023-backend.lolipop.io/image", form);
        yield return request.SendWebRequest ();
        //"Save Texture"ダイアログを表示し、選択されたパスを取得する
        var path = EditorUtility.SaveFilePanelInProject(title: "Save Texture", defaultName: "test", extension: "png", message: "Save Texture");
        //新規の空テクスチャを作成する
        var texture = new Texture2D(1, 1);
        byte[] bytes = System.Convert.FromBase64String(request.downloadHandler.text);
        texture.LoadImage(bytes);
        var png = texture.EncodeToPNG();
        //PNG形式でエンコード
        File.WriteAllBytes(path, png);
        //オブジェクトに画像を表示
        GetComponent<Renderer>().material.mainTexture = texture;
    }
}
