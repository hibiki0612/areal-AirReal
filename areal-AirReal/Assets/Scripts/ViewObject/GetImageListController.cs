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

public class GetImageListController : MonoBehaviour
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

            StorageReference imageRef = storageRef.Child(DictionaryData["changeImgPath"].ToString());
            Debug.Log(DictionaryData["changeImgPath"]);

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

                    Texture2D texture = new Texture2D(128, 128);
                    texture.LoadImage(fileContents);
                    //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    //imageCanvas.sprite = sprite;

                    Debug.Log("画像を生成しました");
                }
            });

        }

    }
}
