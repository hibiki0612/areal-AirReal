using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Kogane;
using UnityEngine.Networking;
using System.Text;


public class SendJsonController : MonoBehaviour
{
    
    
    private Dictionary<string, string> dictionary1;
    private string RGBString;
    private List<string> human;

    private void SendJsonButton()
    {

        human = new List<string> { "human", "[56, 115, 243]" };
        

        string fullText = "a human, has big antenna, on the grass.";
        dictionary1 = new Dictionary<string, string>()
        {
        };

        dictionary1.Add(human[0], human[1]);
        dictionary1.Add("antenna", "[243, 148, 56]");
        dictionary1.Add("FULL_TEXT", fullText);


        var jsonDictionary1 = new JsonDictionary<string, string>(dictionary1);
        var json = JsonUtility.ToJson(jsonDictionary1, true);
        File.WriteAllText("JsonFile.json", json);


        //post‚·‚éˆ—
        /*
        var url = "https://httpbin.org/post";
        var postData = Encoding.UTF8.GetBytes(json);

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
