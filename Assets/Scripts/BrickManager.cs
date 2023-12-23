using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public GameObject brickUnit;

    // 单个方块的宽高
    private float unitWidth;
    private float unitHeight;

    // 生成区域的宽高，指方块数量
    public int areaWidth;
    public int areaHeight;

    // 控制生成的R/G/B单种颜色的概率，0~1之间
    public float rate = 0.4f;

    // 若三种颜色均未生成，是否生成为暗方块
    public bool createInitialDarkBrick = false;

    // 所有生成出的砖块，无论虚实和亮暗
    private List<GameObject> bricks;

    // 亮色方块数量
    private int aliveCount;

    private void Awake()
    {
        unitWidth = brickUnit.transform.localScale.x;
        unitHeight = brickUnit.transform.localScale.y;

        bricks = new List<GameObject>(areaWidth * areaHeight);
        aliveCount = 0;

        for (int h = 0; h < areaHeight; h++)
        {
            for (int w = 0; w < areaWidth; w++)
            {
                // 在指定位置生成砖块
                GameObject newUnit = Instantiate(brickUnit, new Vector3(transform.position.x + w * unitWidth, transform.position.y - h * unitHeight, 0),
                    Quaternion.identity, transform);

                // 随机生成颜色
                float r = UnityEngine.Random.Range(0.0f, 1.0f);
                float g = UnityEngine.Random.Range(0.0f, 1.0f);
                float b = UnityEngine.Random.Range(0.0f, 1.0f);
                newUnit.GetComponent<BrickController>().Initialize(h, w, r < rate, g < rate, b < rate, this);

                // 计数亮色方块
                if (newUnit.GetComponent<BrickController>().isAlive) aliveCount++;

                // 根据要求将暗方块销毁
                if (!createInitialDarkBrick && !newUnit.GetComponent<BrickController>().isAlive) newUnit.SetActive(false);

                // 存储新砖块
                bricks.Add(newUnit);
            }
        }
    }

    /// <summary>
    /// 更新亮色砖块统计值
    /// </summary>
    public void AliveUpdate(bool isAdd)
    {
        if (isAdd) aliveCount++;
        else
        {
            aliveCount--;
            // 判定是否完成此轮
            if(aliveCount == 0)
            {
                GameManager.GameStarted = false;
                GameManager.currentState = GameManager.GameState.WON;
                Respawn();
            }
        }
    }

    private void Respawn()
    {
        aliveCount = 0;

        for (int h = 0; h < areaHeight; h++)
        {
            for (int w = 0; w < areaWidth; w++)
            {
                // 在指定位置重置砖块
                bricks[w + h * areaWidth].SetActive(true);

                // 随机生成颜色
                float r = UnityEngine.Random.Range(0.0f, 1.0f);
                float g = UnityEngine.Random.Range(0.0f, 1.0f);
                float b = UnityEngine.Random.Range(0.0f, 1.0f);

                bricks[w + h * areaWidth].GetComponent<BrickController>().Initialize(h, w, r < rate, g < rate, b < rate, this);

                // 计数亮色方块
                if (bricks[w + h * areaWidth].GetComponent<BrickController>().isAlive) aliveCount++;

                // 根据要求将暗方块销毁
                if (!createInitialDarkBrick && !bricks[w + h * areaWidth].GetComponent<BrickController>().isAlive) bricks[w + h * areaWidth].SetActive(false);
            }
        }
    }
}
