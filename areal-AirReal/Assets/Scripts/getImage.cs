using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Storage;
using UnityEngine;

public class getImage : MonoBehaviour
{
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

                    // ここでgeospatial apiを呼び出す

                    // 緯度経度高度
                    float latitude = (float)Convert.ChangeType(DictionaryData["latitude"], typeof(float));
                    float logitude = (float)Convert.ChangeType(DictionaryData["longtitude"], typeof(float));
                    float altitude = (float)Convert.ChangeType(DictionaryData["altitude"], typeof(float));

                    float quaternion_x = (float)Convert.ChangeType(DictionaryData["quaternion_x"], typeof(float));
                    float quaternion_y = (float)Convert.ChangeType(DictionaryData["quaternion_y"], typeof(float));
                    float quaternion_z = (float)Convert.ChangeType(DictionaryData["quaternion_z"], typeof(float));
                    float quaternion_w = (float)Convert.ChangeType(DictionaryData["quaternion_w"], typeof(float));

                    // Quaternion
                    Quaternion quaternion = new Quaternion(quaternion_x, quaternion_y, quaternion_z, quaternion_w);


                }
            });

        }

    }

}
