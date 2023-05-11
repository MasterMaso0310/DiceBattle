using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int diceNumber; // 骰子數量

    public int DiceNumber
    {
        get { return diceNumber; }
        set { diceNumber = value; }
    }

    // 設定骰子數量的方法
    public void SetDiceNumber(int number)
    {
        DiceNumber = number;
    }
}
