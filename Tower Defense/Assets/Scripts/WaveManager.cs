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
    [System.NonSerialized]
    public bool isCleared = false;

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
                // メソッドの呼び出しが冗長なのでなんとかしたい…
                BoardManager.instance.GenerateEnemy(waves[wi].enemy, waves[wi].atk, waves[wi].speed, waves[wi].hp, waves[wi].money, waves[wi].isFloat, pathList[waves[wi].pathNum].list);
                float interval = waves[wi].durationSec / waves[wi].enemyCount;
                yield return new WaitForSeconds(interval);
            }
        }
        isCleared = true;
    }

    // 敵が湧くのを止める
    public void Stop()
    {
        StopCoroutine("PopWaves");
    }
}