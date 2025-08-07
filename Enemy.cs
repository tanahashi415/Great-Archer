using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 敵に関するスクリプト
public class Enemy : MonoBehaviour
{
    private float oldHP;            // 前フレームのHP
    private bool canMove;           // 移動できるか
    private bool canAttack;         // 攻撃できるか
    private float received;         // 被ダメージ
    private float coolTime;         // 攻撃のクールタイム
    protected PlayerControl script; // 接触相手のプレイヤーのスクリプト
    protected Vector2 initialPos;   // 初期位置
    private Slider HPbar;           // HPバーのインスタンス
    private Animator animator;      // アニメーターのインスタンス

    [Header("基礎パラメータ")]
    public float HP;        // 体力
    public float ATK;       // 攻撃力
    public float ATKSPD;    // 攻撃速度
    public float DEF;       // 防御力
    public float SPD;       // 移動スピード


    void Start()
    {
        // 初期値の設定
        oldHP = HP;
        canMove = true;
        canAttack = false;
        coolTime = 0.0f;
        initialPos = transform.position;

        // HPゲージの生成
        GameObject HPcanvas = Resources.Load<GameObject>("HP Canvas");
        GameObject canvas = Instantiate(HPcanvas, transform.position, Quaternion.identity);
        canvas.transform.SetParent(transform);
        GameObject slider = canvas.transform.Find("HP").gameObject;
        HPbar = slider.GetComponent<Slider>();
        HPbar.maxValue = HP;
        // アニメーターの取得
        animator = transform.Find("Attack Area").gameObject.GetComponent<Animator>();
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
            animator.SetTrigger("attack");
            Attack();
            canAttack = false;
            coolTime = 0.0f;
        }

        // HPの更新
        HPbar.value = HP;

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
        script.HP -= ATK;
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


    // プレイヤーまで到達したとき
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            script = collision.gameObject.GetComponent<PlayerControl>();
            canMove = false;
        }
    }


    // プレイヤーから離れた時
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canMove = true;
        }
    }
}
