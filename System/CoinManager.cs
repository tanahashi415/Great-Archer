using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static int coin;

    public TextMeshProUGUI coinAmount;

    void Start()
    {
        coin = 30;    
    }

    void Update()
    {
        coinAmount.text = coin.ToString();
    }
}
