using System.Collections;
using UnityEngine;

// 商店と敵の出現を制御するスクリプト
public class WaveSystemManager : MonoBehaviour
{
    private GameObject player;
    private Coroutine coroutine;        // コールチン格納用

    // =====商店関連=====
    private GameObject obj;         // 店員のインスタンス
    private SpriteRenderer sprite;  // 店員のスプライトレンダラーインスタンス
    private float initialPos_x;     // 店員の初期位置
    private float SPEED = 7.0f;     // 店員の移動スピード

    [Header("商店の設定")]
    public GameObject fairy;        // 店員
    public float stopPos_x;         // 止まる位置
    public GameObject storePanel;   // 商店パネル


    // =====敵関連=====
    private float t;                // 経過時間

    [System.Serializable]
    // 敵の出現する時間と種類
    public struct Element
    {
        public float time;          // 出現時間
        public GameObject enemy;    // 出現する敵
    }

    [System.Serializable]
    // 敵をまとめたもの
    public struct Wave
    {
        public Element[] wave;
    }

    [Header("ステージ構成")]
    public Wave[] stage;  // ステージ構成を定義



    void Start()
    {
        t = 0;
        obj = Instantiate(fairy, fairy.transform.position, Quaternion.identity);
        initialPos_x = obj.transform.position.x;
        sprite = obj.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        coroutine = StartCoroutine(WaveSystem(stage));
    }


    void Update()
    {
        t += Time.deltaTime;
        if (player == null)
        {
            // 前のコールチンを停止
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }


    IEnumerator WaveSystem(Wave[] stage)
    {
        // 商店 → Wave → 商店 → Wave → ...
        foreach (Wave _wave in stage)
        {
            PlayerControl.isStore = true;
            // 店員入場
            sprite.flipX = false;
            while (obj.transform.position.x < stopPos_x)
            {
                obj.transform.Translate(SPEED * Time.deltaTime, 0, 0);
                yield return null;
            }
            sprite.flipX = true;

            // 商店パネルを表示
            storePanel.SetActive(true);
            yield return null;

            // 商品を抽選
            StoreManager.reset = true;

            // 買い物が終わるまで待機
            yield return new WaitUntil(() => StoreManager.isExit);
            yield return new WaitForSeconds(1f);
            StoreManager.isExit = false;

            // 商店パネルを非表示
            storePanel.SetActive(false);

            // 店員退出
            while (obj.transform.position.x > initialPos_x)
            {
                obj.transform.Translate(-SPEED * Time.deltaTime, 0, 0);
                yield return null;
            }
            PlayerControl.isStore = false;


            // Wave開始
            t = 0;
            Debug.Log("Wave開始");
            foreach (Element element in _wave.wave)
            {
                yield return new WaitUntil(() => t > element.time);
                Instantiate(element.enemy, element.enemy.transform.position, Quaternion.identity);
            }

            // 敵がいなくなるまで待機
            yield return new WaitUntil(() => GameObject.FindWithTag("Enemy") == null);
            Debug.Log("Wave終了");
        }

        // ステージクリアの処理をここに書く
        PlayerControl.isStore = true;
        GameOverManager.StageClear();
        Debug.Log("ステージクリア");
    }
}
