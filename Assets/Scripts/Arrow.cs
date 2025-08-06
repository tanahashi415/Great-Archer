using UnityEngine;

// 矢の処理に関するスクリプト
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;

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

    void FixedUpdate()
    {
        // 矢尻に力をかける
        rb.AddForceAtPosition(0.07f * Vector2.down, weightPos.transform.position, ForceMode2D.Force);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ボーダーに触れたら破壊
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
        // 敵に触れたらダメージを与える
        else if (collision.gameObject.tag == "Enemy")
        {
            // ダメージ処理
            Enemy script = collision.gameObject.GetComponent<Enemy>();
            float speed = rb.linearVelocity.magnitude;
            float damage = (1 + 0.1f * (speed - 10.0f)) * ATK - script.DEF;
            if (damage < 0)
            {
                damage = 0;
            }
            Debug.Log(damage);
            script.HP -= damage + fixedDamage;

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
}
