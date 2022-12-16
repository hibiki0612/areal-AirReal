using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Kogane;
using UnityEngine.Networking;
using System.Text;
using System;
using UnityEngine.UI;
using System.Collections;

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

    [SerializeField] private string img_str;
    //画像が返ってきたらオン
    [SerializeField] bool resultActive =false;
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

    IEnumerator image2image()
    {
        string fileName = "/paint.png";
        string filePath = Application.dataPath + "/" + fileName;
        // 画像ファイルをbyte配列に格納

        byte[] img = File.ReadAllBytes(filePath);
        //string img_str = BitConverter.ToString(img);
        img_str = Convert.ToBase64String(img);

        color_str = textAndColorSave.color_str;
        _sentence = _text.text;

        var url = "http://127.0.0.1:5000/image";
        var data = new Data();

        //data.word = word_str;
        data.rgb = color_str;
        //data.text = _sentence;
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

        };

        texture = new Texture2D(1, 1);
        byte[] bytes = System.Convert.FromBase64String(request.downloadHandler.text);
        texture.LoadImage(bytes);
        var png = texture.EncodeToPNG();
        //PNG形式でエンコード
        
        File.WriteAllBytes("test.png",png);
        //オブジェクトに画像を表示
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

            //画像生成が終わったら
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
                            //返ってきた画像を貼る
                            TargetObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
                            cnt++;

                            TargetObject.GetComponent<MeshRenderer>().material.SetFloat("_MosaicResolution", cnt);
                            currentTime = 0f;

                        }
                    }
                    if (cnt == 64)
                    {
                        TargetObject.GetComponent<MeshRenderer>().material = paper;
                        //返ってきた画像を貼る
                        TargetObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
                    }
                }
                
            }

        }

    }
    

}