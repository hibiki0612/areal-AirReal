using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSlide : MonoBehaviour
{
    void Update()
    {
        // �^�b�`����Ă���ꍇ
        if (Input.touchCount > 0)
        {
            // �ŏ��Ƀ^�b�`���ꂽ���̂��擾
            Touch touch = Input.touches[0];

            // �^�b�`���ꂽ�ʒu����Ray���΂�
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            // Ray���Փ˂����I�u�W�F�N�g�����邩����
            if (Physics.Raycast(ray, out hit))
            {
                
                GameObject obj = hit.collider.gameObject;

                Vector2 delta = touch.deltaPosition * 0.001f;
                if ((delta.x > -1 && delta.y > -1) && (delta.x < 1 && delta.y < 1))
                {
                    
                    // 方向キーの入力値とカメラの向きから、移動方向を決定
                    Vector3 moveForward = Camera.main.transform.up * delta.y + Camera.main.transform.right * delta.x;
                    
                    obj.transform.position += moveForward;

                    obj.transform.rotation = Camera.main.transform.rotation;
                    Debug.Log(moveForward);
                }
                
            }
        }
    }
}