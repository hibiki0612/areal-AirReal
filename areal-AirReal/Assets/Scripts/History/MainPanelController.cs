using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainPanelController : MonoBehaviour
{
    [SerializeField] private GameObject PaintQuad;


    public void SelectButton()
    {
        Debug.Log(("test"));
        PaintQuad.GetComponent<MeshRenderer>().material.mainTexture =
            this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.texture;
    }



    
}