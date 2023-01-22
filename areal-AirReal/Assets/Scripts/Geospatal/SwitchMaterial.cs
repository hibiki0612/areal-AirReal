using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SwitchMaterial : MonoBehaviour
{
    [SerializeField] private Material _replaceTarget;
    private List<Renderer> _renderers = new List<Renderer>();
    
    private void Start() 
    {
        _renderers = GetComponentsInChildren<Renderer>().ToList();
        foreach (var renderer in _renderers)
        {
            if(renderer.transform.parent == transform) renderer.material = _replaceTarget;
        }
    }
}
