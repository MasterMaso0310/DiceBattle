using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float AgilitySpeed;  // 敵人的敏捷速度

    // 執行敵人的動作
    public void PerformAction()
    {
        // 在這裡實現敵人的動作邏輯
        Debug.Log("Enemy performing action");
    }
}
