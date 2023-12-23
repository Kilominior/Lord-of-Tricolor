using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordController : MonoBehaviour
{
    public GameObject ball;
    private float ballBiasY;

    public Transform BoundL;
    public Transform BoundR;

    private float edgeL;
    private float edgeR;

    private ColorComponent colorComp;

    private void Start()
    {
        ballBiasY = ball.transform.position.y - transform.position.y;

        edgeL = BoundL.position.x + BoundL.localScale.x / 2 + transform.localScale.x / 2;
        edgeR = BoundR.position.x - BoundR.localScale.x / 2 - transform.localScale.x / 2;
        //Debug.Log(edgeL + " " + edgeR);

        colorComp = GetComponent<ColorComponent>();
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

        // 更新物体颜色
        colorComp.R = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        colorComp.G = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        colorComp.B = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (!GameManager.GameStarted)
        {
            // 游戏未开始状态下，球跟随弹板移动
            ball.transform.position = transform.position + Vector3.up * ballBiasY;
        }
    }
}
