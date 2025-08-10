using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreManager : MonoBehaviour
{
    private TextMeshProUGUI messageText;
    private string defaultMessage = "森を守ってくれてありがとう！\nコインと引き換えに力をあげるよ！";

    public static bool isExit;  // 商店退出判定用
    public static bool reset;   // 商品抽選トリガー

    [Header("商品を陳列する棚を設定")]
    public GameObject[] choices;    // 商品を陳列する棚

    [System.Serializable]
    // 商品
    public struct Item
    {
        public string itemName; // 商品名
        public Sprite image;    // 商品画像
        public int price;       // 価格
        public float value;     // パラメータ数値
        public int ID;          // 識別タグ
    }

    [Header("陳列する商品を設定")]
    public Item[] items;    // 商品リスト


    void Start()
    {
        isExit = false;
        reset = false;
        GameObject message = transform.Find("Message").gameObject;
        messageText = message.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (reset)
        {
            CoinManager.coin += 5;
            Display();
            reset = false;
        }
    }

    // 陳列する商品の抽選
    public void Display()
    {
        if (CanPay(5))
        {
            Item[] display = new Item[choices.Length];
            for (int i = 0; i < display.Length; i++)
            {
                // 商品リストから商品をランダムに選ぶ
                display[i] = items[Random.Range(0, items.Length)];
                choices[i].SetActive(true);
                ItemManager script = choices[i].GetComponent<ItemManager>();
                script.itemName = display[i].itemName;
                script.image = display[i].image;
                script.price = display[i].price;
                script.value = display[i].value;
                script.ID = display[i].ID;
            }
        }
    }


    public void Buy()
    {
        // 押されたボタンの取得
        GameObject button_ob = EventSystem.current.currentSelectedGameObject;
        // 押されたボタンの親からスクリプトを取得
        GameObject choice = button_ob.transform.parent.gameObject;
        ItemManager script = choice.GetComponent<ItemManager>();

        // プレイヤーを検索
        GameObject target = GameObject.FindWithTag("Player");

        if (target != null)
        {
            PlayerControl status = target.GetComponent<PlayerControl>();
            // プレイヤーの数値を変更
            switch (script.ID)
            {
                // チャージスピード
                case 0:
                    if (CanPay(script.price))
                    {
                        status.chargeSpeed += script.value;
                        choice.SetActive(false);
                    }
                    break;
                // クールタイム
                case 1:
                    if (CanPay_CoolTime(script.price, status.coolTime, script.value))
                    {
                        status.coolTime += script.value;
                        choice.SetActive(false);
                    }
                    break;
                // 貫通数
                case 2:
                    if (CanPay(script.price))
                    {
                        status.penetration += (int)script.value;
                        choice.SetActive(false);
                    }
                    break;
                // 矢の速さ
                case 3:
                    if (CanPay(script.price))
                    {
                        status.arrowSpeed += script.value;
                        choice.SetActive(false);
                    }
                    break;
                // 固定ダメージ
                case 4:
                    if (CanPay(script.price))
                    {
                        status.fixedDamage += script.value;
                        choice.SetActive(false);
                    }
                    break;
                // コンボ時間延長
                case 5:
                    if (CanPay(script.price))
                    {
                        ComboManager.timeLimit += script.value;
                        choice.SetActive(false);
                    }
                    break;
                // 最大体力
                case 6:
                    if (CanPay(script.price))
                    {
                        status.MaxHP += script.value;
                        status.HP += script.value;
                        choice.SetActive(false);
                    }
                    break;
                // 攻撃力
                case 7:
                    if (CanPay(script.price))
                    {
                        status.ATK += script.value;
                        choice.SetActive(false);
                    }
                    break;
                // 回復
                case 8:
                    if (CanPay(script.price))
                    {
                        status.HP += script.value;
                        choice.SetActive(false);
                    }
                    break;
            }
        }
    }

    // 退店
    public void Exit()
    {
        messageText.text = "頑張れ僕らのヒーロー！";
        isExit = true;
    }

    bool CanPay(int price)
    {
        if (CoinManager.coin - price < 0)
        {
            messageText.text = "お金が足りないよ";
            SoundManager.instance.PlaySE(SoundManager.instance.cantBuySE, 1.0f);
            return false;
        }
        else
        {
            CoinManager.coin -= price;
            messageText.text = defaultMessage;
            SoundManager.instance.PlaySE(SoundManager.instance.buySE, 1.0f);
            return true;
        }
    }

    bool CanPay_CoolTime(int price, float coolTime, float value)
    {
        if (CoinManager.coin - price < 0)
        {
            messageText.text = "お金が足りないよ";
            SoundManager.instance.PlaySE(SoundManager.instance.cantBuySE, 1.0f);
            return false;
        }
        else
        {
            if (coolTime + value < 0)
            {
                messageText.text = "君の能力値はもう最大だよ";
                SoundManager.instance.PlaySE(SoundManager.instance.cantBuySE, 1.0f);
                return false;
            }
            else
            {
                CoinManager.coin -= price;
                messageText.text = defaultMessage;
                SoundManager.instance.PlaySE(SoundManager.instance.buySE, 1.0f);
                return true;
            }
        }
    }
}
