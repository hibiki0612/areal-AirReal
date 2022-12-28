using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Linq;
using  UnityEngine.UI;
using TMPro;
using UnityEditor;
public class AcquisitionColorController : MonoBehaviour
{
    public Color32 color;
    
    private Texture tex;
    private Texture2D texture2D;

    private Texture2D targetTexture;

    public string _text;

    public Dictionary<string, Color32> word_List = new Dictionary<string, Color32>();
    private int cnt;
    public List<string> word; 
    
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
                    
                    Debug.Log(hit.textureCoord);
                    position = new Vector3(hit.textureCoord.x - 0.5f, hit.textureCoord.y - 0.5f , 0);
                    textObj1 = Instantiate(TextObj,Vector3.zero,Quaternion.identity);
                    textObj1.transform.parent = hit.transform.GetChild(0);
                    //textObj1.GetComponent<RectTransform>().position = position;
                    textObj1.transform.localPosition = position;
                    this.keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

                    cnt++;
                    textObj1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.keyboard.text;
                    _text = textObj1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

                    tex = hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture;
                    texture2D = ToTexture2D(tex);


                    var png = texture2D.EncodeToPNG();
                    File.WriteAllBytes(Application.persistentDataPath +"/paint.png", png);
                    
                    StartCoroutine(GetColorCoroutine((int)touchPos.x, (int)touchPos.y));
                    
                    
                }
                
            }

        }

        if (textObj1 != null && this.keyboard != null )
        {
            textObj1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.keyboard.text;
        }
        
        if (this.keyboard != null && this.keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            word.Add(this.keyboard.text);
            print(this.keyboard.text);
            this.keyboard = null;
        }

    }

    private IEnumerator GetColorCoroutine(int x, int y)

    {

        yield return new WaitForEndOfFrame();

        targetTexture.ReadPixels(new Rect(x,y, 1, 1), 0, 0);

        color = targetTexture.GetPixel(0, 0);

        var judgement = JudgmentColor(color, colorList);
        
        word_List.Add(_text + cnt.ToString(), color);
        colorList.Add(color);
        Debug.Log(color);
        Debug.Log((_text));
        

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
                // �����F�ɋ߂��ꍇ�́A�������Ȃ�
                Judgment = true;
                Debug.Log(Judgment);
                break;
            }
            Debug.Log(Judgment);
        }
        return Judgment;

    }
}
