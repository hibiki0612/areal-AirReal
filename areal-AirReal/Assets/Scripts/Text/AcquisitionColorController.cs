using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using  UnityEngine.UI;
public class AcquisitionColorController : MonoBehaviour
{
    public Color color;
    
    public Texture tex;
    public Texture2D texture2D;
    [SerializeField] private GameObject Plate;
    [SerializeField] private Image image;
    public RenderTexture texture;
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
                    
                    texture2D = new Texture2D(tex.width, tex.height,TextureFormat.ARGB32, false);
                    Graphics.CopyTexture(tex, texture2D);
                    
                    
                    image.material.mainTexture = texture2D;
                    
                    var png = texture2D.EncodeToPNG();
                    File.WriteAllBytes("Assets/Image/paint.png", png);
                    
                    color = texture2D.GetPixel((int)touchPos.x, (int)touchPos.y);
                }
                
            }

        }
    }
}
