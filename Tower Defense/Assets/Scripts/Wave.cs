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
    public bool isFloat;
    public int money;
    public int pathNum;
    // このWaveで出現する敵の数
    public int enemyCount;
    // このWaveにかかる秒数
    public float durationSec;
    // 前のWaveが始まってからこのWaveが始まるまでの待ち時間の秒数
    public float waitSec;
}
