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

    private bool dicesCreated = false;
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
        if (actionValueController.GetActionValue() >= 80f)
        {
            SetDiceValues();
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

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetDiceValues()
    {
        for (int i = 0; i < characterStats.diceCount; i++)
        {
            dices[i].SetFaceValue(Random.Range(1, 7));
        }
    }

    public int GetFaceValue()
    {
        Debug.Log(faceValue);
        return faceValue;
    }
}
