using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectRotationAnimation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360f, 0f), rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}
