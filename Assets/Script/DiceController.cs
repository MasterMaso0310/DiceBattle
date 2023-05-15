using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public ActionValueController actionValueController;
    public CharacterStats characterStats;
    public Dice dicePrefab; 
    public List<Dice> dices;
    public int faceValue;

    private bool dicesCreated = false; // 這個新的布林值可以用來追蹤骰子是否已經全部生成
    private bool isRolling = false;
    private Vector3 spawnPos;
    private int columnCounter = 0;
    private int rowCounter = 0;
    private float diceHeight;

    public void Start()
    {
        dices = new List<Dice>();
        diceHeight = dicePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        spawnPos = (actionValueController.gameObject.CompareTag("Player")) ? new Vector3(-8, -2, 0) : new Vector3(8, -2, 0); // 根據角色標籤設定初始位置
    }

    public void Update()
    {
        if (!dicesCreated)
        {
            StartCoroutine(CreateDices());
            dicesCreated = true;
        }
        float actionValue = actionValueController.GetActionValue();
        if (actionValue >= 10f && actionValue < 80f)
        {
            StartRolling();
            isRolling = true;
        }
        else if ((actionValue < 10f || actionValue >= 80f) && isRolling)
        {
            StopRolling();
            isRolling = false;
        }
    }

    private IEnumerator CreateDices()
    {
        for (int i = 0; i < characterStats.diceCount; i++)
        {
            Dice newDice = Instantiate(dicePrefab, spawnPos, Quaternion.identity);
            newDice.transform.parent = this.transform;
            dices.Add(newDice);
            
            rowCounter++;
            if(rowCounter >= 6) 
            {
                rowCounter = 0;
                columnCounter++;
            }

            spawnPos.y += diceHeight; // 更新 Y 座標
            if (rowCounter == 0) // 換列
            {
                spawnPos.y = -5;
                spawnPos.x += (actionValueController.gameObject.CompareTag("Player")) ? diceHeight : -diceHeight; // 更新 X 座標
            }

            yield return new WaitForSeconds(0.1f);
        }
        dicesCreated = true;
    }

    public void StartRolling()
    {
        foreach (var dice in dices)
        {
            dice.StartRolling();
        }
    }

    public void StopRolling()
    {
        foreach (var dice in dices)
        {
            dice.StopRolling();
            dice.SetFaceValue(Random.Range(1, 7));
        }
    }

    public int GetFaceValue()
    {
        Debug.Log(faceValue);
        return faceValue;
    }
}
