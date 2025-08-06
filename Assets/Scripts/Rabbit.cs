using UnityEngine;

// 跳ねるタイプの敵キャラ
public class Rabbit : Enemy
{
    private float t;  // 滞空時間

    [Header("特殊パラメータ")]
    public float jumpPower; // ジャンプ力

    protected override void Move()
    {
        // 鉛直投げ上げ
        if (transform.position.y > initialPos.y)
        {
            t += Time.deltaTime;
            transform.Translate(-SPD * Time.deltaTime, (jumpPower - 9.8f * t) * Time.deltaTime, 0);
        }
        else
        {
            transform.Translate(-SPD * Time.deltaTime, jumpPower * Time.deltaTime, 0);
            t = 0.0f;
        }
    }

    protected override void Moved()
    {
        // 鉛直投げ上げ
        if (transform.position.y > initialPos.y)
        {
            t += Time.deltaTime;
            transform.Translate(0, (jumpPower - 9.8f * t) * Time.deltaTime, 0);
        }
    }
}
