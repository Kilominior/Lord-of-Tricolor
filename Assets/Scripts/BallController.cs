using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private const int LayerAbsorb = 7;
    private const int LayerRelease = 8;

    private ColorComponent colorComp;

    public GameObject boundAbsorb;
    public GameObject boundRelease;

    // 难度对应的速度
    public float easySpeed = 0.05f;
    public float hardSpeed = 0.08f;

    // 额定最大速度
    private const float maxSpeed = 0.30f;
    // 一次bounce增加的速度
    private const float unitSpeed = 0.01f;

    // 当前基础速度（难度决定）
    private float baseSpeed;
    // 当前实际速度
    private float currentSpeed;

    // 单次随机角度偏转最大值
    public float maxAngleBias = 15.0f;

    // 弹射次数
    private int bounceCount;

    private void Start()
    {
        colorComp = GetComponent<ColorComponent>();
        colorComp.ResetColor();
    }

    private void Restart()
    {
        currentSpeed = 0.0f;
        bounceCount = 0;
        GameManager.isGameModeNormal = true;
        ModeChange();
    }

    private void ModeChange()
    {
        Debug.Log("<BallController>: 模式切换 -> " + (GameManager.isGameModeNormal ? "吸收者模式" : "归还者模式"));
        if(GameManager.isGameModeNormal)
        {
            boundAbsorb.SetActive(true);
            boundRelease.SetActive(false);
            gameObject.layer = LayerAbsorb;
        }
        else
        {
            boundAbsorb.SetActive(false);
            boundRelease.SetActive(true);
            gameObject.layer = LayerRelease;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.GameStarted) return;

        // 根据难度决定速度
        baseSpeed = GameManager.GameHardness == GameManager.Hardness.EASY ? easySpeed : hardSpeed;

        // 将基础速度乘以速度加成
        if (colorComp.GetColorCount() == 0) currentSpeed = baseSpeed + unitSpeed * bounceCount;
        else
        {
            currentSpeed = baseSpeed * (colorComp.GetColorCount() + 1) * 0.75f;
            // 随着基础速度上升，每次bounce的速度增加会有所衰减
            currentSpeed += unitSpeed * bounceCount * (1 / colorComp.GetColorCount());
        }

        // 速度最小为基础速度，最大为额定值
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
        // Debug.Log("<BallController>: 与物体" + collision.gameObject.name + "碰撞");
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
            Vector2 outDirection = Vector2.Reflect(Direction, collision.GetContact(0).normal);
            //Debug.Log(Out_Direction);

            // 向旋转向量转为弧度制
            Angle_Z = Mathf.Atan2(outDirection.y, outDirection.x);
            //Debug.Log(Angle_Z);
        }

        // 重新转换回角度制
        float NewEularZ = Angle_Z * Mathf.Rad2Deg;

        // 对方向进行随机略微偏转
        NewEularZ += Random.Range(-maxAngleBias, maxAngleBias);

        // 限制偏转结果在法线的90度范围内，避免打入物体内部
        Vector2 newDirection = new Vector2(Mathf.Cos(Angle_Z), Mathf.Sin(Angle_Z));
        if(Vector2.Angle(collision.GetContact(0).normal, newDirection) >= 90)
            NewEularZ = Angle_Z * Mathf.Rad2Deg;

        // 修改子弹角度，达成反射效果
        transform.rotation = Quaternion.Euler(0, 0, NewEularZ);

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
                GameManager.CurrentScore += colorComp.GetColorCount() * GameManager.CurrentRound
                    * (GameManager.GameHardness == GameManager.Hardness.HARD ? 2 : 1);

                // 恢复吸收者模式
                if (!GameManager.isGameModeNormal)
                {
                    GameManager.isGameModeNormal = true;
                    ModeChange();
                }
            }
            else
            {
                Debug.Log("<BallController>: 接触到颜色错误的Lord");
                // bounce次数反增
                bounceCount++;

                // 进入归还者模式
                if (GameManager.isGameModeNormal)
                {
                    GameManager.isGameModeNormal = false;
                    ModeChange();
                }

                // 吸走弹板颜色
                ColorManager.RobColor(colorComp, objCollision.GetComponent<ColorComponent>());
            }
            return;
        }

        if (objCollision.CompareTag("Bound"))
        {
            Debug.Log("<BallController>: 接触边界");
            // 碰到边界，正常增加bounce
            bounceCount++;
            return;
        }

        if (objCollision.CompareTag("Brick"))
        {
            // 碰到砖块，正常增加bounce
            bounceCount++;

            // 根据目前模式进行吸收或释放
            if (GameManager.isGameModeNormal)
            {
                Debug.Log("<BallController>: 接触砖块" + objCollision.name + "并尝试吸收");
                if (ColorManager.AbsorbColor(colorComp, objCollision.GetComponent<ColorComponent>()))
                {
                    // 消掉砖块，积分加等于当前轮数乘以难度倍率
                    objCollision.GetComponent<BrickController>().Destory();
                    GameManager.CurrentScore += GameManager.CurrentRound * (GameManager.GameHardness == GameManager.Hardness.HARD ? 2 : 1);
                }
            }
            else
            {
                Debug.Log("<BallController>: 接触砖块" + objCollision.name + "并释放颜色");
                // 复活砖块（若为暗色），并将颜色传递给接触的砖块
                if(!objCollision.GetComponent<BrickController>().isAlive)
                    objCollision.GetComponent<BrickController>().Relife();
                ColorManager.ReleaseColor(colorComp, objCollision.GetComponent <ColorComponent>());
            }
            return;
        }

        if (objCollision.CompareTag("Respawn"))
        {
            Debug.Log("<BallController>: 进入死区，游戏失败！");
            GameManager.GameStarted = false;
        }
    }
}
