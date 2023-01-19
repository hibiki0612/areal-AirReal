using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveLeft : MonoBehaviour
{
    public float radius = 2.0f;
    Vector3 pos; // 現在のSphereの位置
    MeshRenderer mesh;
    // 初期位置を取得し、高さを保持します。
    Vector3 initPos;
    void Start()
    {
        // 初期位置を保持します。
        initPos = transform.position;
        mesh = GetComponent<MeshRenderer>(); //unityのオブジェクトを取得
        StartCoroutine("Transparent"); //透明化を行うコルーチン
    }
    void Update()
    {
        CalcPosition();
    }
    /// <Summary>
    /// オブジェクトの位置を計算するメソッドです。
    /// </Summary>
    void CalcPosition()
    {
        // 位相を計算します。
        float phase = Time.time * 2 * -(Mathf.PI / 4);
        // 現在の位置を計算します。
        float xPos = initPos.x + radius * Mathf.Cos(phase);
        float yPos = initPos.y + radius * Mathf.Sin(phase);
        // ゲームオブジェクトの位置を設定します。
        Vector3 pos = new Vector3(xPos, yPos, initPos.z);
        transform.position = pos;
    }
    IEnumerator Transparent()
    {
        for (int i = 0; i < 255; i++)
        {
            mesh.material.color = mesh.material.color - new Color32(0,0,0,1); //a値を減少させ,透明に近づける.
            yield return new WaitForSeconds(0.01f); //0.01秒の待ち時間
        }
    }
}
