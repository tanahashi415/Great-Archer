using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// プレイヤー操作に関するスクリプト
public class PlayerControl : MonoBehaviour
{
    private float chargeTime;   // 溜め時間
    private int chargeLevel;    // 溜め段階
    private Vector3 pos;        // キャラクターの座標

    public Slider slider;       // 溜めゲージ

    [Header("基礎パラメータ")]
    public GameObject arrow;    // 生成する矢
    public float chargeSpeed;   // 溜めスピード
    public int penetration;     // 貫通数
    public float arrowSpeed;    // 矢の速さ
    public int HP;              // 体力
    public int ATK;             // 攻撃力


    void Start()
    {
        pos = transform.position;
        slider.value = 0.0f;
    }


    void Update()
    {
        // 左ボタン長押しで弓を引く
        if (Input.GetMouseButtonDown(0))
        {
            chargeTime = 0.0f;
        }
        else if (Input.GetMouseButton(0))
        {
            chargeTime += Time.deltaTime;
        }
        // 左ボタンを離して矢を射る
        else if (Input.GetMouseButtonUp(0))
        {
            // マウスの位置を取得し、ワールド座標に変換
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + 10.0f * Vector3.forward;
            // 射出角度の計算
            Vector3 direction = mousePos - pos;
            float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
            // 矢を生成
            GameObject obj = Instantiate(arrow, pos + Vector3.right, Quaternion.Euler(angle * Vector3.forward));
            // 矢を射出
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce((arrowSpeed + chargeLevel) * direction.normalized, ForceMode2D.Impulse);
            // 溜め時間リセット
            chargeTime = 0.0f;
        }

        // 溜めの進捗表示
        slider.value = chargeSpeed * chargeTime;
        // 溜め段階
        if (slider.value < slider.maxValue)
        {
            chargeLevel = 0;
        }
        else if (slider.value >= slider.maxValue)
        {
            chargeLevel = 1;
        }
    }
}
