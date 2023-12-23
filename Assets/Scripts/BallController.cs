using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private ColorComponent colorComp;

    // 难度对应的速度
    public float easySpeed = 0.05f;
    public float hardSpeed = 0.10f;

    // 额定最大速度
    private const float maxSpeed = 0.4f;
    // 一次bounce增加的速度
    private const float unitSpeed = 0.01f;

    // 当前基础速度（难度决定）
    private float baseSpeed;
    // 当前实际速度
    private float currentSpeed;

    // 弹射次数
    private int bounceCount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colorComp = GetComponent<ColorComponent>();
        colorComp.ResetColor();
    }

    private void Restart()
    {
        currentSpeed = 0.0f;
        bounceCount = 0;
    }

    private void FixedUpdate()
    {
        if (!GameManager.GameStarted) return;

        // 根据难度决定速度
        baseSpeed = GameManager.CurrentHardness == GameManager.Hardness.EASY ? easySpeed : hardSpeed;

        // 将基础速度乘以速度加成
        currentSpeed = baseSpeed * (colorComp.GetColorCount() + 1) + unitSpeed * bounceCount;

        // 速度最小为基础速度，最大为额定速度
        currentSpeed = Mathf.Clamp(currentSpeed, baseSpeed, maxSpeed);

        Debug.Log("<BallController>: 球当前速度为"+ currentSpeed);

        // 移动球
        transform.Translate(currentSpeed, 0.0f, 0.0f);
    }

    private void Update()
    {
        if (!GameManager.GameStarted)
        {
            Restart();
            // 当按下鼠标左键时游戏开始
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.GameStarted = true;
                Debug.Log("<BallController>: 游戏开始");
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("<BallController>: 与物体" + collision.gameObject.name + "碰撞");
        // 将角度制转换为弧度制   
        float Angle_Z = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        // 拿到旋转向量，以便之后进行反射计算
        Vector2 Direction = new Vector2(Mathf.Cos(Angle_Z), Mathf.Sin(Angle_Z));
        //Debug.Log(Direction);

        // 两种情况，一是原路返回，二是镜面反射
        if(Random.Range(0, 3) == 0)
        {
            // 将Z的角度直接改为负值
            Angle_Z += Mathf.PI;
        }
        else
        {
            // 做反射运算，得到反射后的选择向量
            Vector2 Out_Direction = Vector2.Reflect(Direction, collision.GetContact(0).normal);
            //Debug.Log(Out_Direction);

            // 向旋转向量转为弧度制
            Angle_Z = Mathf.Atan2(Out_Direction.y, Out_Direction.x);
            //Debug.Log(Angle_Z);
        }

        // 对方向进行略微偏转


        // 重新转换回角度制，并修改子弹角度，达成反射效果
        transform.rotation = Quaternion.Euler(0, 0, Angle_Z * Mathf.Rad2Deg);

        GameObject objCollision = collision.gameObject;

        // 根据碰撞物颜色改变自身颜色和速度
        if (objCollision.CompareTag("Player"))
        {
            // 碰到弹板，根据对应颜色判断行动
            if(ColorManager.CompareColor(colorComp, objCollision.GetComponent<ColorComponent>()))
            {
                Debug.Log("<BallController>: 正常接触Lord");
                // 重置bounce次数
                bounceCount = 0;

                // 重置颜色
                colorComp.ResetColor();

                // 积分增加，为带回的颜色数乘当前轮次
                GameManager.CurrentScore += colorComp.GetColorCount() * GameManager.CurrentRound;
            }
            else
            {
                Debug.Log("<BallController>: 接触到颜色错误的Lord");
                // bounce次数反增
                bounceCount++;

                // 进入归还者模式
                GameManager.isGameStateNormal = false;


                // 吸走弹板颜色
            }
        }

        if (objCollision.CompareTag("Bound"))
        {

            // 碰到边界，正常增加bounce
            bounceCount++;
        }

        if (objCollision.CompareTag("Brick"))
        {
            // 碰到砖块，正常增加bounce
            bounceCount++;

            // 根据目前模式进行吸收或释放
            if (GameManager.isGameStateNormal)
            {

            }
            else
            {

            }

            // 消掉砖块，积分加等于当前轮数
            GameManager.CurrentScore += GameManager.CurrentRound;
        }
    }
}
