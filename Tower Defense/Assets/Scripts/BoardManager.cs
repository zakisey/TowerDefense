using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance = null;
    private int rows = 12;
    private int columns = 15;
    private int money;
    public GameObject playersBase;
    private GameObject enemyOnBoard;
    public GameObject socket;
    public GameObject[] groundTiles;
    public GameObject waveManagerPrefab;
    private GameObject waveManager;
    private List<Vector3> gridPositions = new List<Vector3>();
    public Transform boardHolder;
    private Text lifeText;
    private Text waveText;
    private Text moneyText;

    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            SetUnitButtonsSate();
            SetMoneyText();
        }
    }

    private void SetMoneyText()
    {
        moneyText.text = "Money: " + Money + "$";
    }

    private void SetUnitButtonsSate()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("UnitButton");
        foreach (GameObject button in buttons)
        {
            GameObject unit = button.GetComponent<UnitButton>().unit;
            button.GetComponent<Button>().interactable = IsUsableUnit(unit);
        }
    }

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

    void GenerateSocket()
    {
        Instantiate(socket, new Vector3(6.5f, 7.5f, 0f), Quaternion.identity, boardHolder);
        Instantiate(socket, new Vector3(9.5f, 3.5f, 0f), Quaternion.identity, boardHolder);
    }

    void GenerateEnemy()
    {
       waveManager = Instantiate(waveManagerPrefab);
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
        }
    }

    public void SetWaveText(string text)
    {
        waveText.text = text;
    }

    /// <summary>
    /// ユニットをソケットに置く
    /// </summary>
    /// <param name="unit">置くユニット</param>
    public void SetUnitOnSocket(GameObject unit, GameObject socket)
    {
        GameObject instance = Instantiate(unit, socket.transform.position, Quaternion.identity) as GameObject;
        instance.transform.SetParent(socket.transform);
        Money -= unit.GetComponent<Unit>().cost;//お金を払う
    }


    public void SetupScene()
    {
        boardHolder = new GameObject("Board").transform;
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        Money = 10;
        BoardSetup();
        GeneratePlayersBase();
        GenerateSocket();
        GenerateEnemy();
    }
   
    /// <summary>
    /// ユニットがお金の面で配置可能かを判断
    /// </summary>
    /// <param name="unit">配置するユニット</param>
    /// <returns></returns>
    public bool IsUsableUnit(GameObject unit)
    {
        return unit.GetComponent<Unit>().cost <= money;
    }
}
