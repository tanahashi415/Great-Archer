using UnityEngine;

// 投げるタイプの敵キャラ
public class Throw : Enemy
{
    [Header("特殊パラメータ")]
    public GameObject ThrowingObj;  // 投擲物
    public float stopPos_x;         // 止まる位置

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
            // 初速を計算
            float distance = transform.position.x - target.transform.position.x;
            float power = Mathf.Sqrt(9.81f * distance);

            // 投擲物を生成
            GameObject obj = Instantiate(ThrowingObj, transform.position + Vector3.left, Quaternion.identity);
            // 投擲物のパラメータを変更
            Throwing script = obj.GetComponent<Throwing>();
            script.ATK = ATK;
            script.penetration = 0;
            script.fixedDamage = 0;
            // 投擲物を射出
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce(power * new Vector2(-1, 1).normalized, ForceMode2D.Impulse);
        }
        else
        {
            ATKSPD = 0.0f;
            transform.Translate(-SPD * Time.deltaTime, 0, 0);
        }
    }
}
