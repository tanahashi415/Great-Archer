using UnityEngine;

// 宝箱系モンスター
public class Treasure : Throw
{
    private int count = 0;  // 攻撃回数

    public int limit = 0;   // 攻撃回数上限

    // 攻撃回数上限までは攻撃
    protected override void Attack()
    {
        if (count < limit)
        {
            base.Attack();
            count++;
        }
    }

    // 上限を超えたら逃げる
    protected override void Moved()
    {
        if (count >= limit)
        {
            transform.Translate(SPD * Time.deltaTime, 0, 0);
        }
    }
}
