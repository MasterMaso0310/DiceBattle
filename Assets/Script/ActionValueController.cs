using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionValueController : MonoBehaviour
{
    public Slider actionSlider;
    public float agility;
    public DiceController diceController; // 添加對DiceController的引用

    private float actionValue = 0f;

    private void Update()
    {
        if (actionValue < 100f)
        {
            actionValue += Time.deltaTime * (100f / agility);
            actionSlider.value = actionValue;
        }
        else
        {
            // 到達100，進行行動
            TakeAction();
        }
    }

    public float GetActionValue()
    {
        return actionValue;
    }

    public void ResetActionValue()
    {
        actionValue = 0f;
    }

    private void TakeAction()
    {
        // 決定行動
        int[] diceValues = new int[diceController.dices.Length];
        for (int i = 0; i < diceController.dices.Length; i++)
        {
            diceValues[i] = diceController.dices[i].GetFaceValue();
        }

        // 呼叫CharacterActionController的PerformAction方法
        GetComponent<CharacterActionController>().PerformAction(diceValues);

        // 行動結束後，重置行動值
        actionValue = 0f;
    }
}