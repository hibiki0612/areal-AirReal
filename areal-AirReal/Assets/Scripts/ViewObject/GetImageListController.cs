using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Storage;
using UnityEngine;

using System;
using System.Threading.Tasks;

public class GetImageListController : MonoBehaviour
{
    [SerializeField] private GameObject after_paint;
    private MeshRenderer _meshRenderer;
    [SerializeField] private Material paintMaterial;

    public List<Texture> AfterpaintList;
    public List<Texture> BeforepaintList;
    [SerializeField] private Material material;

    [SerializeField] private ViewObjectController viewObjectController;

    public Texture2D generateImage(Dictionary<string, object> DictionaryData, StorageReference imageRef)
    {
        const long maxAllowedSize = 1 * 1024 * 1024;
        Texture2D texture = new Texture2D(128, 128);

        _ = imageRef.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogException(task.Exception);
            }
            else
            {
                // ?????
                byte[] fileContents = task.Result;
                texture.LoadImage(fileContents);

            }
        });
        return texture;
    }

    public async void Awake()
    {
        _meshRenderer = after_paint.GetComponent<MeshRenderer>();
        // Firebase storage ???N???C?A???g
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://areal-71159.appspot.com");

        // Firestore ???N???C?A???g
        FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

        // Limit(10)?10????????????????
        QuerySnapshot getData = await firestore.Collection("products").OrderBy("created_at").Limit(20).GetSnapshotAsync();
        foreach (var document in getData.Documents)
        {
            Dictionary<string, object> DictionaryData = document.ToDictionary();
            Debug.Log(DictionaryData["created_at"]);

            // ??????????????AfterpaintList????????
            StorageReference beforeImageRef = storageRef.Child(DictionaryData["path"].ToString());
            Texture2D beforeTexture = generateImage(DictionaryData, beforeImageRef);
            BeforepaintList.Add(beforeTexture);


            StorageReference afterImageRef = storageRef.Child(DictionaryData["changeImgPath"].ToString());
            Texture2D afterTexture = generateImage(DictionaryData, afterImageRef);
            AfterpaintList.Add(afterTexture);

        }
        Invoke("Active", 2);
    }

    private void Active()
    {
        viewObjectController.active = true;
    }
}