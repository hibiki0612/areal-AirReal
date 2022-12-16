using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadingAnimation : MonoBehaviour
{

    void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 360f), 6f, RotateMode.FastBeyond360)
    .SetEase(Ease.Linear)
    .SetLoops(-1, LoopType.Restart);
    }


}
