using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputTextController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private GameObject TextObj;
     
    void Start()
    {

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
                    
                    var position = new Vector3(hit.textureCoord.x, hit.textureCoord.y, 0);
                    var textObj1 = Instantiate(TextObj);
                    textObj1.GetComponent<RectTransform>().position = position;
                    textObj1.transform.parent = hit.transform.GetChild(0);
                    
                }
            }
        }
    }
}
