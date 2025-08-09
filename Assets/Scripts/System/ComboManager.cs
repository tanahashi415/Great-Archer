using System.Collections;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private static bool isCombo;    // コンボしているか
    public static int combo;        // コンボ数

    private Coroutine coroutine;            // コールチン格納用
    private TextMeshProUGUI comboText;      // テキスト
    private RectTransform rectTransform;    // 位置
    private Vector2 initialPos;             // 初期位置

    public GameObject text; // テキストオブジェクトのインスタンス
    public static float timeLimit; // コンボ受付時間

    void Start()
    {
        comboText = text.GetComponent<TextMeshProUGUI>();
        rectTransform = text.GetComponent<RectTransform>();
        initialPos = rectTransform.position;
        comboText.enabled = false;
        isCombo = false;
        combo = 0;
        timeLimit = 1.0f;
    }

    void Update()
    {
        if (isCombo)
        {
            // 前のコールチンを停止
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                rectTransform.position = initialPos;
            }
            // 新たにコールチンを開始
            coroutine = StartCoroutine(DisplayCombo());
            isCombo = false;
        }
        
    }

    public static void Combo()
    {
        combo++;
        isCombo = true;
    }

    IEnumerator DisplayCombo()
    {
        // コンボ表示
        comboText.enabled = true;
        comboText.text = combo.ToString() + " Combo!!";

        // 振動させる
        float time = 0.025f;
        WaitForSeconds wait = new WaitForSeconds(time);
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y + 15.0f);
        yield return wait;
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - 30.0f);
        yield return wait;
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y + 25.0f);
        yield return wait;
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y - 15.0f);
        yield return wait;
        rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y + 5.0f);
        if (timeLimit > 0.1f)
        {
            yield return new WaitForSeconds(timeLimit - 0.1f);
        }

        // コンボ非表示
        comboText.enabled = false;
        combo = 0;
    }
}
