using UnityEngine;

// 投擲物（敵の攻撃）に関するスクリプト
public class Throwing : Arrow
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        // ボーダーに触れたら破壊
        if (tag == "Border")
        {
            Destroy(gameObject);
        }
        // プレイヤーに触れたらダメージを与える
        else if (tag == "Player")
        {
            // ダメージ処理
            PlayerControl script = collision.gameObject.GetComponent<PlayerControl>();
            script.HP -= ATK;

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

    protected override void FixedUpdate()
    {

    }
}
