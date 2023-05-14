using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionController : MonoBehaviour
{
    public GameManager gameManager;  // 在這裡添加gameManager變數

    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void PerformAction(int[] diceValues)
    {
        Debug.Log(gameObject.name + " is taking action.");
        // 計算基於骰子值的攻擊力、防禦力，或其他行動
        int attackPower = 0;
        foreach (int value in diceValues)
        {
            attackPower += value;
        }

        // 如果這個角色是玩家，對敵人造成傷害
        if (gameObject.tag == "Player")
        {
            gameManager.DealDamageToEnemy(attackPower);
        }
        // 如果這個角色是敵人，對玩家造成傷害
        else if (gameObject.tag == "Enemy")
        {
            gameManager.DealDamageToPlayer(attackPower);
        }
    }

    // 添加 OnDestroy 方法
    void OnDestroy()
    {
        Debug.Log("CharacterActionController on " + gameObject.name + " was destroyed.", this);
    }
}
