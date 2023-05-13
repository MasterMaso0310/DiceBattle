using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public ActionValueController actionValueController;
    public Dice[] dices; // 建立一個骰子陣列
    
    private int faceValue;

    private void Update()
    {
        if (actionValueController.GetActionValue() < 80f)
        {
            // 隨機變換骰面
            faceValue = Random.Range(1, 7);
        }
    }

    public int GetFaceValue()
    {
        return faceValue;
    }
}
