using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 加上這一行
public class GameManager : MonoBehaviour
{
    public ActionValueController playerController;
    public ActionValueController enemyController;

    // 健康值可以根據你的遊戲設計進行設定
    public int playerHealth = 100;
    public int enemyHealth = 100;
    public GameObject resultPanel;
    public Text resultText;  // 現在這裡的Text類型應該能被識別了
    
    public void DealDamageToPlayer(int damage)
    {
        playerHealth -= damage;

        // 檢查玩家是否被擊倒
        if (playerHealth <= 0)
        {
            // 停止遊戲並顯示敵人獲勝的訊息
            Debug.Log("Enemy wins!");
            Time.timeScale = 0;

            resultText.text = "Enemy wins!";
            resultPanel.SetActive(true);
        }
    }

    public void DealDamageToEnemy(int damage)
    {
        enemyHealth -= damage;

        // 檢查敵人是否被擊倒
        if (enemyHealth <= 0)
        {
            // 停止遊戲並顯示玩家獲勝的訊息
            Debug.Log("Player wins!");
            Time.timeScale = 0;

            resultText.text = "Player wins!";
            resultPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        // 啟動遊戲，允許角色的行動值增加
        Time.timeScale = 1;

        // 重置玩家和敵人的健康值
        playerHealth = 100;
        enemyHealth = 100;

        // 重置玩家和敵人的行動值
        playerController.ResetActionValue();
        enemyController.ResetActionValue();
    }
}
