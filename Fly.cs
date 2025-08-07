using UnityEngine;

// 飛ぶタイプの敵キャラ
public class Fly : Enemy
{
    private float t = 0;    // 経過時間

    [Header("特殊パラメータ")]
    public float A;     // 振幅
    public float omega; // 角速度


    protected override void Move()
    {
        t += Time.deltaTime;
        transform.Translate(-SPD * Time.deltaTime, A * omega * Mathf.Cos(omega * t) * Time.deltaTime, 0);
    }
}
