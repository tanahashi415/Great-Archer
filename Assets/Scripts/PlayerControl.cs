using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// プレイヤー操作に関するスクリプト
public class PlayerControl : Entity
{
    private float chargeTime;   // 溜め時間
    private int chargeLevel;    // 溜め段階
    private Vector3 pos;        // キャラクターの座標
    private bool canCharge;     // 溜め可能か
    private Slider ChargeBar;   // 溜めゲージのインスタンス
    private Image image;        // 溜めゲージのImageコンポーネント

    public GameObject arrow;    // 生成する矢
    public float chargeSpeed;   // 溜めスピード
    public float coolTime;      // クールタイム
    public int penetration;     // 貫通数
    public float arrowSpeed;    // 矢の速さ
    public float fixedDamage;   // 矢の固定ダメージ

    public GameObject chargeMaxEffect;  // 溜め完了エフェクト
    public GameObject chargeEffect;     // 溜めエフェクト

    [Header("設定しなくていもの")]
    public static bool isStore; // 買い物中かどうか

    protected override void Awake()
    {
        // 溜めゲージの生成
        GameObject Chargecanvas = Resources.Load<GameObject>("Charge Canvas");
        GameObject canvas2 = Instantiate(Chargecanvas, transform.position, Quaternion.identity);
        canvas2.transform.SetParent(transform);
        GameObject slider2 = canvas2.transform.Find("Charge").gameObject;
        ChargeBar = slider2.GetComponent<Slider>();
        // 溜めゲージのImageコンポーネント取得
        GameObject fill = ChargeBar.transform.Find("Fill Area/Fill").gameObject;
        image = fill.GetComponent<Image>();

        base.Awake();

        // 初期値の設定
        pos = transform.position;
        ChargeBar.value = 0.0f;
        canCharge = true;
        isStore = false;
    }


    protected override void Update()
    {
        base.Update();

        if (isStore)
        {
            StartCoroutine(CoolDown());
        }
        else
        {
            if (canCharge)
            {
                // 溜めの進捗表示
                ChargeBar.value = chargeSpeed * chargeTime;
                // 溜め段階
                if (ChargeBar.value < ChargeBar.maxValue)
                {
                    chargeLevel = 0;
                }
                else if (ChargeBar.value >= ChargeBar.maxValue)
                {
                    chargeLevel = 1;
                    chargeMaxEffect.SetActive(true);
                }

                // 左ボタン長押しで弓を引く
                if (Input.GetMouseButtonDown(0))
                {
                    chargeTime = 0.0f;
                }
                else if (Input.GetMouseButton(0))
                {
                    chargeTime += Time.deltaTime;
                    chargeEffect.SetActive(true);
                }
                // 左ボタンを離して矢を射る
                else if (Input.GetMouseButtonUp(0))
                {
                    // マウスの位置を取得し、ワールド座標に変換
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + 10.0f * Vector3.forward;
                    // 射出角度の計算
                    Vector3 direction = mousePos - pos;
                    float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
                    // 矢を生成
                    GameObject obj = Instantiate(arrow, pos, Quaternion.Euler(angle * Vector3.forward));
                    // 矢のパラメータを変更
                    Arrow script = obj.GetComponent<Arrow>();
                    script.ATK = ATK;
                    script.penetration = penetration + chargeLevel;
                    script.fixedDamage = fixedDamage;
                    // 矢を射出
                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                    rb.AddForce((arrowSpeed + chargeLevel) * direction.normalized, ForceMode2D.Impulse);
                    // SEの再生
                    SoundManager.instance.PlaySE(SoundManager.instance.shotSE, 1.0f);
                    // クールダウンの開始
                    StartCoroutine(CoolDown());
                }
            }
        }

        // HPが変化したとき
        if (received != 0)
        {
            // HPが0になったら破壊
            if (HP <= 0)
            {
                canCharge = false;
                chargeMaxEffect.SetActive(false);
                chargeEffect.SetActive(false);
                chargeTime = 0.0f;
                GameOverManager.GameOver();
                Destroy(gameObject);
            }
            else
            {
                HPChange(received);
            }
        }

        // oldHPを更新
        oldHP = HP;
    }


    // クールダウン
    IEnumerator CoolDown()
    {
        canCharge = false;
        chargeMaxEffect.SetActive(false);
        chargeEffect.SetActive(false);
        chargeTime = 0.0f;

        // ゲージを青色に変更
        image.color = Color.blue;

        // 溜め時間リセット
        float rate = ChargeBar.value / coolTime;
        while (ChargeBar.value > 0.0f)
        {
            ChargeBar.value -= rate * Time.deltaTime;
            yield return null;
        }

        image.color = Color.yellow;
        canCharge = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Coin")
        {
            CoinManager.coin++;
        }
        else if (tag == "ThrowingObj")
        {
            SoundManager.instance.PlaySE(SoundManager.instance.damageSE, 1.0f);
        }
    }
}
