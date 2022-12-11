using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSlide : MonoBehaviour
{
    void Update()
    {
        // タッチされている場合
        if (Input.touchCount > 0)
        {
            // 最初にタッチされたものを取得
            Touch touch = Input.touches[0];

            // タッチされた位置からRayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            // Rayが衝突したオブジェクトがあるか判定
            if (Physics.Raycast(ray, out hit))
            {
                // 衝突したオブジェクトを取得
                GameObject obj = hit.collider.gameObject;

                // タッチの移動量を取得
                Vector2 delta = touch.deltaPosition * 0.001f;

                obj.transform.position += new Vector3(delta.x,delta.y,0);

                obj.transform.rotation = Camera.main.transform.rotation;
            }
        }
    }
}