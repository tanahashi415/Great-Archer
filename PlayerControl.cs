using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// プレイヤー操作に関するスクリプト
public class PlayerControl : MonoBehaviour
{
    private float oldHP;        // 前フレームのHP
    private float received;     // 被ダメージ
    private float chargeTime;   // 溜め時間
    private int chargeLevel;    // 溜め段階
    private Vector3 pos;        // キャラクターの座標
    private bool canCharge;     // 溜め可能か
    private Slider HPBar;       // HPバーのインスタンス

    public Slider ChargeBar;            // 溜めゲージのインスタンス
    public GameObject chargeMaxEffect;  // 溜め完了エフェクト
    public GameObject chargeEffect;     // 溜めエフェクト

    [Header("基礎パラメータ")]
    public GameObject arrow;    // 生成する矢
    public float chargeSpeed;   // 溜めスピード
    public float coolTime;      // クールタイム
    public int penetration;     // 貫通数
    public float arrowSpeed;    // 矢の速さ
    public float fixedDamage;   // 矢の固定ダメージ

    public float HP;            // 体力
    public float ATK;           // 攻撃力


    void Start()
    {
        // 初期値の設定
        oldHP = HP;
        pos = transform.position;
        ChargeBar.value = 0.0f;
        canCharge = true;

        // HPゲージの生成
        GameObject HPcanvas = Resources.Load<GameObject>("HP Canvas");
        GameObject canvas = Instantiate(HPcanvas, transform.position, Quaternion.identity);
        canvas.transform.SetParent(transform);
        GameObject slider = canvas.transform.Find("HP").gameObject;
        HPBar = slider.GetComponent<Slider>();
        HPBar.maxValue = HP;
    }


    void Update()
    {
        // HPの更新
        HPBar.value = HP;

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
                GameObject obj = Instantiate(arrow, pos + Vector3.right, Quaternion.Euler(angle * Vector3.forward));
                // 矢のパラメータを変更
                Arrow script = obj.GetComponent<Arrow>();
                script.ATK = ATK;
                script.penetration = penetration;
                script.fixedDamage = fixedDamage;
                // 矢を射出
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce((arrowSpeed + chargeLevel) * direction.normalized, ForceMode2D.Impulse);
                // クールダウンの開始
                StartCoroutine(CoolDown());
            }
        }

        // HPの変化を確認
        received = oldHP - HP;
        // HPが変化したとき
        if (received != 0)
        {
            // HPが0になったら破壊
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                SpriteRenderer sprite = GetComponent<SpriteRenderer>();

                // ダメージを受けた時
                if (received > 0)
                {
                    // ダメージエフェクト
                    StartCoroutine(Damage(sprite));
                }
                // 回復した時
                else if (received < 0)
                {
                    // 回復エフェクト
                    StartCoroutine(Heal(sprite));
                }
            }
        }
        // HPを更新
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
        GameObject fill = ChargeBar.transform.Find("Fill Area/Fill").gameObject;
        Image image = fill.GetComponent<Image>();
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
    

    // ダメージエフェクト
    IEnumerator Damage(SpriteRenderer sprite)
    {
        sprite.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        sprite.color = Color.white;
    }


    // 回復エフェクト
    IEnumerator Heal(SpriteRenderer sprite)
    {
        sprite.color = Color.green;

        yield return new WaitForSeconds(0.5f);

        sprite.color = Color.white;
    }
}
