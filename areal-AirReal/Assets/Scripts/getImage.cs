using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions.Samples.Geospatial;

public class getImage : MonoBehaviour
{

    //[SerializeField] Image imageCanvas;
    public GeospatialController geospatialController;

    public async void getAllImage()
    {
        // Firebase storage のクライアント
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://areal-71159.appspot.com");

        const long maxAllowedSize = 1 * 1024 * 1024;

        // Firestore のクライアント
        FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

        QuerySnapshot getData = await firestore.Collection("products").GetSnapshotAsync();
        foreach (var document in getData.Documents)
        {
            Dictionary<string, object> DictionaryData = document.ToDictionary();
            
            StorageReference imageRef = storageRef.Child(DictionaryData["path"].ToString());
            Debug.Log(DictionaryData["path"]);

            _ = imageRef.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogException(task.Exception);
                }
                else
                {
                    // 画像データ
                    byte[] fileContents = task.Result;
                    Debug.Log("画像データを取得しました");

                    // ここでgeospatial apiを呼び出す

                    // 緯度経度高度
                    float latitude = (float)Convert.ChangeType(DictionaryData["latitude"], typeof(float));
                    float longitude = (float)Convert.ChangeType(DictionaryData["longitude"], typeof(float));
                    float altitude = (float)Convert.ChangeType(DictionaryData["altitude"], typeof(float));

                    Debug.Log(latitude);
                    Debug.Log(longitude);
                    Debug.Log(altitude);

                    float quaternion_x = (float)Convert.ChangeType(DictionaryData["quaternion_x"], typeof(float));
                    float quaternion_y = (float)Convert.ChangeType(DictionaryData["quaternion_y"], typeof(float));
                    float quaternion_z = (float)Convert.ChangeType(DictionaryData["quaternion_z"], typeof(float));
                    float quaternion_w = (float)Convert.ChangeType(DictionaryData["quaternion_w"], typeof(float));
                    // Quaternion
                    Quaternion quaternion = new Quaternion(quaternion_x, quaternion_y, quaternion_z, quaternion_w);

                    Debug.Log(quaternion);
                    Texture2D texture = new Texture2D(128, 128);
                    texture.LoadImage(fileContents);
                    //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    //imageCanvas.sprite = sprite;

                    geospatialController.SetHistory(latitude, longitude, altitude, quaternion, texture);
                    Debug.Log("画像を生成しました");
                }
            });

        }

    }

}
