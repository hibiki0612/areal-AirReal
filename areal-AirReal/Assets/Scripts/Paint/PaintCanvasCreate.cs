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

    public GameObject TargetObj;

    [SerializeField] private GameObject AssumedLocation;
    // Start is called before the first frame update
    void Start()
    {

        mainCamera = Camera.main.transform　;
        AssumedLocation.SetActive(true);
    }

    private void Update()
    {
        var position = mainCamera.position + mainCamera.forward * DistanceSlider.value;
        AssumedLocation.transform.position = position;
        AssumedLocation.transform.rotation = mainCamera.rotation;
    }


    // Update is called once per frame
    public void CreatePaintCanvas()
    {
        AssumedLocation.SetActive(false);
        var position = mainCamera.position + mainCamera.forward * DistanceSlider.value;
        TargetObj = Instantiate(PaintCanvas, position, mainCamera.rotation);
    }
}
