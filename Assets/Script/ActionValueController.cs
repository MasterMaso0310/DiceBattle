using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionValueController : MonoBehaviour
{
    public Slider actionSlider;
    public Button takeActionButton;  // Add reference to your "TakeAction" button
    public CharacterStats characterStats; // 添加對CharacterStats的引用
    public DiceController diceController; // 添加對DiceController的引用
    public CharacterActionController characterActionController; // 新增characterActionController的組件
    public TextMeshProUGUI actionValueText; // 新增對應的行動值顯示的文字UI參考
    private float actionValue = 0f;

    // 在遊戲開始時設定滑塊的最大值
    private void Start()
    {
        // Disable the button at the start
        takeActionButton.interactable = false;
        actionSlider.maxValue = 100f;
        if (characterActionController == null)
        {
            Debug.LogError("CharacterActionController not found on " + gameObject.name);
        }
    }

    private void Update()
    {
        //當actionValue小於100，則行動值持續增加，這邊可以直接改成，如果行動值小於100，並且當有人行動值到達100，且還未結算行動之前，就先停止增加
        if (actionValue < 100f  && GameManager.instance.canIncreaseActionValue) // 檢查新的布林變量
        {
            takeActionButton.interactable = false;
            actionValue += Time.deltaTime * (characterStats.agility);
            actionSlider.value = actionValue;
            actionValueText.text = actionValue.ToString("0.00"); // 範圍已經是0-100
        }
        //Update()函數會一直不斷被執行，而當
        else if(actionValue >= 100f)
        {
            GameManager.instance.canIncreaseActionValue = false; // 在這裡停止增加行動值
            // 如果ActionValueController是附掛在Player物件上，則開啟按鈕可按的程序
            // 到達100，開啟行動按鈕
            if (gameObject.tag == "Player")
            {
                // 將按鈕啟動
                takeActionButton.interactable = true;
                // 將正在行動開啟
                GameManager.instance.isActionTaken = true;
            }
            else if(gameObject.tag == "Enemy")
            {
                EnemyAction();
            }
        }
    }

    public float GetActionValue()
    {
        return actionValue;
    }

    // 決定行動，在這邊先以簡單的方式來設計，以按下特定按鈕之後進行攻擊來作為初始化範例
    public void TakeAction()
    {
        Debug.Log("玩家執行行動");
        //這邊是獲得骰子的數量
        int[] diceValues = new int[diceController.dices.Count];
        //以迴圈方式每個骰子都獲得最終骰面的數值
        for (int i = 0; i < diceController.dices.Count; i++)
        {
            Debug.Log("第" + i + "顆骰子的點數: " + diceController.dices[i].GetFaceValue());
            //從dices物件當中的GetFaceValue()功能獲得骰面數值
            diceValues[i] = diceController.dices[i].GetFaceValue();
        }
        //打印出最終的骰面組合
        Debug.Log(diceValues);

        // 使用存儲的引用來調用方法
        characterActionController.PerformAction(diceValues);
        // 在這邊需要新增一個按下TakeAction按鈕的動作
        // 行動結束後，重置行動值
        actionValue = 0f;
        // 在行動結束時，設定 isActionTaken 為 false
        GameManager.instance.isActionTaken = false;
        GameManager.instance.canIncreaseActionValue = true; // 在這裡重新啟動增加行動值
    }

    // 決定行動，在這邊先以簡單的方式來設計，以按下特定按鈕之後進行攻擊來作為初始化範例
    public void EnemyAction()
    {
        Debug.Log("敵人執行行動");
        //這邊是獲得骰子的數量
        int[] diceValues = new int[diceController.dices.Count];
        //以迴圈方式每個骰子都獲得最終骰面的數值
        for (int i = 0; i < diceController.dices.Count; i++)
        {
            //從dices物件當中的GetFaceValue()功能獲得骰面數值
            diceValues[i] = diceController.dices[i].GetFaceValue();
        }
        //打印出最終的骰面組合
        Debug.Log(diceValues);

        // 使用存儲的引用來調用方法
        characterActionController.PerformAction(diceValues);
        // 在這邊需要新增一個按下TakeAction按鈕的動作
        // 行動結束後，重置行動值
        actionValue = 0f;
        // 在行動結束時，設定 isActionTaken 為 false
        GameManager.instance.isActionTaken = false;
        GameManager.instance.canIncreaseActionValue = true; // 在這裡重新啟動增加行動值
    }

    public void ToggleActionValueTextVisibility()
    {   //要不要讓行動值以文字內容被顯示的開關
        bool visibility = actionValueText.enabled;
        actionValueText.enabled = !visibility;
    }
}