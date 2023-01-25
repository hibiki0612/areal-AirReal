using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Kogane;
using UnityEngine.Networking;
using System.Text;
using System;
using UnityEngine.UI;
using System.Collections;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using Firebase.Storage;
using Firebase.Firestore;
using System.Threading.Tasks;

public class SendJsonController : MonoBehaviour
{
    [SerializeField] private TextAndColorSave textAndColorSave;

    private string word_str;
    private string color_str;
    public string _sentence;
    [SerializeField] private Text _text;
    private GameObject TargetObject;
    [SerializeField] private PaintCanvasCreate paintCanvasCreate;
    [SerializeField] private Material mosaic;
    [SerializeField] private Material paper;
    private Texture2D texture;
    public float span = 1f;
    private float currentTime = 0f;
    private int cnt = 64;
    private bool MosaicActive;

    [SerializeField] private AppearLoading appearLoading;

    [SerializeField] private string img_str;
    //�摜���Ԃ��Ă�����I��
    [SerializeField] bool resultActive =false;

    [SerializeField] private GeospatialController _geospatialController;
    
    
    
    
    
    
    public void SaveObject()
    {
        TargetObject = paintCanvasCreate.TargetObj;
    }

    [Serializable]
    private sealed class Data
    {
        public string word = ",horse";
        public string rgb = "";
        public string text = "a horse on the grass";
        public string image = "****";
    }

    public void GanerateButton()
    {
        MosaicActive = true;
        paper = TargetObject.GetComponent<MeshRenderer>().material;
        mosaic.mainTexture = TargetObject.GetComponent<MeshRenderer>().material.mainTexture;
        TargetObject.GetComponent<MeshRenderer>().material = mosaic;
        StartCoroutine(image2image());
    }

    async void saveFirebase()
    {
        // APIを呼び出して画像の加工をする
        // レスポンスが返ってきたらその画像をstorageに保存する

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;
            
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://areal-71159.appspot.com");
            
            
        string filename = System.Guid.NewGuid() + "test.png";
        StorageReference changeImageRef = storageRef.Child("images/" + filename);
                
        // ストレージに保存するために、画像データをバイト配列に変換する
        byte[] img_data = texture.EncodeToPNG();

        await changeImageRef.PutBytesAsync(img_data).ContinueWith(async (task) =>
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
                string changeImgPath = changeImageRef.Path;

                // 保存するデータ
                Dictionary<string, object> product_data = new Dictionary<string, object> {
                    {"changeImgPath", changeImgPath}
                };
                Debug.Log("docId");
                Debug.Log(changeImage.docId);
                DocumentReference reAddedDocRef = firestore.Collection("products").Document(changeImage.docId);
                await reAddedDocRef.UpdateAsync(product_data);

                
                Debug.Log(reAddedDocRef.Id + "にデータが保存されました");
            }
        });
    }
    IEnumerator image2image()
    {
        string fileName = "/paint.png";
        string filePath = Application.persistentDataPath + "/" + fileName;
        // �摜�t�@�C����byte�z��Ɋi�[

        byte[] img = File.ReadAllBytes(filePath);
        //string img_str = BitConverter.ToString(img);
        img_str = Convert.ToBase64String(img);
        
        texture = new Texture2D(1, 1);
        texture.LoadImage(img);
        _geospatialController.OnSaveButton(texture);

        word_str = textAndColorSave.word_str;
        Debug.Log((textAndColorSave.word_str));
        color_str = textAndColorSave.color_str;
        _sentence = _text.text;

        var url = "http://18.182.135.49:5000/image";
        var data = new Data();

        data.word = word_str;
        data.rgb = color_str;
        data.text = _sentence;
        data.image = img_str;
        Debug.Log(data.word);
        Debug.Log(data.rgb);
        Debug.Log(data.text);
        Debug.Log(data.image);

        var json = JsonUtility.ToJson(data);
        var postData = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
        {
            uploadHandler = new UploadHandlerRaw(postData),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");

        var operation = request.SendWebRequest();
        yield return operation;

        operation.completed += _ =>
        {
            Debug.Log(operation.isDone);
            Debug.Log(operation.webRequest.downloadHandler.text);
            Debug.Log(operation.webRequest.isHttpError);
            Debug.Log(operation.webRequest.isNetworkError);
            resultActive = true;
            appearLoading.LoadingDisAppear();
            var textobj = GameObject.FindGameObjectsWithTag("Text");
            foreach (var obj in textobj)
            {
                obj.SetActive(false);
            }
            
           
        };

        texture = new Texture2D(1, 1);
        byte[] bytes = System.Convert.FromBase64String(request.downloadHandler.text);
        texture.LoadImage(bytes);

        saveFirebase();


        //var png = texture.EncodeToPNG();
        //PNG�`���ŃG���R�[�h

        //File.WriteAllBytes("test.png",png);
        //�I�u�W�F�N�g�ɉ摜��\��
        //TargetObject.GetComponent<MeshRenderer>().material.mainTexture = texture;

    }

    bool mosaicstatus = false;
    bool ansStatus = false;

    void Update()
    {
        
        if (MosaicActive)
        {
            if (!resultActive)
            {
                if (!mosaicstatus)
                {
                    if (cnt >= 5)
                    {
                        currentTime += Time.deltaTime;

                        if (currentTime > span / 10)
                        {

                            TargetObject.GetComponent<MeshRenderer>().material.SetFloat("_MosaicResolution", cnt);
                            currentTime = 0f;
                            cnt--;
                            if (cnt == 5)
                            {
                                mosaicstatus = true;
                            }

                        }

                    }
                }
                if (mosaicstatus)
                {
                    if (cnt <= 60)
                    {
                        currentTime += Time.deltaTime;

                        if (currentTime > span / 10)
                        {

                            TargetObject.GetComponent<MeshRenderer>().material.SetFloat("_MosaicResolution", cnt);
                            currentTime = 0f;
                            cnt++;
                            if (cnt == 60)
                            {
                                mosaicstatus = false;
                            }

                        }
                    }
                }
                
            }

            //�摜�������I�������
            if (resultActive)
            {
                if (cnt >= 5)
                {
                    currentTime += Time.deltaTime;

                    if (currentTime > span / 10)
                    {

                        TargetObject.GetComponent<MeshRenderer>().material.SetFloat("_MosaicResolution", cnt);
                        currentTime = 0f;
                        cnt--;
                        if (cnt == 5)
                        {
                            //mosaicstatus = true;
                            ansStatus = true;
                        }

                    }

                }
                if (ansStatus)
                {
                    if (cnt <= 64)
                    {
                        currentTime += Time.deltaTime;

                        if (currentTime > span / 20)
                        {
                            //�Ԃ��Ă����摜��\��
                            TargetObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
                            cnt++;

                            TargetObject.GetComponent<MeshRenderer>().material.SetFloat("_MosaicResolution", cnt);
                            currentTime = 0f;

                        }
                    }
                    if (cnt == 64)
                    {
                        TargetObject.GetComponent<MeshRenderer>().material = paper;
                        //�Ԃ��Ă����摜��\��
                        TargetObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
                    }
                }
                
            }

        }

    }
    

}