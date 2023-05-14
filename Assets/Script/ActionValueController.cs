using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionValueController : MonoBehaviour
{
    public Slider actionSlider;
    public CharacterStats characterStats; // 添加對CharacterStats的引用
    public DiceController diceController; // 添加對DiceController的引用
    public CharacterActionController characterActionController; // 新增characterActionController的組件
    public TextMeshProUGUI actionValueText; // 新增對應的行動值顯示的文字UI參考
    private float actionValue = 0f;

    // 在遊戲開始時設定滑塊的最大值
    private void Start()
    {
        actionSlider.maxValue = 100f;
        if (characterActionController == null)
        {
            Debug.LogError("CharacterActionController not found on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (actionValue < 100f)
        {
            actionValue += Time.deltaTime * (100f / characterStats.agility);
            actionSlider.value = actionValue;
            actionValueText.text = actionValue.ToString("0.00"); // 範圍已經是0-100
        }
        else
        {
            // 到達100，進行行動
            TakeAction();
            GameManager.instance.ActionTaken();
        }
    }

    public float GetActionValue()
    {
        return actionValue;
    }

    public void ResetActionValue()
    {
        actionValue = 0f;
        // 在行動結束時，設定 isActionTaken 為 false
        GameManager.instance.isActionTaken = false;
    }

    private void TakeAction()
    {
        // 決定行動
        int[] diceValues = new int[diceController.dices.Count];
        for (int i = 0; i < diceController.dices.Count; i++)
        {
            diceValues[i] = diceController.dices[i].GetFaceValue();
        }
        Debug.Log(diceValues);
        // 使用存儲的引用來調用方法
        characterActionController.PerformAction(diceValues);

        // 行動結束後，重置行動值
        actionValue = 0f;
    }

    public void ToggleActionValueTextVisibility()
    {   //要不要讓行動值以文字內容被顯示的開關
        bool visibility = actionValueText.enabled;
        actionValueText.enabled = !visibility;
    }
}