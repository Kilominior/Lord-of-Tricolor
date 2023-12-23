using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorManager
{
    public const byte BrightColorValue = 220;
    public const byte DarkColorValue = 30;

    /// <summary>
    /// 比较两个Color组件的颜色是否一致
    /// </summary>
    public static bool CompareColor(ColorComponent compA, ColorComponent compB)
    {
        if(compA.R == compB.R && compA.G == compB.G && compA.B == compB.B) { return true; }
        return false;
    }

    /// <summary>
    /// 将目标砖块的颜色吸收到球上，砖块和球均变色
    /// </summary>
    /// <param name="compOfBall">球</param>
    /// <param name="compOfBrick">目标砖块</param>
    /// <returns>目标砖块是否被销毁</returns>
    public static bool AbsorbColor(ColorComponent compOfBall, ColorComponent compOfBrick)
    {
        if (!compOfBall.R && compOfBrick.R)
        {
            compOfBall.R = true; compOfBrick.R = false;
        }
        if(!compOfBall.G && compOfBrick.G)
        {
            compOfBall.G = true; compOfBrick.G = false;
        }
        if(!compOfBall.B && compOfBrick.B)
        {
            compOfBall.B = true; compOfBrick.B = false;
        }

        if (!compOfBrick.R && !compOfBrick.G && !compOfBrick.B) return true;
        return false;
    }

    /// <summary>
    /// 将球的颜色释放到目标砖块上，球不变色
    /// </summary>
    /// <param name="compOfBall">球</param>
    /// <param name="compOfBrick">目标砖块</param>
    public static void ReleaseColor(ColorComponent compOfBall, ColorComponent compOfBrick)
    {
        if (compOfBall.R && !compOfBrick.R) compOfBrick.R = true;
        if (compOfBall.G && !compOfBrick.G) compOfBrick.G = true;
        if (compOfBall.B && !compOfBrick.B) compOfBrick.B = true;
    }

    /// <summary>
    /// 从弹板上吸走颜色，弹板不受影响
    /// </summary>
    /// <param name="compOfBall">球</param>
    /// <param name="compOfLord">弹板</param>
    public static void RobColor(ColorComponent compOfBall, ColorComponent compOfLord)
    {
        if (!compOfBall.R && compOfLord.R) compOfBall.R = true;
        if (!compOfBall.G && compOfLord.G) compOfBall.G = true;
        if (!compOfBall.B && compOfLord.B) compOfBall.B = true;
    }
}
