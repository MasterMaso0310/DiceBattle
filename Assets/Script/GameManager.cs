using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private Transform diceSet;

    [SerializeField]
    private GameObject progressBar;

    public float rollDuration = 2f;  // 骰子滾動的時間（秒）

    public Sprite[] diceFaces;  // 骰子各面的圖片

    private Slider progressSlider;
    private bool rollingDice = false;

    private void Start()
    {
        progressSlider = progressBar.GetComponent<Slider>();
        progressSlider.value = 1f;  // 初始化進度條為100%

        // 初始化骰子各面的圖片
        diceFaces = new Sprite[6];
        for (int i = 0; i < 6; i++)
        {
            diceFaces[i] = Resources.Load<Sprite>("DiceFaces/Dice" + (i + 1));
        }
    }

    private void Update()
    {
        // 遊戲開始後一直跑條
        // StartRollDice();
        // 按下鍵盤空白鍵才開始跑條
        if (Input.GetKeyDown(KeyCode.Space) && !rollingDice)
        {
            StartRollDice();
        }
    }

    private void StartRollDice()
    {
        StartCoroutine(RollDiceCoroutine());
    }

    private IEnumerator RollDiceCoroutine()
    {
        rollingDice = true;

        // 顯示進度條
        progressSlider.value = 1f;

        // 計算進度條的移動時間
        float progressDuration = CalculateProgressDuration(player.AgilitySpeed);

        // 移動進度條
        float progressTimer = 0f;
        while (progressTimer < progressDuration)
        {
            progressSlider.value = Mathf.Lerp(1f, 0f, progressTimer / progressDuration);
            progressTimer += Time.deltaTime;
            yield return null;
        }

        // 停止進度條變動
        progressSlider.value = 0f;

        // 獲取 Player 的 DiceNumber
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
        rollingDice = false;
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