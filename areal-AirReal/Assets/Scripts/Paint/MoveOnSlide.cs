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
                // �Փ˂����I�u�W�F�N�g���擾
                GameObject obj = hit.collider.gameObject;

                // �^�b�`�̈ړ��ʂ��擾
                Vector2 delta = touch.deltaPosition * 0.001f;

                obj.transform.position += new Vector3(delta.x,delta.y,0);

                obj.transform.rotation = Camera.main.transform.rotation;
            }
        }
    }
}