using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GeospatialPlateauExample : MonoBehaviour
{
    [Header("Geospatial")]
    [SerializeField] private ARAnchorManager _arAnchorManager;

    [Header("Buildings")]
    [SerializeField] private BuildingGroup _buildingGroup;

    private BuildingGroup _placedBuilding;
    private bool isEnter = false; //SwitchOcclusionMaterialには一度だけ入る

    private void Update()
    {
        PlaceBuildings();

        SwitchOcclusionMaterial();
    }

    private void PlaceBuildings()
    {
        if (_placedBuilding != null) return;
            
        var placed = Place(_buildingGroup.gameObject, new GeospatialPose
        {
            Latitude = _buildingGroup.Origin.Latitude,
            Longitude = _buildingGroup.Origin.Longitude,
            Altitude = _buildingGroup.Origin.Altitude
        });
        if (placed == null) return;
        if (placed.TryGetComponent<BuildingGroup>(out var buildingGroup))
        {
            _placedBuilding = buildingGroup;
        }
    }

    private void SwitchOcclusionMaterial()
    {
        if (_placedBuilding == null) return;
        if(!isEnter)
        {
            _placedBuilding.SwitchMaterial();
            isEnter = true;
        }
           
    }

    /// <summary>
    /// <see cref="targetObj"/>を<see cref="pose"/>に配置する
    /// </summary>
    /// <param name="targetObj">対象のオブジェクト</param>
    /// <param name="pose">地理座標上の姿勢</param>
    /// <returns></returns>
    private GameObject Place(GameObject targetObj, GeospatialPose pose)
    {
        var quaternion = Quaternion.AngleAxis(180f - (float)pose.Heading, Vector3.up);
        var anchor = _arAnchorManager.AddAnchor(pose.Latitude, pose.Longitude, pose.Altitude, quaternion);
        var placed = Instantiate(targetObj, anchor.transform);
        return placed;
    }

    /*
    /// <summary>
    /// 配置済みのオブジェクトを削除する
    /// </summary>
    private void Clear()
    {

        if(_placedBuilding == null) return;
        Destroy(_placedBuilding.gameObject);
        _placedBuilding = null;
    }
    */
}
