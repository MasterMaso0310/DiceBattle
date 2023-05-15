using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    // 骰子的面值
    private int faceValue;
    private Sprite[] diceFaces; // 儲存骰子面值的精靈
    private SpriteRenderer sr; // 骰子的精靈渲染器
    private bool isRolling = false; // 骰子是否正在滾動
    private IEnumerator rollingAnimation; // 用來儲存滾動骰子的協程
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        diceFaces = new Sprite[6];
        for (int i = 0; i < 6; i++)
        {
            diceFaces[i] = Resources.Load<Sprite>("DiceFaces/Dice" + (i + 1));
        }
    }

    // 設定骰子的面值
    public void SetFaceValue(int value)
    {
        isRolling = false;
        faceValue = value;
        sr.sprite = diceFaces[value - 1];
    }

    public void StartRolling()
    {
        isRolling = true;
        StartCoroutine(RollingAnimation());
    }

    public void StopRolling()
    {
        isRolling = false;
        if (rollingAnimation != null) // 確定rollingAnimation非null才停止
        {
            StopCoroutine(rollingAnimation);
        }
    }

    private IEnumerator RollingAnimation()
    {
        while (isRolling)
        {
            sr.sprite = diceFaces[Random.Range(0, 6)];
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public int GetFaceValue()
    {
        return faceValue;
    }
}
