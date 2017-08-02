using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance = null;
    private int rows = 8;
    private int columns = 12;
    public GameObject playersBase;
    private GameObject enemyOnBoard;
    public GameObject socket;
    private GameObject socketOnBoard;
    public GameObject[] groundTiles;
    public GameObject waveManager;
    private List<Vector3> gridPositions = new List<Vector3>();
    public Transform boardHolder;
    private Text lifeText;
    private Text waveText;

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

    }

    //ボード(ステージ)ごとにプレハブを作って対処のほうがいいかも
    /// <summary>
    /// ボードの表示？ 
    /// </summary>
    void BoardSetup()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject toInstantiate = groundTiles[7];
                if (y == 2)
                {
                    toInstantiate = groundTiles[2];
                }
                if (y == 3)
                {
                    toInstantiate = groundTiles[1];
                }
                if (y == 4)
                {
                    toInstantiate = groundTiles[0];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /// <summary>
    /// ツールバー(？)にいるユニットの表示
    /// </summary>
    void GeneratePlayersBase()
    {
        GameObject instance = Instantiate(playersBase, new Vector3(11, 3, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(boardHolder);
    }

    //Waveで対処
    /// <summary>
    /// 敵を作る
    /// </summary>
    //void GenerateEnemy()
    //{
    //    StartCoroutine("CreateEnemy");
    //}

    //ボード(ステージ)ごとにプレハブを作って対処のほうがいいかも
    /// <summary>
    /// ユニットを置く場所を生成
    /// </summary>
    void GenerateSocket()
    {
        GameObject instance = Instantiate(socket, new Vector3(5, 1, 0f), Quaternion.identity) as GameObject;
        socketOnBoard = instance;
        instance.transform.SetParent(boardHolder);
    }

    void GenerateEnemy()
    {
        waveManager = Instantiate(waveManager);
        waveManager.GetComponent<WaveManager>().StartGeneration();
    }

    /// <summary>
    /// ベースのライフを表示するためのテキスト表示
    /// </summary>
    /// <param name="life">ベースのライフ</param>
    public void SetLifeText(int life)
    {
        if (life > 0)
        {
            lifeText.text = "Life: " + life;
        }
        else
        {
            lifeText.text = "Game Over!!";
            waveManager.GetComponent<WaveManager>().Terminate();
            Destroy(waveManager);
            GameManager.instance.EndGame();
        }
    }

    public void SetWaveText(int wavenumber)
    {
        if (wavenumber != -1)
        {
            waveText.text = "Wave: " + wavenumber;
        }
        else
        {
            waveText.text = "Cleared!!";
            waveManager.GetComponent<WaveManager>().Terminate();
            Destroy(waveManager);
            GameManager.instance.EndGame();
        }
    }

    //どのソケットに置くかを引数に入れる
    /// <summary>
    /// ユニットをソケットに置く
    /// </summary>
    /// <param name="unit">置くユニット</param>
    public void SetUnitOnSocket(GameObject unit)
    {
        GameObject instance = Instantiate(unit, socketOnBoard.transform.position, Quaternion.identity) as GameObject;
        instance.transform.SetParent(socketOnBoard.transform);
    }


    public void SetupScene()
    {
        boardHolder = new GameObject("Board").transform;
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
        BoardSetup();
        GeneratePlayersBase();
        GenerateSocket();
        GenerateEnemy();
    }

    //使わないかも
    /// <summary>
    /// 敵の名前を探す
    /// </summary>
    /// <returns></returns>
    public GameObject SearchNearestEnemy()
    {
        return enemyOnBoard;
    }
}
