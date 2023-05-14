using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int agility;
    public int health;
    public int diceCount;
    public int[] diceValues; //新增一個陣列來保存每個骰子的面值

    private void Start()
    {
        diceValues = new int[diceCount];  //在遊戲開始時初始化陣列大小
    }
    // 這裡可以添加更多的屬性和方法，例如修改屬性，或者處理特殊的遊戲事件等
}