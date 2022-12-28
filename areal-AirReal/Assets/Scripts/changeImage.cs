using UnityEngine;
using Firebase.Storage;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class changeImage : MonoBehaviour
{
    //[SerializeField] Texture2D image;

    // 保存するデータ
    // Quaternion move_q = Quaternion.Euler(0f, 0f, 1.0f);
    // Dictionary<string, object> product_data = new Dictionary<string, object> {
    //     {"latitude", 0},
    //     {"longitude", 0},
    //     {"altitude", 0},
    // };
    public static string docId;

    // 画像をストレージに保存し、そのパスを firestore に保存する
    public void SaveImageAndPath(Dictionary<string, object> product_data, Quaternion quaternion, Texture2D image)
    {
        
        float quaternion_x = quaternion.x;
        float quaternion_y = quaternion.y;
        float quaternion_z = quaternion.z;
        float quaternion_w = quaternion.w;

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
                Debug.Log(product_data);
                // 保存した画像のパス
                string imgPath = imageRef.Path;
                product_data["path"] = imgPath;
                product_data["quaternion_x"] = quaternion_x;
                product_data["quaternion_y"] = quaternion_y;
                product_data["quaternion_z"] = quaternion_z;
                product_data["quaternion_w"] = quaternion_w;
                //product_data["likes"];
                //product_data["description"]

                DocumentReference addedDocRef = await firestore.Collection("products").AddAsync(product_data);

                docId = addedDocRef.Id;
                Debug.Log(docId+ "にデータが保存されました");
            }
        });
    }

    // public void clickBtn()
    // {
    //     //SaveImageAndPath(product_data, move_q, image);
    // }
}
