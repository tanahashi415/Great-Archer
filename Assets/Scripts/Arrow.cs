using UnityEngine;
using System.Collections;

// 矢の処理に関するスクリプト
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy script;           // 敵のスクリプトのインスタンス

    public int penetration;         // 貫通数
    public float ATK;               // 矢のダメージ
    public float fixedDamage;       // 矢の固定ダメージ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        float angle = Vector3.SignedAngle(Vector3.right, rb.linearVelocity, Vector3.forward);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        // ボーダーに触れたら破壊
        if (tag == "Border")
        {
            Time.timeScale = 1.0f;
            Destroy(gameObject);
        }
        // 敵に触れたらダメージを与える
        else if (tag == "Enemy")
        {
            // ダメージ計算
            script = collision.gameObject.GetComponent<Enemy>();
            Damage(1.0f);
            StartCoroutine(HitStop(collision.gameObject.transform));
        }
        // 弱点に触れたらダメージ2倍
        else if (tag == "WeakPoint")
        {
            // ダメージ計算
            script = collision.gameObject.transform.parent.gameObject.GetComponent<Enemy>();
            Damage(2.0f);
            StartCoroutine(HitStop(collision.gameObject.transform));
        }
    }

    // ダメージ処理
    void Damage(float rate)
    {
        // コンボ加算
        ComboManager.Combo();
        // ダメージ計算
        float speed = rb.linearVelocity.magnitude;
        float damage = (1 + 0.1f * (speed - 10.0f)) * ATK - script.DEF; // 基本計算式
        float comboBonus = 1 + 0.1f * ComboManager.combo;
        damage = rate * comboBonus * damage;       // ボーナス計算式
        if (damage < 0)
        {
            damage = 0;
        }
        // ダメージ処理
        script.HP -= damage + fixedDamage;
        // SE
        SoundManager.instance.PlayHitSE(comboBonus);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        // ボーダーに触れたら破壊
        if (tag == "Border")
        {
            Time.timeScale = 1.0f;
            Destroy(gameObject);
        }
    }

    // ヒットストップ
    IEnumerator HitStop(Transform target)
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.03f);  
        // ザ・ワールド！
        Time.timeScale = 0.0f;

        // 振動させる
        if (target != null)
        {
            target.position = new Vector2(target.position.x + 0.1f, target.position.y);
            yield return wait;
        }
        if (target != null)
        {
            target.position = new Vector2(target.position.x - 0.2f, target.position.y);
            yield return wait;
        }
        if (target != null)
        {
            target.position = new Vector2(target.position.x + 0.1f, target.position.y);
            yield return wait;
        }

        // そして時は動き出す
        Time.timeScale = 1.0f;

        // 貫通処理
        if (penetration > 0)
        {
            penetration--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
