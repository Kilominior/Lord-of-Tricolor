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
}
