using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    private const int NormalLayer = 3;
    private const int DarkLayer = 6;

    // 行列位置
    private int row;
    private int col;

    // 是否为亮色方块
    public bool isAlive;

    // 颜色
    private ColorComponent colorComp;
    private BrickManager brickManager;

    /// <summary>
    /// 初始化砖块状态
    /// </summary>
    public void Initialize(int row, int col, bool R, bool G, bool B, BrickManager brickManager)
    {
        colorComp = this.GetComponent<ColorComponent>();
        this.row = row;
        this.col = col;
        colorComp.ColorUpdateTo(R, G, B);
        isAlive = (colorComp.R || colorComp.G || colorComp.B);
        gameObject.layer = isAlive ? NormalLayer : DarkLayer;
        this.brickManager = brickManager;
    }

    /// <summary>
    /// 将砖块变为暗色砖块
    /// </summary>
    public void Destory()
    {
        Debug.Log("<BrickController>: 砖块" + name + "转为暗色");
        isAlive = false;
        gameObject.layer = DarkLayer;
        brickManager.AliveUpdate(false);
    }

    /// <summary>
    /// 将砖块恢复为亮色砖块
    /// </summary>
    public void Relife()
    {
        Debug.Log("<BrickController>: 砖块"+ name + "转为亮色");
        isAlive = true;
        gameObject.layer = NormalLayer;
        brickManager.AliveUpdate(true);
    }
}
