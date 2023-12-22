using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordController : MonoBehaviour
{
    public Transform BoundL;
    public Transform BoundR;

    private float edgeL;
    private float edgeR;

    private void Start()
    {
        edgeL = BoundL.position.x + BoundL.localScale.x / 2 + transform.localScale.x / 2;
        edgeR = BoundR.position.x - BoundR.localScale.x / 2 - transform.localScale.x / 2;
        //Debug.Log(edgeL + " " + edgeR);
    }

    // Update is called once per frame
    void Update()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mousePos = Input.mousePosition;

        // 将鼠标在屏幕上的位置转换为世界空间中的位置
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));

        // 限制位置范围
        float xPos = mouseWorldPos.x;
        xPos = Mathf.Clamp(xPos, edgeL, edgeR);

        // 更新物体位置
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
}
