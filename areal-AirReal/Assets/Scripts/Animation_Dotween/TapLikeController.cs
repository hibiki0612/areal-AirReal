using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TapLikeController : MonoBehaviour
{

    [SerializeField] private GameObject Like;
    // Update is called once per frame
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
                    var position = new Vector3(hit.transform.position.x, hit.transform.position.y - 0.8f, hit.transform.position.z);
                    Instantiate(Like, position, Quaternion.identity);

                }

            }
        }
    }
}
