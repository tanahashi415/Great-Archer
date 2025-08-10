using UnityEngine;
using System.Collections;

public class WhiteTiger : Enemy
{
    [Header("特殊パラメータ")]
    public float stopPos_x;     // 止まる位置
    public float arrivalTime;   // 敵に到達するまでの時間

    protected override void Move()
    {
        transform.Translate(-SPD * Time.deltaTime, 0, 0);
        // 停止位置まで動く
        if (transform.position.x < stopPos_x)
        {
            canMove = false;
        }
    }

    protected override void Attack()
    {
        // プレイヤーを検索
        GameObject target = GameObject.FindWithTag("Player");

        if (target != null)
        {
            // プレイヤーの位置を取得
            Vector2 targetPos = target.transform.position;
            // 攻撃
            StartCoroutine(Go(targetPos));
        }
        else
        {
            // プレイヤーが見つからなかった場合そのまま直進
            ATKSPD = 0.0f;
            transform.Translate(-SPD * Time.deltaTime, 0, 0);
        }

    }


    IEnumerator Go(Vector2 targetPos)
    {
        float distance = transform.position.x - (targetPos.x + 1);
        float v_x = distance / arrivalTime;
        float v_y = 9.81f * arrivalTime / 2.0f;

        // プレイヤーの位置まで移動
        float t = 0.0f;
        while (t < arrivalTime)
        {
            transform.Translate(-v_x * Time.deltaTime, (v_y - 9.81f * t) * Time.deltaTime, 0);
            yield return null;
            t += Time.deltaTime;
        }

        // 攻撃
        base.Attack();

        // 元の位置に戻る
        t = 0.0f;
        while (t < arrivalTime)
        {
            transform.Translate(v_x * Time.deltaTime, (v_y - 9.81f * t) * Time.deltaTime, 0);
            yield return null;
            t += Time.deltaTime;
        }
    }


    protected override IEnumerator Defeat(SpriteRenderer sprite)
    {
        while (sprite.color.a > 0.0f)
        {
            sprite.color = sprite.color - new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}
