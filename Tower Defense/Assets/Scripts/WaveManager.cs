using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveManager : MonoBehaviour
{
    public static WaveManager instance = null;
    // List<Vector2>をシリアライズするためのラッパークラス
    [System.Serializable]
    public class Path
    {
        public List<Vector2> list;
    }
    public List<Path> pathList;
    public List<Wave> waves;

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

    private void Start()
    {
        StartCoroutine("PopWaves");
    }

    // wavesからひとつずつWaveを取り出し、敵を涌かせる
    IEnumerator PopWaves()
    {
        for (int wi = 0; wi < waves.Count; wi++)
        {
            // waveを涌かせる前の待ち時間
            BoardManager.instance.SetWaveText(wi, waves.Count);
            yield return new WaitForSeconds(waves[wi].waitSec);
            BoardManager.instance.SetWaveText(wi + 1, waves.Count);
            // waveの中の敵を一体ずつ涌かせる
            for (int ei = 0; ei < waves[wi].enemyCount; ei++)
            {
                BoardManager.instance.GenerateEnemy(waves[wi].enemy, waves[wi].atk, waves[wi].speed, waves[wi].hp, waves[wi].money, pathList[waves[wi].pathNum].list);
                float interval = waves[wi].durationSec / waves[wi].enemyCount;
                yield return new WaitForSeconds(interval);
            }
        }
    }

    public void Stop()
    {
        StopCoroutine("PopWaves");
    }

    /*
    public List<Wave> waveList;
    private GameObject enemyHolder;
    private Wave thisWave;
    private int currentWave = 0;
    private float waveInterval = 15.0f;
    private float updateTimer = 0.0f;
    public bool isPopping = true;

    void Start()
    {
        enemyHolder = new GameObject("Enemies");
        updateTimer = waveInterval;
    }

    void Update()
    {
        if (currentWave < waveList.Count)
        {
            if (updateTimer >= waveInterval)
            {
                BoardManager.instance.SetWaveText("Wave: " + (currentWave + 1) + " / " + waveList.Count);
                thisWave = Instantiate(waveList[currentWave]);
                thisWave.enemyHolder = this.enemyHolder.transform;
                thisWave.waveManager = this;
                currentWave++;

                updateTimer = 0;
            }
            updateTimer += Time.deltaTime;
        }
        else if (!isPopping && enemyHolder.transform.childCount == 0 && !GameManager.instance.isGameOver)
        {
            BoardManager.instance.SetWaveText("Cleared!!");
            GameManager.instance.EndGame();
        }
    }

    /// <summary>
    /// 失敗時全ての敵を消去用
    /// </summary>
    public void Terminate()
    {
        if (thisWave != null) Destroy(thisWave.gameObject);
        Destroy(enemyHolder);
        Destroy(this.gameObject);
    }
    */
}