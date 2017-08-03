using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
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
    }*/

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
}
