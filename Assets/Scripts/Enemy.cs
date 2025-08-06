using UnityEngine;
using System.Collections;

// 敵の防御に関するスクリプト
public class Enemy : MonoBehaviour
{
    private float oldHP;    // 前フレームのHP

    [Header("基礎パラメータ")]
    public float HP;    // 体力
    public float DEF;   // 防御力

    void Start()
    {
        oldHP = HP;
    }

    void Update()
    {
        // HPが0になったら破壊
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            // ダメージを受けた時
            if ((oldHP - HP) > 0)
            {
                // ダメージエフェクト
                StartCoroutine(Damage(sprite));
            }
            // 回復した時
            else if ((oldHP - HP) < 0)
            {
                // 回復エフェクト
                StartCoroutine(Heal(sprite));
            }
        }
        // HPを更新
        oldHP = HP;
    }

    // ダメージエフェクト
    IEnumerator Damage(SpriteRenderer sprite)
    {
        sprite.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        sprite.color = Color.white;
    }

    // 回復エフェクト
    IEnumerator Heal(SpriteRenderer sprite)
    {
        sprite.color = Color.green;

        yield return new WaitForSeconds(0.5f);

        sprite.color = Color.white;
    }
}
