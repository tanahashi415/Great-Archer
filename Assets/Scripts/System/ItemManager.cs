using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 商品を設定・表示するスクリプト
public class ItemManager : MonoBehaviour
{
    public string itemName; // 商品名
    public Sprite image;    // 商品画像
    public int price;       // 価格
    public float value;     // パラメータ数値
    public int ID;          // 識別タグ

    // 上記を設定するインスタンス
    private TextMeshProUGUI _itemName;
    private Image _image;
    private TextMeshProUGUI _price;


    void Start()
    {
        // インスタンスを取得
        _itemName = transform.Find("Explaination").gameObject.GetComponent<TextMeshProUGUI>();
        _image = transform.Find("Image").gameObject.GetComponent<Image>();
        _price = transform.Find("Buy/Price").gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // 商品の表示を変更
        _itemName.text = itemName;
        _image.sprite = image;
        _price.text = price.ToString();
    }
}
