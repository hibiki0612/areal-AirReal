using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AppearAnimation : MonoBehaviour
{
    void Start()
    {
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(1, 3f).SetEase(Ease.OutBack);
        transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360)
    .SetEase(Ease.OutCubic);
    

    }
}
