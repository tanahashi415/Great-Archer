using UnityEngine;
using System.Collections;

// 矢の処理に関するスクリプト
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy script;           // 敵のスクリプトのインスタンス

    public GameObject weightPos;    // 力をかける位置
    public int penetration;         // 貫通数
    public float ATK;               // 矢のダメージ
    public float fixedDamage;       // 矢の固定ダメージ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        // 矢尻に力をかける
        rb.AddForceAtPosition(0.07f * Vector2.down, weightPos.transform.position, ForceMode2D.Force);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        // ボーダーに触れたら破壊
        if (tag == "Border")
        {
            StartCoroutine(DelayDestroy(gameObject));
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
        float speed = rb.linearVelocity.magnitude;
        float damage = rate * ((1 + 0.1f * (speed - 10.0f)) * ATK - script.DEF);
        if (damage < 0)
        {
            damage = 0;
        }
        // ダメージ処理
        script.HP -= damage + fixedDamage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        // ボーダーに触れたら破壊
        if (tag == "Border")
        {
            StartCoroutine(DelayDestroy(gameObject));
        }
    }

    // ヒットストップ
    IEnumerator HitStop(Transform target)
    {
        Time.timeScale = 0.0f;

        target.position = new Vector2(target.position.x + 0.1f, target.position.y);
        yield return new WaitForSecondsRealtime(0.03f);

        target.position = new Vector2(target.position.x - 0.2f, target.position.y);
        yield return new WaitForSecondsRealtime(0.03f);

        target.position = new Vector2(target.position.x + 0.1f, target.position.y);
        yield return new WaitForSecondsRealtime(0.03f);

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

    // ヒットストップとボーダー接触の同時発生回避
    IEnumerator DelayDestroy(GameObject gameObject)
    {
        yield return null;
        Destroy(gameObject);
    }
}
