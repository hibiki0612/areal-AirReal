using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputTextController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    
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
            var TouchWorldPos = Camera.main.ScreenToWorldPoint(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "PaintCanvas")
                {
                    var position = new Vector3(TouchWorldPos.x, TouchWorldPos.y, -0.01f);
                    var text1 = Instantiate(text, position, Quaternion.identity);
                    text1.transform.parent = hit.transform.GetChild(0);
                    text1.transform.position = TouchWorldPos;
                }
            }
        }
    }
}
