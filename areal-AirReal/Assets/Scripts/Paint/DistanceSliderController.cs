using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceSliderController : MonoBehaviour
{
    private GameObject obj;
    [SerializeField] private MoveOnSlide moveOnSlide;
    [SerializeField] Slider distanceSlider;
    // Start is called before the first frame update
    void Start()
    {
        obj = moveOnSlide.obj;
        distanceSlider.value = obj.transform.position.z;
    }

    void Update()
    {
        
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, distanceSlider.value);
    }
}
