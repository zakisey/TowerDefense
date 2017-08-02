using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance = null;
    private int rows = 12;
    private int columns = 15;
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

    private const string boardText =
        "aaaaaaaaaaadeaa" +
        "aaaaaaaaaaadeaa" +
        "aahbbbbbbbbjeaa" +
        "aadlcccklcccfaa" +
        "aadeaaadeaaaaaa" +
        "aadeaaadeaaaaaa" +
        "bbjeaaadeaaaaaa" +
        "cccfaaadeaahgaa" +
        "aaaaaaadeaadeaa" +
        "aaaaaaadmbbjeaa" +
        "aaaaaaaiccccfaa" +
        "aaaaaaaaaaaaaaa";

    /// <summary>
    /// 文字列からボードのグラフィックを生成して配置する
    /// </summary>
    void BoardSetup()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                char c = boardText[y * columns + x];
                GameObject toInstantiate = groundTiles[c - 'a'];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x+0.5f, rows - y - 1+0.5f, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /// <summary>
    /// ツールバー(？)にいるユニットの表示
    /// </summary>
    void GeneratePlayersBase()
    {
        GameObject instance = Instantiate(playersBase, new Vector3(12f, 4f, 0f), Quaternion.identity) as GameObject;
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
        GameObject instance = Instantiate(socket, new Vector3(6.5f, 7.5f, 0f), Quaternion.identity) as GameObject;
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
