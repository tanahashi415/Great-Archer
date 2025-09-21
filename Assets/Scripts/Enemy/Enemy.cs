using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// 敵に関するスクリプト
public class Enemy : Entity
{
    protected bool canMove;             // 移動できるか
    private bool canAttack;             // 攻撃できるか
    private float coolTime;             // 攻撃のクールタイム
    protected PlayerControl script;     // 接触相手のプレイヤーのスクリプト
    protected Vector2 initialPos;       // 初期位置
    private Animator animator;          // アニメーターのインスタンス
    private GameObject coinEmmision;    // コイン放出のゲームオブジェクト

    public float ATKSPD;    // 攻撃速度
    public float DEF;       // 防御力
    public float SPD;       // 移動スピード
    public int coin;        // 獲得コイン


    protected override void Awake()
    {
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
        else if (coin < 40)
        {
            coinEmmision = Resources.Load<GameObject>("Coin Emission 10");
        }
        else
        {
            coinEmmision = Resources.Load<GameObject>("Coin Emission 40");
        }

        base.Awake();

        // 初期値の設定
        canMove = true;
        canAttack = false;
        coolTime = 0.0f;
        initialPos = transform.position;
    }


    protected override void Update()
    {
        base.Update();

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
                HPChange(received);
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
