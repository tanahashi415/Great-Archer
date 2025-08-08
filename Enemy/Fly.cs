using UnityEngine;
using System.Collections;

// 飛ぶタイプの敵キャラ
public class Fly : Enemy
{
    private float t = 0;        // 経過時間

    [Header("特殊パラメータ")]
    public float A;             // 振幅
    public float omega;         // 角速度
    public float stopPos_x;     // 止まる位置
    public float arrivalTime;   // 敵に到達するまでの時間


    protected override void Move()
    {
        // 移動
        t += Time.deltaTime;
        transform.Translate(-SPD * Time.deltaTime, A * omega * Mathf.Cos(omega * t) * Time.deltaTime, 0);

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
            // プレイヤーまでの距離と速度を計算
            float distance_x = transform.position.x - target.transform.position.x;
            float v_x = distance_x / arrivalTime;
            float distance_y = transform.position.y - target.transform.position.y;
            float v_y = distance_y / arrivalTime;
            // 攻撃
            StartCoroutine(Go(v_x, v_y));
        }
        else
        {
            // プレイヤーが見つからなかった場合そのまま直進
            ATKSPD = 0.0f;
            t += Time.deltaTime;
            transform.Translate(-SPD * Time.deltaTime, A * omega * Mathf.Cos(omega * t) * Time.deltaTime, 0);
        }
        
    }


    IEnumerator Go(float v_x, float v_y)
    {
        // プレイヤーの位置まで移動
        float _t = 0.0f;
        while (_t < arrivalTime)
        {
            transform.Translate(-v_x * Time.deltaTime, -v_y * Time.deltaTime, 0);
            yield return null;
            _t += Time.deltaTime;
        }

        // 攻撃
        base.Attack();

        // 元の位置に戻る
        _t = 0.0f;
        while (_t < arrivalTime)
        {
            transform.Translate(v_x * Time.deltaTime, v_y * Time.deltaTime, 0);
            yield return null;
            _t += Time.deltaTime;
        }
    }
}
