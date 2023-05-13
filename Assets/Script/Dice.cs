using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private int faceValue;

    // 你可能還需要一個方法來設定faceValue的值，例如：
    public void RollDice()
    {
        faceValue = Random.Range(1, 7);
    }

    public int GetFaceValue()
    {
        return faceValue;
    }
}
