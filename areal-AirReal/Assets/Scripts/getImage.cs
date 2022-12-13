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
                    byte[] fileContents = task.Result;
                    // テストコード
                    string str = "";
                    for (int i = 0; i < fileContents.Length; i++)
                    {
                        str += string.Format("{0:X2}", fileContents[i]);
                    }
                    Debug.Log(str);
                    //ここまで
                    Debug.Log("Finished downloading!");

                    // ここでgeospatial apiを呼び出す
                }
            });

        }

    }

}
