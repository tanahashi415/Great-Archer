using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private static int coin;

    public TextMeshProUGUI coinAmount;

    void Start()
    {
        coin = 0;    
    }

    void Update()
    {
        coinAmount.text = coin.ToString();
    }

    public static void GetCoin(int amount)
    {
        coin += amount;
        Debug.Log(amount + "コインゲット!");
        Debug.Log("現在のコイン：" + coin);
    }
}
