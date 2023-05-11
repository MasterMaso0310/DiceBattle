using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int diceNumber; // 骰子數量

    [SerializeField]
    private float agilitySpeed; // 敏捷速度

    public int DiceNumber
    {
        get { return diceNumber; }
        private set { diceNumber = Mathf.Clamp(value, 1, 12); }
    }

    public float AgilitySpeed
    {
        get { return agilitySpeed; }
        private set { agilitySpeed = value; }
    }

    // 在這裡可以添加其他屬性和方法
}
