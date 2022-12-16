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

            if (Input.touchCount == 1)
            {
                Vector2 delta = touch.deltaPosition * 0.001f;
                if ((delta.x > -1 && delta.y > -1) && (delta.x < 1 && delta.y < 1))
                {


                    Vector3 moveForward = Camera.main.transform.up * delta.y + Camera.main.transform.right * delta.x;

                    obj.transform.position += moveForward;

                    obj.transform.rotation = Camera.main.transform.rotation;

                }
            }
            
            else if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                {
                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                    Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                    float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
                    float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                    float deltaMagnitudeDiff =  touchDeltaMag - prevTouchDeltaMag;
                    
                    if (obj.transform.localScale.x > 0.5f)
                    {
                        obj.transform.localScale += Vector3.one * deltaMagnitudeDiff * Time.deltaTime* 0.25f;
                    }
                    else
                    {
                        if (deltaMagnitudeDiff > 0)
                        {
                            obj.transform.localScale += Vector3.one * deltaMagnitudeDiff * Time.deltaTime* 0.25f;
                        }
                    }
                    
                }
            }
            
            
            /*
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
            */
        }
        

    }
}