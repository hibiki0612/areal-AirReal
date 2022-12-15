using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextAndColorSave : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private string text;
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
                    color = this.gameObject.GetComponent<AcquisitionColorController>().color;
                    text = this.gameObject.GetComponent<InputTextController>()._text;
                    
                }
            }
        }
    }
}
