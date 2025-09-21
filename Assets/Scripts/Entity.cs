using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    protected private float oldHP;      // 前フレームのHP
    protected private float received;   // 被ダメージ
    public Slider HPBar;                // HPバーのインスタンス
    private TextMeshProUGUI damageText; // ダメージ表記のテキスト
    private Coroutine coroutine;        // コールチン格納用

    [Header("基礎パラメータ")]
    public float MaxHP;         // 最大体力
    public float HP;            // 現在の体力
    public float ATK;           // 攻撃力


    protected virtual void Awake()
    {
        // HPゲージの生成
        GameObject HPcanvas = Resources.Load<GameObject>("HP Canvas");
        GameObject canvas1 = Instantiate(HPcanvas, transform.position, Quaternion.identity);
        canvas1.transform.SetParent(transform);
        GameObject slider1 = canvas1.transform.Find("HP").gameObject;
        HPBar = slider1.GetComponent<Slider>();
        HPBar.maxValue = MaxHP;
        HPBar.value = HP;
        // ダメージ表記テキストの取得
        GameObject text = canvas1.transform.Find("Damage").gameObject;
        damageText = text.GetComponent<TextMeshProUGUI>();
        damageText.enabled = false;

        // 初期値の設定
        HP = MaxHP;
        oldHP = HP;
    }


    protected virtual void Update()
    {
        // HPの更新
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
        HPBar.maxValue = MaxHP;
        HPBar.value = HP;

        // HPの変化を確認
        received = oldHP - HP;
    }


    protected void HPChange(float received)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // ダメージを受けた時
        if (received > 0)
        {
            // 前のコールチンを停止
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            // 新たにコールチンを開始
            coroutine = StartCoroutine(Damage(sprite));

        }
        // 回復した時
        else if (received < 0)
        {
            // 前のコールチンを停止
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            // 新たにコールチンを開始
            coroutine = StartCoroutine(Heal(sprite));
        }
    }


    // ダメージエフェクト
    IEnumerator Damage(SpriteRenderer sprite)
    {
        // 色を赤色に変更
        sprite.color = Color.red;
        // ダメージを表記
        damageText.enabled = true;
        damageText.text = received.ToString("f0");
        damageText.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        // 元に戻す
        damageText.enabled = false;
        sprite.color = Color.white;
    }


    // 回復エフェクト
    IEnumerator Heal(SpriteRenderer sprite)
    {
        // 色を緑色に変更
        sprite.color = Color.green;
        // ダメージを表記
        damageText.enabled = true;
        received = -received;
        damageText.text = received.ToString("f0");
        damageText.color = Color.green;

        yield return new WaitForSeconds(0.5f);

        // 元に戻す
        damageText.enabled = false;
        sprite.color = Color.white;
    }
}
