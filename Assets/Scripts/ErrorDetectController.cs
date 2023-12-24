using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDetectController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BallController>() != null)
        {
            // 检测到球进入不该出现的区域，将球送回并道歉
            GameManager.isGameStarted = false;
            GameManager.currentState = GameManager.GameState.ERROR;
        }
    }
}
