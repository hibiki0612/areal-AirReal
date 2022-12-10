using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PaintCanvasCreate : MonoBehaviour
{
    [SerializeField] private GameObject PaintCanvas;
    private Transform mainCamera;
    [SerializeField] private Slider DistanceSlider;
    
    // Start is called before the first frame update
    void Start()
    {

        mainCamera = Camera.main.transform　;
        
    }

    // Update is called once per frame
    public void CreatePaintCanvas()
    {
        
        var position = mainCamera.position + mainCamera.forward * DistanceSlider.value;
        Instantiate(PaintCanvas, position, mainCamera.rotation);
    }
}
