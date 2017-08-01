using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public GameObject enemy1;
    public Vector2 enemy1Origin;
    public GameObject enemy2;
    public Vector2 enemy2Origin;

    public float enemyPopInterval;
    public int numberOfWaves;
    public List<int> waveEnemyNumber;

    public Transform enemyHolder;
    private GameObject instance;

    private int currentWave = 1;
    private int enemiesPopped = 0;

    public void StartGeneration()
    {
        enemyHolder = new GameObject("Enemies").transform;
        StartCoroutine("CoGenerateEnemies");
    }

    void Update()
    {
        if (currentWave > numberOfWaves)
        {
            StopCoroutine("CoGenerateEnemies");
            print("Generation Stopped");
            BoardManager.instance.SetWaveText(-1);
        }
    }

    /// <summary>
    /// 敵を生成するコルーチン
    /// </summary>
    private IEnumerator CoGenerateEnemies()
    {
        while (currentWave <= numberOfWaves)
        {
            BoardManager.instance.SetWaveText(currentWave);

            while (enemiesPopped < waveEnemyNumber[currentWave - 1])
            {
                InstantiateEnemyFromWave();
                enemiesPopped++;
                yield return new WaitForSeconds(enemyPopInterval);
            }

            while (enemyHolder.transform.childCount != 0)
            {
                yield return new WaitForEndOfFrame();
            }

            enemiesPopped = 0;
            currentWave++;

            print("wait 5 secs for another wave");
            yield return new WaitForSeconds(5.0f);
        }
    }

    /// <summary>
    /// Wave数から出す敵とスポーン位置を変更する
    /// </summary>
    private void InstantiateEnemyFromWave()
    {
        switch (currentWave)
        {
            case 1:
                instance = Instantiate(enemy1, enemy1Origin, Quaternion.identity);
                instance.transform.SetParent(enemyHolder);
                break;
            case 2:
                instance = Instantiate(enemy1, enemy1Origin, Quaternion.identity);
                instance.transform.SetParent(enemyHolder);
                break;
            default:
                instance = Instantiate(enemy1, enemy1Origin, Quaternion.identity);
                instance.transform.SetParent(enemyHolder);
                break;
        }
    }

    public void DestroyEnemyHolder()
    {
        Destroy(enemyHolder.gameObject);
    }
}