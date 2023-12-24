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
            if(GameManager.CurrentScore >= GameManager.CurrentRound * GameManager.RelifeCost)
            {
                Debug.Log("<DeadlineController>: 进入死区，但积分足够，自动复活");
                GameManager.isGameStarted = false;
                GameManager.currentState = GameManager.GameState.RELIFE;
                // 复活的代价是积分减少
                GameManager.CurrentScore -= GameManager.CurrentRound * GameManager.RelifeCost;
            }
            else
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
}
