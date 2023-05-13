// GameManager.cs 程式碼
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager_old : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private Transform diceSet;

    [SerializeField]
    private GameObject playerProgressBarObject;

    [SerializeField]
    private GameObject enemyProgressBarObject;

    private Slider playerProgressBar;
    private Slider enemyProgressBar;

    public float rollDuration = 2f;  // 骰子滾動的時間（秒）

    public Sprite[] diceFaces;  // 骰子各面的圖片

    private bool isPlayerTurn = true;
    private bool rollingDice = false;

    private bool isPlayerProgressBarEmpty = false;
    private bool isEnemyProgressBarEmpty = false;
    private bool isPause = false;

    private float playerProgressTimer; // 声明玩家进度计时器变量
    private float playerProgressDuration; // 声明玩家进度持续时间变量

    private float enemyProgressTimer; // 声明敌人进度计时器变量
    private float enemyProgressDuration; // 声明敌人进度持续时间变量

    private void Start()
    {
        playerProgressBar = playerProgressBarObject.GetComponent<Slider>();
        enemyProgressBar = enemyProgressBarObject.GetComponent<Slider>();

        playerProgressBar.value = 1f;  // 初始化玩家進度條為100%
        enemyProgressBar.value = 1f;   // 初始化敵人進度條為100%

        // 初始化骰子各面的圖片
        diceFaces = new Sprite[6];
        for (int i = 0; i < 6; i++)
        {
            diceFaces[i] = Resources.Load<Sprite>("DiceFaces/Dice" + (i + 1));
        }
    }

    private void Update()
    {
        // 按下鍵盤空白鍵才開始跑條
        if (Input.GetKeyDown(KeyCode.Space) && !rollingDice)
        {
            if (isPlayerTurn)
            {
                StartRollDice();
            }
        }
    }

    private void StartRollDice()
    {
        StartCoroutine(RollDiceCoroutine());

        // 檢查是否需要暫停進度條倒扣
        if (playerProgressBar.value <= 0f || enemyProgressBar.value <= 0f)
        {
            isPause = true;
        }
        else
        {
            // 玩家进度条不为0时，重置敌人进度条为当前值
            isPause = false;
            enemyProgressTimer = (1f - enemyProgressBar.value) * enemyProgressDuration;
        }

        StartCoroutine(EnemyProgressBarCoroutine());
    }

    private IEnumerator EnemyProgressBarCoroutine()
    {
        // 計算敵人進度條的移動時間
        enemyProgressDuration = CalculateProgressDuration(enemy.AgilitySpeed);

        // 移動敵人進度條
        while (enemyProgressTimer < enemyProgressDuration)
        {
            if (!isPause && playerProgressBar.value > 0f)  // 如果沒有暫停且玩家進度條大於0，才移動進度條
        {
        enemyProgressBar.value = Mathf.Lerp(1f, 0f, enemyProgressTimer / enemyProgressDuration);
        enemyProgressTimer += Time.deltaTime;
        }
        yield return null;
        }
        // 停止敵人進度條變動
        enemyProgressBar.value = 0f;
        // 根據玩家和敵人的進度條情況切換回合
        if (isPlayerProgressBarEmpty)
        {
            isPlayerProgressBarEmpty = false;
            StartCoroutine(PlayerTurnCoroutine());
        }
        else if (isEnemyProgressBarEmpty)
        {
            isEnemyProgressBarEmpty = false;
            StartCoroutine(EnemyTurnCoroutine());
        }
    }

    private IEnumerator RollDiceCoroutine()
    {
        rollingDice = true;

        // 顯示玩家和敵人的進度條
        playerProgressBar.value = 1f;
        enemyProgressBar.value = 1f;

        // 計算玩家和敵人進度條的移動時間
        playerProgressDuration = CalculateProgressDuration(player.AgilitySpeed);
        enemyProgressDuration = CalculateProgressDuration(enemy.AgilitySpeed);

        // 移動玩家和敵人的進度條
        playerProgressTimer = 0f;
        enemyProgressTimer = 0f;
        while (playerProgressTimer < playerProgressDuration || enemyProgressTimer < enemyProgressDuration)
        {
            if (!isPause)
            {
                if (playerProgressTimer < playerProgressDuration)
                {
                    playerProgressBar.value = Mathf.Lerp(1f, 0f, playerProgressTimer / playerProgressDuration);
                    playerProgressTimer += Time.deltaTime;

                    // 檢查玩家進度條是否為空
                    if (playerProgressBar.value <= 0f)
                    {
                        isPlayerProgressBarEmpty = true;
                    }
                }

                if (enemyProgressTimer < enemyProgressDuration)
                {
                    enemyProgressBar.value = Mathf.Lerp(1f, 0f, enemyProgressTimer / enemyProgressDuration);
                    enemyProgressTimer += Time.deltaTime;

                    // 檢查敵人進度條是否為空
                    if (enemyProgressBar.value <= 0f)
                    {
                        isEnemyProgressBarEmpty = true;
                    }
                }
            }
            yield return null;
        }

        // 停止玩家和敵人的進度條變動
        playerProgressBar.value = 0f;
        enemyProgressBar.value = 0f;

        // 獲取玩家的 DiceNumber
        int diceNumber = player.DiceNumber;

        // 生成骰子
        GenerateDice(diceNumber);

        // 滾動骰子
        foreach (Transform dice in diceSet)
        {
            StartCoroutine(RollSingleDice(dice));
        }

        // 等待骰子滾動結束
        yield return new WaitForSeconds(rollDuration);

        int totalDiceValue = 0;
        // 讀取骰子點數並執行相應的動作
        foreach (Transform dice in diceSet)
        {
            int diceValue = Random.Range(1, 7); // 產生1到6的隨機數字
            totalDiceValue += diceValue;
            Debug.Log("Dice Value: " + diceValue);
            PerformAction(diceValue);
        }

        Debug.Log("Total Dice Value: " + totalDiceValue);

        // 根據玩家和敵人的進度條情況切換回合
        if (isPlayerProgressBarEmpty)
        {
            isPlayerProgressBarEmpty = false;
            StartCoroutine(PlayerTurnCoroutine());
        }
        else if (isEnemyProgressBarEmpty)
        {
            isEnemyProgressBarEmpty = false;
            StartCoroutine(EnemyTurnCoroutine());
        }

        rollingDice = false;
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        // 顯示敵人進度條
        enemyProgressBar.value = 1f;

        // 計算敵人進度條的移動時間
        enemyProgressDuration = CalculateProgressDuration(enemy.AgilitySpeed);

        // 移動敵人進度條
        enemyProgressTimer = 0f;
        while (enemyProgressTimer < enemyProgressDuration)
        {
            enemyProgressBar.value = Mathf.Lerp(1f, 0f, enemyProgressTimer / enemyProgressDuration);
            enemyProgressTimer += Time.deltaTime;
            yield return null;
        }

        // 停止敵人進度條變動
        enemyProgressBar.value = 0f;

        // 敵人執行行動
        enemy.PerformAction();

        // 切換回玩家回合
        isPlayerTurn = true;
        StartCoroutine(PlayerTurnCoroutine());
    }

    private IEnumerator PlayerTurnCoroutine()
    {
        // 顯示玩家進度條
        playerProgressBar.value = 1f;

        // 計算玩家進度條的移動時間
        playerProgressDuration = CalculateProgressDuration(player.AgilitySpeed);

        // 移動玩家進度條
        playerProgressTimer = 0f;
        while (playerProgressTimer < playerProgressDuration)
        {
            playerProgressBar.value = Mathf.Lerp(1f, 0f, playerProgressTimer / playerProgressDuration);
            playerProgressTimer += Time.deltaTime;
            yield return null;
        }

        // 停止玩家進度條變動
        playerProgressBar.value = 0f;
    }

    private float CalculateProgressDuration(float agilitySpeed)
    {
        // 按敏捷速度計算進度條的移動時間
        // 每1% 需要花費 log(i) 秒的時間，i 為敏捷速度的數值，如果這麼算，則敏捷越高進度條跑越慢
        // 如果想要敏捷越高花費時間越短，則應該敏捷數值要與progressDuration成反比
        float progressDuration = (50 / agilitySpeed);
        return progressDuration;
    }

    private void GenerateDice(int diceNumber)
    {
        // 限制 diceNumber 的範圍在 1 到 12 之間
        diceNumber = Mathf.Clamp(diceNumber, 1, 12);

        // 移除原有的骰子
        foreach (Transform dice in diceSet)
        {
            Destroy(dice.gameObject);
        }

        // 計算生成骰子的起始位置
        float startX = -8f;
        float startY = 4f;
        int rowLimit = 6;
        int currentColumn = 0;
        int currentRow = 0;

        // 計算需要暫停的進度條數量
        int pauseCount = Mathf.CeilToInt(diceNumber / 2f);

        // 生成新的骰子
        for (int i = 0; i < diceNumber; i++)
        {
            GameObject diceObject = new GameObject("Dice");
            diceObject.transform.parent = diceSet;

            // 計算骰子的位置
            float posX = startX + (currentColumn * 1.5f);
            float posY = startY - (currentRow * 1.5f);
            diceObject.transform.position = new Vector3(posX, posY, 0f);

            diceObject.AddComponent<SpriteRenderer>();
            diceObject.AddComponent<Dice>();

            currentRow++;
            if (currentRow >= rowLimit)
            {
                currentRow = 0;
                currentColumn++;
            }

            // 如果是需要暫停的進度條，則將進度條設置為當前值
            if (i < pauseCount)
            {
                float currentProgress = Mathf.Lerp(1f, 0f, playerProgressTimer / playerProgressDuration);
                playerProgressBar.value = currentProgress;
                enemyProgressBar.value = currentProgress;
            }
        }
    }

    private IEnumerator RollSingleDice(Transform dice)
    {
        // 停留時間
        float delay = 1f;

        // 開始滾動骰子
        int randomValue = Random.Range(0, 6);
        SpriteRenderer spriteRenderer = dice.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = diceFaces[randomValue];
        }

        // 隨機停留的骰面
        int targetValue = Random.Range(0, 6);

        float timer = 0f;
        while (timer < delay)
        {
            // ...

            if (spriteRenderer != null)
            {
                int newValue = Random.Range(0, 6);
                if (newValue != randomValue)
                {
                    randomValue = newValue;
                    spriteRenderer.sprite = diceFaces[randomValue];
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 停止骰子的旋轉
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = diceFaces[targetValue];
        }
    }

    private int GetDiceValue(Transform dice)
    {
        // 根據骰子的骰面圖片或其他標識來決定骰子的點數
        // 這裡只是一個示例，你可以根據你的需求來實現這個函數
        int value = 0;
        // ...
        return value;
    }

    private void PerformAction(int diceValue)
    {
        // 根據骰子的點數執行相應的動作
        // 這裡只是一個示例，你可以根據你的需求來實現這個函數
        Debug.Log("Dice value: " + diceValue);
        // ...
    }
}