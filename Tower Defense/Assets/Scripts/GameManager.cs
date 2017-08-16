using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        Normal,
        UnitPlacing
    }

    public static GameManager instance = null;
    public Button pauseButton;
    public Button resumeButton;
    public Button fastButton;
    private int money;
    private GameMode mode = GameMode.Normal;
    private GameObject unitToPlace = null;
    private bool isCleared = false;
    /// <summary>
    /// 結果表示用
    /// </summary>
    private GameObject GameOverScreen, ClearScreen;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            BoardManager.instance.SetUnitButtonsState();
            BoardManager.instance.SetMoneyText(money);
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
        PauseGame();
    }

    private void Start()
    {
        InitGame();
    }

    private void Update()
    {
        // クリック処理
        GameObject clicked = GetClickedObject();

        if (clicked != null)
        {
            switch (clicked.tag)
            {
                case "Socket":
                    if (mode == GameMode.UnitPlacing)
                    {
                        Money -= unitToPlace.GetComponent<Unit>().cost;
                        BoardManager.instance.SetUnitOnSocket(unitToPlace, clicked.gameObject);
                    }
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            unitToPlace = null;
            ChangeGameMode(GameMode.Normal);
        }

        // クリア判定
        if (WaveManager.instance.isCleared && BoardManager.instance.IsCleared())
        {
            if (!isCleared)
            {
                isCleared = true;
                ClearGame();
            }
        }
    }

    private void OnGUI()
    {
        ShowUnitToPlaceOnCursor();
    }

    /// <summary>
    /// ユニットがお金の面で配置可能かを判断
    /// </summary>
    /// <param name="unit">配置するユニット</param>
    /// <returns></returns>
    public bool IsUsableUnit(GameObject unit)
    {
        return unit.GetComponent<Unit>().cost <= Money;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseButton.interactable = false;
        resumeButton.interactable = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        resumeButton.interactable = false;
        pauseButton.interactable = true;
    }

    public void FastForwardGame()
    {
        // TODO: 早送り機能の実装
    }

    // ユニットを設置するモードのとき、マウスカーソルの位置に設置するユニットを表示する
    private void ShowUnitToPlaceOnCursor()
    {
        if (unitToPlace == null) return;

        Texture texture = unitToPlace.GetComponent<SpriteRenderer>().sprite.texture;
        Texture textureCanon = unitToPlace.transform.Find("Canon").GetComponent<SpriteRenderer>().sprite.texture;
        // Vector3でマウス位置座標を取得する
        Vector3 position = Input.mousePosition;
        position = new Vector3(position.x, Screen.height - position.y, 0f);
        
        GUI.DrawTexture(new Rect(position.x - 32, position.y - 32, 64, 64), texture);
        GUI.DrawTexture(new Rect(position.x - 32, position.y - 32 - 10, 64, 64), textureCanon);
    }

    // 左クリックされたオブジェクトを取得
    private GameObject GetClickedObject()
    {
        Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collision = Physics2D.OverlapPoint(tapPoint);
        return (Input.GetMouseButtonDown(0) && collision) ? collision.transform.gameObject : null;
    }

    public void InitGame()
    {
        BoardManager.instance.SetupScene();
        Money = 10;

        GameOverScreen = GameObject.Find("GameOver");
        ClearScreen = GameObject.Find("Clear");

        GameOverScreen.SetActive(false);
        ClearScreen.SetActive(false);
    }

    // ゲームオーバー(HPが0)になったときに呼ばれる
    public void EndGame()
    {
        WaveManager.instance.Stop();
        BoardManager.instance.DestroyAllEnemies();
        DisplayGameOverScreen();
    }

    public void ChangeGameMode(GameMode mode)
    {
        this.mode = mode;
    }

    public void ChangeGameModeToUnitPlacing(GameObject unitToPlace)
    {
        this.mode = GameMode.UnitPlacing;
        this.unitToPlace = unitToPlace;
    }

    // ゲームをクリア(Waveが全て終わり、敵が盤面におらず、HPが1以上)したときに呼ばれる
    private void ClearGame()
    {
        print("clear");
        DisplayClearScreen();
    }

    private void DisplayGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }

    private void DisplayClearScreen()
    {
        ClearScreen.SetActive(true);
    }
}
