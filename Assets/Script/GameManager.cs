using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 加上這一行
using TMPro;

public class GameManager : MonoBehaviour
{
    public ActionValueController playerController;
    public ActionValueController enemyController;
    // 這是唯一的GameManager實例
    public static GameManager instance;
    public bool isActionTaken = false;
    // 健康值可以根據你的遊戲設計進行設定
    public int playerHealth = 100;
    public int enemyHealth = 100;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;  // 現在這裡的Text類型應該能被識別了
    public void DealDamageToPlayer(int damage)
    {
        Debug.Log("enemy is attack to player");
        playerHealth -= damage;

        // 更新玩家生命值的顯示
        playerHealthText.text = "Player Health: " + playerHealth;

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
        Debug.Log("player is attack to enemy");
        enemyHealth -= damage;

        // 更新敵人生命值的顯示
        enemyHealthText.text = "Enemy Health: " + enemyHealth;

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
        Debug.Log("Game started. Player Health: " + playerHealth + ", Enemy Health: " + enemyHealth);  // 這行將會在Console視窗中輸出

        // 更新玩家和敵人的血量文字
        playerHealthText.text = "Player Health: " + playerHealth;
        enemyHealthText.text = "Enemy Health: " + enemyHealth;
    }

    private void Awake()
    {
        // 如果instance已經被設定，並且不是這個實例，那麼銷毀這個物件
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // 否則，這個物件就是GameManager的唯一實例
            instance = this;
        }
    }

    public void ActionTaken()
    {
        isActionTaken = true;
    }
}
