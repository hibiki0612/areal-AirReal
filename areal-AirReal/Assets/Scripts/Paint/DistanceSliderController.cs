using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceSliderController : MonoBehaviour
{
    private GameObject obj;
    [SerializeField] private PaintCanvasCreate paintCanvasCreate;
    [SerializeField] Slider distanceSlider;
    // Start is called before the first frame update
    void Start()
    {
        obj = paintCanvasCreate.TargetObj;
        distanceSlider.value = obj.transform.position.z;
    }

    void Update()
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, distanceSlider.value);
    }
}
