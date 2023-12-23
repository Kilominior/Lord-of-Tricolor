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
    public float rate = 0.5f;

    // 若三种颜色均未生成，是否生成为暗方块
    public bool createInitialDarkBrick = false;


}
