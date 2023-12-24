using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineController : MonoBehaviour
{
    public BrickManager brickManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BallController>() != null)
        {
            Debug.Log("<DeadlineController>: 进入死区，游戏失败！");
            GameManager.isGameStarted = false;
            GameManager.currentState = GameManager.GameState.LOST;
            // 重置游戏进度
            GameManager.CurrentRound = 1;
            GameManager.CurrentScore = 0;
            brickManager.Respawn();
        }
    }
}
