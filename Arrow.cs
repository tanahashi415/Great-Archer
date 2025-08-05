using UnityEngine;

// 矢の処理に関するスクリプト
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject weightPos;    // 力をかける位置

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ボーダーに触れたら破壊
        if (collision.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }
    }
}
