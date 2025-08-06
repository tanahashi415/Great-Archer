using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// プレイヤー操作に関するスクリプト
public class PlayerControl : MonoBehaviour
{
    private float chargeTime;   // 溜め時間
    private int chargeLevel;    // 溜め段階
    private Vector3 pos;        // キャラクターの座標
    private bool canCharge;     // 溜め可能か

    public Slider slider;               // 溜めゲージ
    public GameObject chargeMaxEffect;  // 溜め完了エフェクト
    public GameObject chargeEffect;     // 溜めエフェクト

    [Header("基礎パラメータ")]
    public GameObject arrow;    // 生成する矢
    public float chargeSpeed;   // 溜めスピード
    public float coolTime;      // クールタイム
    public int penetration;     // 貫通数
    public float arrowSpeed;    // 矢の速さ
    public float fixedDamage;   // 矢の固定ダメージ
    
    public float HP;            // 体力
    public float ATK;           // 攻撃力


    void Start()
    {
        pos = transform.position;
        slider.value = 0.0f;
        canCharge = true;
    }


    void Update()
    {
        if (canCharge)
        {
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
                chargeMaxEffect.SetActive(true);
            }

            // 左ボタン長押しで弓を引く
            if (Input.GetMouseButtonDown(0))
            {
                chargeTime = 0.0f;
            }
            else if (Input.GetMouseButton(0))
            {
                chargeTime += Time.deltaTime;
                chargeEffect.SetActive(true);
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
                // 矢のパラメータを変更
                Arrow script = obj.GetComponent<Arrow>();
                script.ATK = ATK;
                script.penetration = penetration;
                script.fixedDamage = fixedDamage;
                // 矢を射出
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce((arrowSpeed + chargeLevel) * direction.normalized, ForceMode2D.Impulse);
                // クールダウンの開始
                StartCoroutine(CoolDown());
            }
        }
    }

    // クールダウン
    IEnumerator CoolDown()
    {
        canCharge = false;
        chargeMaxEffect.SetActive(false);
        chargeEffect.SetActive(false);
        chargeTime = 0.0f;

        // ゲージを青色に変更
        GameObject fill = slider.transform.Find("Fill Area/Fill").gameObject;
        Image image = fill.GetComponent<Image>();
        image.color = Color.blue;

        // 溜め時間リセット
        float rate = slider.value / coolTime;
        while (slider.value > 0.0f)
        {
            slider.value -= rate * Time.deltaTime;
            yield return null;
        }

        image.color = Color.yellow;
        canCharge = true;
    }
}
