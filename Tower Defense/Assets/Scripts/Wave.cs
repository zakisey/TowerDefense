using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave : System.Object
{
    // このWaveで出現する敵のPrefab
    public GameObject enemy;
    // このWaveで出現する敵のデータ
    public float atk;
    public float speed;
    public float hp;
    public int money;
    public int pathNum;
    // このWaveで出現する敵の数
    public int enemyCount;
    // このWaveにかかる秒数
    public float durationSec;
    // 前のWaveが始まってからこのWaveが始まるまでの待ち時間の秒数
    public float waitSec;

    /*
    public int enemyNumber;
    public GameObject enemy;
    public Transform enemyHolder;
    private int enemiesPopped = 0;
    private float enemyPopInterval = 1.0f;
    private float updateTimer = 0.0f;
    public WaveManager waveManager;


    /*　最終的にDBからWaveを生成するに向けて保留
    public Wave(string monsterID)
    {
        enemy1 = GameObject.Find(monsterID);
        enemyHolder = WaveManager.instance.enemyHolder;
    }

    void Start()
    {
        updateTimer = enemyPopInterval;
    }

    void Update()
    {
        if (updateTimer >= enemyPopInterval)
        {
            if (enemiesPopped < enemyNumber)
            {
                waveManager.isPopping = true;
                InstantiateEnemy();
            }
            else
            {
                waveManager.isPopping = false;
                Destroy(this.gameObject);
            }
            updateTimer = 0;
        }
        updateTimer += Time.deltaTime;
    }
    
    /// <summary>
    /// 敵の沸かす
    /// </summary>
    private void InstantiateEnemy()
    {
        GameObject enemyinstance = Instantiate(enemy);
        enemyinstance.transform.SetParent(enemyHolder);
        enemiesPopped++;
    }
    */
}
