using UnityEngine;
using System.Collections;

public class WhiteTiger : Enemy
{
    private bool summon = false;

    [Header("特殊パラメータ")]
    public float stopPos_x;     // 止まる位置
    public float arrivalTime;   // 敵に到達するまでの時間
    public GameObject[] enemies;    // 召喚する敵

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
            if (summon)
            {
                // 召喚リストから敵をランダムに選ぶ
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];
                // 敵を召喚
                SoundManager.instance.PlaySE(SoundManager.instance.tigerSE, 1.0f);
                GameObject instance = Instantiate(enemy, enemy.transform.position, Quaternion.identity);
                // HPを2倍に強化
                if (instance.GetComponent<Enemy>() != null)
                {
                    Enemy script = instance.GetComponent<Enemy>();
                    script.MaxHP *= 2.0f;
                }
                else if (instance.GetComponent<Fly>() != null)
                {
                    Fly script = instance.GetComponent<Fly>();
                    script.MaxHP *= 2.0f;
                }
                else if (instance.GetComponent<Jump>() != null)
                {
                    Jump script = instance.GetComponent<Jump>();
                    script.MaxHP *= 2.0f;
                }
                else if (instance.GetComponent<Throw>() != null)
                {
                    Throw script = instance.GetComponent<Throw>();
                    script.MaxHP *= 2.0f;
                }
                // 次は攻撃
                summon = false;
            }
            else
            {
                // プレイヤーの位置を取得
                Vector2 targetPos = target.transform.position;
                // 攻撃
                StartCoroutine(Go(targetPos));
                // 次は召喚
                summon = true;
            }
            
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
