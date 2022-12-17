using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CameraAnimation : MonoBehaviour
{
    
    [SerializeField] private GameObject CanvasQuad;
    [SerializeField] private GameObject PaintQuad;
    
    [SerializeField] private GameObject Content;

    [SerializeField] private GameObject ScrollView;

    public void cameraAnimationStart()
    {
        Vector3[] path = {
            new Vector3(0f, -1.0f, -2.5f),
            //new Vector3(0f, 0.0f, -3.0f),
            new Vector3(0f, -2.0f, -1.50f),
            new Vector3(0f, -2.0f, 1.50f),

        };

        this.transform.DOLocalPath(path, 2f, PathType.CatmullRom).SetEase(Ease.OutSine);
        MoveButton();
    } 
    
    public void MoveButton()
    {
        CanvasQuad.GetComponent<MeshRenderer>().material.mainTexture =
            PaintQuad.GetComponent<MeshRenderer>().material.mainTexture;
    }

    
    private void Update()
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
                    cameraAnimationStart();
                    ScrollView.SetActive(false);
                }
            }
        }
    }
}
