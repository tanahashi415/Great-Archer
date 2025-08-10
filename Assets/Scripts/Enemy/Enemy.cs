using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

// 敵に関するスクリプト
public class Enemy : MonoBehaviour
{
    private float oldHP;                // 前フレームのHP
    protected bool canMove;             // 移動できるか
    private bool canAttack;             // 攻撃できるか
    private float received;             // 被ダメージ
    private float coolTime;             // 攻撃のクールタイム
    protected PlayerControl script;     // 接触相手のプレイヤーのスクリプト
    protected Vector2 initialPos;       // 初期位置
    private Slider HPbar;               // HPバーのインスタンス
    private Animator animator;          // アニメーターのインスタンス
    private TextMeshProUGUI damageText; // ダメージ表記のテキスト
    private GameObject coinEmmision;    // コイン放出のゲームオブジェクト
    private Coroutine coroutine;        // コールチン格納用

    [Header("基礎パラメータ")]
    public float MaxHP;        // 体力
    public float ATK;       // 攻撃力
    public float ATKSPD;    // 攻撃速度
    public float DEF;       // 防御力
    public float SPD;       // 移動スピード
    public int coin;        // 獲得コイン

    [Header("設定しなくていもの")]
    public float HP;            // 現在の体力


    protected virtual void Start()
    {
        // HPゲージの生成
        GameObject HPcanvas = Resources.Load<GameObject>("HP Canvas");
        GameObject canvas = Instantiate(HPcanvas, transform.position, Quaternion.identity);
        canvas.transform.SetParent(transform);
        GameObject slider = canvas.transform.Find("HP").gameObject;
        HPbar = slider.GetComponent<Slider>();
        HPbar.maxValue = HP;
        // ダメージ表記テキストの取得
        GameObject text = canvas.transform.Find("Damage").gameObject;
        damageText = text.GetComponent<TextMeshProUGUI>();
        damageText.enabled = false;
        // アニメーターの取得
        animator = transform.Find("Attack Area").gameObject.GetComponent<Animator>();
        // コイン放出の設定
        if (coin == 0)
        {
            coinEmmision = null;
        }
        else if (coin < 10)
        {
            coinEmmision = Resources.Load<GameObject>("Coin Emission 5");
        }
        else if (coin < 100)
        {
            coinEmmision = Resources.Load<GameObject>("Coin Emission 10");
        }
        else
        {
            coinEmmision = Resources.Load<GameObject>("Coin Emission 40");
        }

        // 初期値の設定
        HP = MaxHP;
        oldHP = HP;
        canMove = true;
        canAttack = false;
        coolTime = 0.0f;
        initialPos = transform.position;
    }


    void Update()
    {
        // 移動
        if (canMove)
        {
            Move();
        }
        else
        {
            Moved();
            // 攻撃のクールタイム
            coolTime += Time.deltaTime;
            if (coolTime > ATKSPD)
            {
                canAttack = true;
            }
        }

        // 攻撃
        if (canAttack)
        {
            Attack();
            canAttack = false;
            coolTime = 0.0f;
        }

        // HPの更新
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
        HPbar.value = HP;

        // HPの変化を確認
        received = oldHP - HP;
        // HPが変化したとき
        if (received != 0)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            // HPが0になったら撃破
            if (HP <= 0)
            {
                ATK = 0;
                // 何度も撃破できてしまう対策
                if (oldHP > 0)
                {
                    // コインの放出
                    if (coinEmmision)
                    {
                        SoundManager.instance.PlaySE(SoundManager.instance.coinSE, 1.0f);
                        Instantiate(coinEmmision, transform.position, Quaternion.identity);
                    }
                    CoinManager.coin += coin;
                    // 撃破エフェクト
                    StartCoroutine(Defeat(sprite));
                }
            }
            else
            {
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
        }
        // oldHPを更新
        oldHP = HP;
    }


    // 移動
    protected virtual void Move()
    {
        transform.Translate(-SPD * Time.deltaTime, 0, 0);
    }


    // 移動完了
    protected virtual void Moved()
    {

    }


    // 攻撃
    protected virtual void Attack()
    {
        animator.SetTrigger("attack");
        script.HP -= ATK;
    }


    // 撃破エフェクト
    protected virtual IEnumerator Defeat(SpriteRenderer sprite)
    {
        while (sprite.color.a > 0.0f)
        {
            sprite.color = sprite.color - new Color(0, 0, 0, 2.0f * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
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


    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーまで到達したとき動けないようになる
        if (collision.gameObject.tag == "Player")
        {
            script = collision.gameObject.GetComponent<PlayerControl>();
            canMove = false;
        }
        // ボーダーに触れると破壊
        else if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
    }


    // プレイヤーから離れた時
    void OnTriggerExit2D(Collider2D collision)
    {
        // 動けるようになる
        if (collision.gameObject.tag == "Player")
        {
            canMove = true;
        }
    }
}
