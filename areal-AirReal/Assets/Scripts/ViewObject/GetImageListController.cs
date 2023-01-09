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
    [SerializeField] private GameObject after_paint;
    private MeshRenderer _meshRenderer;
    [SerializeField] private Material paintMaterial;

    public List<Texture> AfterpaintList;
    [SerializeField] List<GameObject> AfterobjList;
    [SerializeField] private Material material;

    [SerializeField] private ViewObjectController viewObjectController;
    public async void Awake()
    {
        _meshRenderer = after_paint.GetComponent<MeshRenderer>();
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
                    Texture paintTexture = texture as Texture;

                    //_meshRenderer.material.mainTexture = paintTexture;

                    AfterpaintList.Add(paintTexture);
                    Debug.Log("画像を生成しました");
                }
            });

        }
        Invoke("Active", 2);
       


    }

    private void Active()
    {
        viewObjectController.active = true;
    }
}
