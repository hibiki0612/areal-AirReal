using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AppearLoading : MonoBehaviour
{
    private GameObject TargetObject;
    [SerializeField] private PaintCanvasCreate paintCanvasCreate;

    public void SaveObject()
    {
        TargetObject = paintCanvasCreate.TargetObj;
    }

    public void LoadingAppearButton()
    {
        TargetObject.transform.GetChild(4).gameObject.SetActive(true);
        var obj = TargetObject.transform.GetChild(4).gameObject;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(0.5f, 3f);
    }
    
    public  void LoadingDisAppear()
    {
        TargetObject.transform.GetChild(4).gameObject.SetActive(true);
        var obj = TargetObject.transform.GetChild(4).gameObject;
        obj.transform.DOScale(0f, 2f);
    }
}
