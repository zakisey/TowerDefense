﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance = null;
    // Prefabs
    public GameObject playersBasePrefab;
    public GameObject socketPrefab;
    public GameObject[] groundTiles;
    // シーン内のオブジェクトへの参照
    public GameObject lifeText;
    public GameObject waveText;
    public GameObject moneyText;
    private GameObject playersBase;

    public List<string> boardData;
    public List<Vector2> socketPosList;
    public Vector2 playersBasePos;

    // ボード上の要素を入れておくholder
    private Transform boardHolder;

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

    public void SetMoneyText(int money)
    {
        moneyText.GetComponent<Text>().text = "Money: $" + money;
    }

    public void SetLifeText(int life)
    {
        lifeText.GetComponent<Text>().text = "Life: " + life;
    }

    public void SetWaveText(int currentWaveNum, int maxWaveNum)
    {
        waveText.GetComponent<Text>().text = "Wave: " + currentWaveNum + " / " + maxWaveNum;
    }

    // コストを見て配備可能なユニットのボタンだけ有効にする
    public void SetUnitButtonsState()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("UnitButton");
        foreach (GameObject button in buttons)
        {
            GameObject unit = button.GetComponent<UnitButton>().unit;
            button.GetComponent<Button>().interactable = GameManager.instance.IsUsableUnit(unit);
        }
    }

    // クリア判定(HPが0より大きくて敵が残っていないときクリア)
    public bool IsCleared()
    {
        return playersBase.GetComponent<PlayersBase>().HP > 0 && HasNoEnemy();
    }

    public void SetupScene()
    {
        boardHolder = new GameObject("Board").transform;
        BoardSetup();
        GeneratePlayersBase();
        GenerateSocket();
    }

    // EnemyのPrefabと移動経路を受け取って、敵を盤面に配置する
    public void GenerateEnemy(GameObject enemyPrefab, float atk, float speed, float hp, int money, List<Vector2> path)
    {
        GameObject enemy = Instantiate(enemyPrefab, path[0], Quaternion.identity, boardHolder);
        enemy.GetComponent<Enemy>().Init(atk, speed, hp, money, path);
    }

    /// <summary>
    /// ユニットをソケットに置く
    /// </summary>
    /// <param name="unit">置くユニット</param>
    public void SetUnitOnSocket(GameObject unit, GameObject socket)
    {
        GameObject instance = Instantiate(unit, socket.transform.position, Quaternion.identity) as GameObject;
        instance.transform.SetParent(socket.transform);
    }

    // 盤面の敵を全て削除する
    public void DestroyAllEnemies()
    {
        foreach (Enemy enemy in boardHolder.transform.GetComponentsInChildren<Enemy>())
        {
            Destroy(enemy.gameObject);
        }
    }

    // 盤面に敵がまだ残っているかどうか
    private bool HasNoEnemy()
    {
        return boardHolder.transform.GetComponentInChildren<Enemy>() == null;
    }

    // 文字列のリストからタイルを生成して配置する
    private void BoardSetup()
    {
        for (int y = 0; y < boardData.Count; y++)
        {
            string s = boardData[y];
            for (int x = 0; x < s.Length; x++)
            {
                char c = s[x];
                GameObject toInstantiate = groundTiles[c - 'a'];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x + 0.5f, boardData.Count - y - 1 + 0.5f, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private void GeneratePlayersBase()
    {
        playersBase = Instantiate(playersBasePrefab, playersBasePos, Quaternion.identity) as GameObject;
        playersBase.transform.SetParent(boardHolder);
    }

    private void GenerateSocket()
    {
        foreach (Vector2 pos in socketPosList)
        {
            Instantiate(socketPrefab, pos, Quaternion.identity, boardHolder);
        }
    }
}
