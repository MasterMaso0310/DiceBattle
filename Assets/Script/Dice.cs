using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private int diceValue;

    public void SetValue(int value)
    {
        diceValue = value;
    }

    public int GetValue()
    {
        return diceValue;
    }
}
