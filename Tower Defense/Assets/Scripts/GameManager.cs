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
    /// <summary>
    /// ゲームスコア表示用
    /// </summary>
    public GameObject Star;
    private int money;
    private GameMode mode = GameMode.Normal;
    private GameObject unitToPlace = null;
    private bool isCleared = false;
    /// <summary>
    /// ゲーム結果表示用
    /// </summary>
    private GameObject GameOverScreen, ClearScreen;
    private GameObject unitUpgradeMenu;

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
                        Money -= unitToPlace.GetComponent<Unit>().initialCost;
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
        return unit.GetComponent<Unit>().initialCost <= Money;
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
        Money = 100;

        //表示用のオブジェクトを取得し、非表示にする
        GameOverScreen = GameObject.Find("GameOver");
        ClearScreen = GameObject.Find("Clear");
        unitUpgradeMenu = GameObject.Find("UnitUpgradeMenu");
        
        GameOverScreen.SetActive(false);
        ClearScreen.SetActive(false);
        unitUpgradeMenu.SetActive(false);
    }

    public void ShowUnitUpgradeMenu(GameObject unitToUpgrade)
    {
        UnitUpgradeMenu menu = unitUpgradeMenu.GetComponent<UnitUpgradeMenu>();
        menu.SetUnitToUpgrade(unitToUpgrade);
        unitUpgradeMenu.SetActive(true);
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
        DisplayClearScreen();
    }

    private void DisplayGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }
    private void DisplayClearScreen()
    {
        if (ClearScreen.activeInHierarchy) return;
        ClearScreen.SetActive(true);

        //ベースのライフをもとに星を表示
        //ベースは1つしかないことを前提としてbaseLifeを取得している
        float baseLife = FindObjectOfType<PlayersBase>().HP, MaxLife = 10f;
        //配置の際はキャンバスのscaleを考慮して場所を決める(デザインが楽なので)
        Vector3 canvasScale = GameObject.Find("Canvas").transform.localScale;
        //生成した星の情報を変えるためだけのオブジェクト
        GameObject starObject;
        //1つ目の星には何もしない
        Instantiate(Star, ClearScreen.transform.position + new Vector3(-100 * canvasScale.x, 5 * canvasScale.y, 0), Quaternion.identity, ClearScreen.transform);
        //2つ目以降はスコアによって色を変える
        starObject = Instantiate(Star, ClearScreen.transform.position + new Vector3(  0 * canvasScale.x, 5 * canvasScale.y, 0), Quaternion.identity, ClearScreen.transform);
        if (baseLife / MaxLife < 0.5) starObject.GetComponent<Image>().color = new Color(0, 0, 0);
        starObject = Instantiate(Star, ClearScreen.transform.position + new Vector3(100 * canvasScale.x, 5 * canvasScale.y, 0), Quaternion.identity, ClearScreen.transform);
        if (baseLife / MaxLife <  1 ) starObject.GetComponent<Image>().color = new Color(0, 0, 0);
    }

    public void OnClickToReturn()
    {
        print("return");
        SceneManager.LoadScene("StageSelection");

    }
    public void OnClickToRetry()
    {
        print("retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
