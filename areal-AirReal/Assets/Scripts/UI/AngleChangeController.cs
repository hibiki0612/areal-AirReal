using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleChangeController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject frameObj;


    [SerializeField]
    [Tooltip("xŽ²‚Ì‰ñ“]Šp“x")]
    private float rotateX = 0;

    [SerializeField]
    [Tooltip("yŽ²‚Ì‰ñ“]Šp“x")]
    private float rotateY = 0;

    [SerializeField]
    [Tooltip("zŽ²‚Ì‰ñ“]Šp“x")]
    private float rotateZ = 0;
    // Update is called once per frame
    void Update()
    {
        frameObj.transform.Rotate(new Vector3(rotateX, rotateY, rotateZ) * Time.deltaTime);
    }
}
