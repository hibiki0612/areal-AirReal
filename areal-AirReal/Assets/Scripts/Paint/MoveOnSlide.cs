using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class MoveOnSlide : MonoBehaviour
{

    public GameObject obj;
    [SerializeField] private PaintCanvasCreate paintCanvasCreate;
    public void Start()
    {
        obj = paintCanvasCreate.TargetObj;

    }
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            #if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject()) return;
            #else
	            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))return;
            #endif

            Touch touch = Input.touches[0];

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                obj = hit.collider.gameObject;

                Vector2 delta = touch.deltaPosition * 0.001f;
                if ((delta.x > -1 && delta.y > -1) && (delta.x < 1 && delta.y < 1))
                {


                    Vector3 moveForward = Camera.main.transform.up * delta.y + Camera.main.transform.right * delta.x;

                    obj.transform.position += moveForward;

                    obj.transform.rotation = Camera.main.transform.rotation;
                        
                }
                
               
                
            }
        }
        

    }
}