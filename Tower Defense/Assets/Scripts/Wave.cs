using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int enemyNumber;

    public GameObject enemy1;
    public GameObject enemy2;

    private GameObject enemyToPop;

    private Transform enemyHolder;

    private int enemiesPopped = 0;
    private float enemyPopInterval = 1.0f;
    
    private GameObject enemyinstance;

    /*　最終的にDBからWaveを生成するに向けて保留
    public Wave(string monsterID)
    {
        enemy1 = GameObject.Find(monsterID);
        enemyHolder = WaveManager.instance.enemyHolder;
    }*/

    public void StartWave()
    {
        enemyHolder = WaveManager.instance.enemyHolder;
        StartCoroutine(CoStartWave());
    }

    /// <summary>
    /// Waveのインスタンスの動きを制御する
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoStartWave()
    {
        while (enemiesPopped < enemyNumber)
        {
            RandomChoice();
            InstantiateEnemy();
            yield return new WaitForSeconds(enemyPopInterval);
        }

        KillSelf();
    }

    /// <summary>
    /// 次に沸かす敵を選ぶ
    /// </summary>
    private void RandomChoice()
    {
        int temp = (int)Random.Range(1.0f, 3.0f);

        switch (temp)
        {
            case 1:
                enemyToPop = enemy1;
                break;
            case 2:
                enemyToPop = enemy2;
                break;
        }
    }

    /// <summary>
    /// 敵の沸かす
    /// </summary>
    private void InstantiateEnemy()
    {
        enemyinstance = Instantiate(enemyToPop);
        enemyinstance.transform.SetParent(enemyHolder);
        enemiesPopped++;
    }

    public void KillSelf()
    {
        Destroy(this.gameObject);
    }
}
