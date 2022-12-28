using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MozaicController : MonoBehaviour
{
    
    public float span = 1f;
    private float currentTime = 0f;
    private int cnt = 64;


    [SerializeField] private GameObject TargetObject;
    // Update is called once per frame

    [SerializeField] private Material before_material;
    [SerializeField] private Material after_material;
    [SerializeField] private Material _material;
    [SerializeField] private Texture _texture;
    private bool active = false;
    void Update()
    {
        if (cnt >= 5 && !active)
        {
            currentTime += Time.deltaTime;

            if (currentTime > span / 20)
            {
                TargetObject.GetComponent<MeshRenderer>().material = before_material;
                TargetObject.GetComponent<MeshRenderer>().material.mainTexture = _texture;
                TargetObject.GetComponent<MeshRenderer>().material.SetFloat("_MosaicResolution", cnt);
                currentTime = 0f;
                if (cnt <= 5)
                {
                    //cnt = 2;
                    active = true;
                }

                else
                {
                    cnt--;
                    
                }
                
            }

        }
        
        else if (cnt <= 64 && active)
        {
            currentTime += Time.deltaTime;

            if (currentTime > span / 20)
            {
                TargetObject.GetComponent<MeshRenderer>().material = after_material;
                TargetObject.GetComponent<MeshRenderer>().material.SetFloat("_MosaicResolution", cnt);
                currentTime = 0f;
                if (cnt >= 60)
                {
                    //cnt  = 64;
                    TargetObject.GetComponent<MeshRenderer>().material = _material;

                }
 
                else
                {
                    cnt++;
                }
                

            }

        }

    }
}
