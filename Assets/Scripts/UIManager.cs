using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text bestScore;
    public Text currentScore;
    public Text bestRound;
    public Text currentRound;
    public Button easyBtn;
    public Button hardBtn;

    private void Awake()
    {
        // 更新难度按键状态
        if(GameManager.CurrentHardness == GameManager.Hardness.EASY)
        {
            easyBtn.interactable = false;
            hardBtn.interactable = true;
        }
        else
        {
            easyBtn.interactable = true;
            hardBtn.interactable = false;
        }

        // 注册按键行为
        easyBtn.onClick.AddListener(() =>
        {
            GameManager.CurrentHardness = GameManager.Hardness.EASY;
            easyBtn.interactable = false;
            hardBtn.interactable = true;
        });

        hardBtn.onClick.AddListener(() =>
        {
            GameManager.CurrentHardness = GameManager.Hardness.HARD;
            hardBtn.interactable = false;
            easyBtn.interactable = true;
        });
    }

    private void Update()
    {
        // 鼠标右键切换难度
        if (Input.GetMouseButtonDown(1))
        {
            if(GameManager.CurrentHardness == GameManager.Hardness.HARD)
            {
                GameManager.CurrentHardness = GameManager.Hardness.EASY;
                easyBtn.interactable = false;
                hardBtn.interactable = true;
            }
            else
            {
                GameManager.CurrentHardness = GameManager.Hardness.HARD;
                hardBtn.interactable = false;
                easyBtn.interactable = true;
            }
        }

        // 更新分数信息
        bestScore.text = GameManager.MaxScore.ToString();
        bestRound.text = GameManager.MaxRound.ToString();
        currentScore.text = GameManager.CurrentScore.ToString();
        currentRound.text = GameManager.CurrentRound.ToString();
    }
}
