using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waveList;
    private GameObject enemyHolder;
    private Wave thisWave;
    private int currentWave = 0;
    private float waveInterval = 15.0f;
    private float updateTimer = 0.0f;

    void Start()
    {
        enemyHolder = new GameObject("Enemies");
        updateTimer = waveInterval;
        print(enemyHolder);
    }

    void Update()
    {
        if (currentWave < waveList.Count)
        {
            if (updateTimer >= waveInterval)
            {
                BoardManager.instance.SetWaveText(currentWave + 1);
                thisWave = Instantiate(waveList[currentWave]);
                thisWave.enemyHolder = this.enemyHolder.transform;
                currentWave++;

                updateTimer = 0;
            }
            updateTimer += Time.deltaTime;
        }
        else if (enemyHolder.transform.childCount == 0)
        {
            print("Cleared");
            BoardManager.instance.SetWaveText(-1);
        }
    }

    /// <summary>
    /// 失敗時全ての敵を消去用
    /// </summary>
    public void Terminate()
    {
        Destroy(thisWave.gameObject);
        Destroy(enemyHolder);
        Destroy(this.gameObject);
    }
}