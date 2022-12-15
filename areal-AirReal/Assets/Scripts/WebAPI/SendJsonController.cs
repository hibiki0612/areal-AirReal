using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Kogane;
using UnityEngine.Networking;
using System.Text;
using System;

public class SendJsonController : MonoBehaviour
{

    [Serializable]
    private sealed class Data
    {
        public string word = "human, antenna";
        public string rgb = "[56, 115, 243], [243, 148, 56]";
        public string text = "a human, has big antenna, on the grass.";
    }

    private void Awake()
    {
        /*
        var url      = "http://127.0.0.1:5000/image";
        var data     = new Data();
        var json     = JsonUtility.ToJson( data );
        var postData = Encoding.UTF8.GetBytes( json );

        var request = new UnityWebRequest( url, UnityWebRequest.kHttpVerbPOST )
        {
            uploadHandler   = new UploadHandlerRaw( postData ),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader( "Content-Type", "application/json" );

        var operation = request.SendWebRequest();

        operation.completed += _ =>
        {
            Debug.Log( operation.isDone );
            Debug.Log( operation.webRequest.downloadHandler.text );
            Debug.Log( operation.webRequest.isHttpError );
            Debug.Log( operation.webRequest.isNetworkError );
        };
        */
    }

}