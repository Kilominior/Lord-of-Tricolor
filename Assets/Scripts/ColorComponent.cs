using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorComponent: MonoBehaviour
{
    private bool r;
    public bool R
    {
        set { r = value; RendererUpdate(); }
        get { return r; }
    }

    private bool g;
    public bool G
    {
        set { g = value; RendererUpdate(); }
        get { return g; }
    }

    private bool b;
    public bool B
    {
        set { b = value; RendererUpdate(); }
        get { return b; }
    }

    public SpriteRenderer spriteRenderer;

    ColorComponent()
    {
        r = false; g = false; b = false;
    }

    public void ColorUpdateTo(bool r, bool g, bool b)
    {
        R = r; G = g; B = b;
    }

    public void ResetColor()
    {
        R = false; G = false; B = false;
    }

    public int GetColorCount()
    {
        int count = 0;
        count += R ? 1 : 0;
        count += G ? 1 : 0;
        count += B ? 1 : 0;
        return count;
    }

    // 同步颜色状态到Renderer
    private void RendererUpdate()
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("<ColorComponent>: 未绑定SpriteRenderer！");
            return;
        }
        spriteRenderer.color = new Color32(R ? ColorManager.BrightColorValue : ColorManager.DarkColorValue,
            G ? ColorManager.BrightColorValue : ColorManager.DarkColorValue, B ? ColorManager.BrightColorValue : ColorManager.DarkColorValue, 255);
    }
}
