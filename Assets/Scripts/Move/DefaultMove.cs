using UnityEngine;

// 敵の攻撃・移動に関するスクリプト
public class DefaultMove : MonoBehaviour
{
    private bool canMove;   // 移動できるか

    public float ATK;   // 攻撃力
    public float SPD;   // 移動スピード

    void Start()
    {
        canMove = true;
    }

    void Update()
    {
        // 移動
        if (canMove)
        {
            transform.Translate(-SPD * Time.deltaTime, 0, 0);
        }
        // 攻撃
        else
        {

        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stop")
        {
            canMove = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stop")
        {
            canMove = true;
        }
    }
}
