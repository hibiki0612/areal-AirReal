using UnityEngine;
using Firebase.Storage;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class changeImage : MonoBehaviour
{
    // 画像をストレージに保存し、そのパスを firestore に保存する
    public void SaveImageAndPath(Texture2D image)
    {

        // Firebase storage のクライアント
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Firestore のクライアント
        FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

        // ストレージに保存するために、画像データをバイト配列に変換する
        byte[] data = image.EncodeToPNG();

        string filename = System.Guid.NewGuid() + "test.png";
        

        // ストレージの参照をもとに、新しい画像データを保存する
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://areal-71159.appspot.com");

        StorageReference imageRef = storageRef.Child("images/" + filename);

        imageRef.PutBytesAsync(data).ContinueWith(async task => {
            if (task.IsFaulted)
            {
                // 保存に失敗したときの処理
                Debug.Log("画像の保存に失敗しました");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("画像の保存に成功しました");
                // 保存した画像のパス
                string imgPath = imageRef.Path;

                // 保存するデータ
                Dictionary<string, object> product_data = new Dictionary<string, object> {
                    {"x", ""},
                    {"y", ""},
                    {"z", ""},
                    {"path", imgPath}
                };

                DocumentReference addedDocRef = await firestore.Collection("products").AddAsync(product_data);

                string docId = addedDocRef.Id;
                Debug.Log(docId+ "にデータが保存されました");

                // APIを呼び出して画像の加工をする
                // レスポンスが返ってきたらその画像をstorageに保存する
                filename = System.Guid.NewGuid() + "test.png";
                StorageReference changeImageRef = storageRef.Child("images/" + filename);
                
                // ストレージに保存するために、画像データをバイト配列に変換する
                byte[] data = image.EncodeToPNG();

                await changeImageRef.PutBytesAsync(data).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        // 保存に失敗したときの処理
                        Debug.Log("2回目の画像の保存に失敗しました");
                    }
                    else if (task.IsCompleted)
                    {
                        Debug.Log("画像の保存に成功しました");
                        // 保存した画像のパス
                        string changeImgPath = imageRef.Path;

                        // 保存するデータ
                        Dictionary<string, object> product_data = new Dictionary<string, object> {
                            {"changeImgPath", changeImgPath}
                        };

                        Task reAddedDocRef = firestore.Collection("products").Document(docId).SetAsync(product_data);

                        Debug.Log(reAddedDocRef.Id + "にデータが保存されました");
                    }
                });


                        //// firestore 上の、この画像のパスを保存する場所
                        //DocumentReference pathRef = firestore.Document("products");


                        //// firestoreへの保存
                        //pathRef.SetAsync(product_data);
                    }
        });
    }
}
