using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionController : MonoBehaviour
{
    public GameManager gameManager;

    public void PerformAction(int[] diceValues)
    {
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
}