using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waveList;
    public Transform enemyHolder;

    public static WaveManager instance = null;

    private Wave thisWave;
    private int currentWave = 0;
    private float waveInterval = 15.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (currentWave >= waveList.Count && enemyHolder.childCount == 0)
        {
            StopCoroutine("CoGenerateEnemies");
            print("Cleared");
            BoardManager.instance.SetWaveText(-1);
        }
    }

    public void StartGeneration()
    {
        enemyHolder = new GameObject("Enemies").transform;
        StartCoroutine("CoGenerateEnemies");
    }

    /// <summary>
    /// WaveListから順次にWaveを呼び出す
    /// </summary>
    private IEnumerator CoGenerateEnemies()
    {
        while (currentWave < waveList.Count)
        {
            BoardManager.instance.SetWaveText(currentWave + 1);
            thisWave = Instantiate(waveList[currentWave]);
            thisWave.StartWave();

            yield return new WaitForSeconds(waveInterval);

            currentWave++;
        }

        yield break;
    }

    /// <summary>
    /// 失敗時全ての敵を消去用
    /// </summary>
    public void Terminate()
    {
        Destroy(enemyHolder.gameObject);
        if(thisWave != null) thisWave.KillSelf();
        Destroy(thisWave);
    }
}