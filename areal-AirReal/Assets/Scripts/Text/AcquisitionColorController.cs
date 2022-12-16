using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Linq;
using  UnityEngine.UI;
using TMPro;
public class AcquisitionColorController : MonoBehaviour
{
    public Color32 color;
    
    private Texture tex;
    private Texture2D texture2D;

    private Texture2D targetTexture;

    public string _text;

    public Dictionary<string, Color> word_List = new Dictionary<string, Color>();
    private int cnt;
    
    [SerializeField] private GameObject TextObj;
    [SerializeField] private Vector3 position;

    private TouchScreenKeyboard keyboard;
    private GameObject textObj1;

    private List<Color> colorList = new List<Color>();
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
                    
                    var png = texture2D.EncodeToPNG();
                    File.WriteAllBytes(Application.dataPath +"/paint.png", png);
                    
                    StartCoroutine(GetColorCoroutine((int)touchPos.x, (int)touchPos.y,hit));
                    
                    cnt++;

                }
                
            }

        }

        if (textObj1 != null)
        {
            textObj1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.keyboard.text + cnt;
        }
    }

    private IEnumerator GetColorCoroutine(int x, int y ,RaycastHit hit)

    {

        yield return new WaitForEndOfFrame();

        targetTexture.ReadPixels(new Rect(x,y, 1, 1), 0, 0);

        color = targetTexture.GetPixel(0, 0);

        var judgement = JudgmentColor(color, colorList);
        if (judgement)
        {
            Destroy(textObj1);
        }
        else
        {
            Debug.Log(hit.textureCoord);
            position = new Vector3(hit.textureCoord.x - 0.5f, hit.textureCoord.y - 0.5f, 0);
            textObj1 = Instantiate(TextObj, Vector3.zero, Quaternion.identity);
            textObj1.transform.parent = hit.transform.GetChild(0);
            //textObj1.GetComponent<RectTransform>().position = position;
            textObj1.transform.localPosition = position;
            this.keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
            textObj1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.keyboard.text + cnt;
            _text = textObj1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            word_List.Add(_text, color);
            colorList.Add(color);
            Debug.Log(color);
            Debug.Log((_text));
        }

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

    private bool JudgmentColor(Color color,List<Color> colorList)
    {
        bool Judgment = false;
        foreach(Color saveColor in colorList)
        {
            float diffR = Mathf.Abs(saveColor.r - color.r);
            float diffG = Mathf.Abs(saveColor.g - color.g);
            float diffB = Mathf.Abs(saveColor.b - color.b);
            if (diffR <= 0.1f && diffG <= 0.1f && diffB <= 0.1f)
            {
                // 同じ色に近い場合は、何もしない
                Judgment = true;
                Debug.Log(Judgment);
                break;
            }
            Debug.Log(Judgment);
        }
        return Judgment;

    }
}
