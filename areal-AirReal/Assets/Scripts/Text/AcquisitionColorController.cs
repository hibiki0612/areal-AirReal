using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using  UnityEngine.UI;
public class AcquisitionColorController : MonoBehaviour
{
    public Color color;
    
    private Texture tex;
    private Texture2D texture2D;

    private Texture2D targetTexture;

    private void Start()
    {
        targetTexture = new Texture2D(1, 1);
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject()) return;
#else
	                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))return;
#endif
            
            Vector3 touchPos = Input.GetTouch(0).position;
            
            
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "PaintCanvas")
                {
                    
                    tex = hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture;
                    texture2D = ToTexture2D(tex);
                    Debug.Log(texture2D);

                    var png = texture2D.EncodeToPNG();
                    File.WriteAllBytes("Assets/Image/paint.png", png);
                    Debug.Log(hit.textureCoord);
                    StartCoroutine(GetColorCoroutine((int)touchPos.x, (int)touchPos.y));
                                        
                }
                
            }

        }
    }

    private IEnumerator GetColorCoroutine(int x, int y)

    {

        yield return new WaitForEndOfFrame();

        targetTexture.ReadPixels(new Rect(x,y, 1, 1), 0, 0);

        color = targetTexture.GetPixel(0, 0);

    }


    public Texture2D ToTexture2D(Texture texture)
    {
        var sw = texture.width;
        var sh = texture.height;
        var format = TextureFormat.RGBA32;
        var result = new Texture2D(sw, sh, format, false);
        var currentRT = RenderTexture.active;
        var rt = new RenderTexture(sw, sh, 32);
        Graphics.Blit(texture, rt);
        RenderTexture.active = rt;
        var source = new Rect(0, 0, rt.width, rt.height);
        result.ReadPixels(source, 0, 0);
        result.Apply();
        RenderTexture.active = currentRT;
        return result;
    }
}
