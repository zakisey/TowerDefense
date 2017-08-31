using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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
    private GameObject StageMenu;
    private Slider SESlider;
    private Slider BGMSlider;

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
                    //条件式の2つ目について
                    //ユニットが置いてない(ソケットが子objectを持たない)時に置ける
                    if (mode == GameMode.UnitPlacing && clicked.transform.childCount == 0)
                    {
                        Money -= unitToPlace.GetComponent<Unit>().initialCost;
                        BoardManager.instance.SetUnitOnSocket(unitToPlace, clicked.gameObject);
                        ChangeToNormalMode();
                    }
                    break;
            }
        }

        //右クリックをするとnormalモードに変わる
        if (Input.GetMouseButtonDown(1))
        {
            ChangeToNormalMode();
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
        fastButton.interactable = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        pauseButton.interactable = true;
        resumeButton.interactable = false;
        fastButton.interactable = true;
        AudioManager.instance.SetPitchResume();
        StageMenu.SetActive(false);
    }

    public void FastForwardGame()
    {
        Time.timeScale = 2.0f;
        pauseButton.interactable = true;
        resumeButton.interactable = true;
        fastButton.interactable = false;
        AudioManager.instance.SetPitchFastForward();
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
        StageMenu = GameObject.Find("StageMenu");
        SESlider = StageMenu.transform.Find("SESlider").GetComponent<Slider>();
        BGMSlider = StageMenu.transform.Find("BGMSlider").GetComponent<Slider>();

        GameOverScreen.SetActive(false);
        ClearScreen.SetActive(false);
        unitUpgradeMenu.SetActive(false);
        StageMenu.SetActive(false);
        // TODO:AudioManagerから音量をもらってSliderのvalueを初期化する

        // AudioManagerの音量に変更を加える
        SESlider.onValueChanged.AddListener(delegate { });
        BGMSlider.onValueChanged.AddListener(delegate { });
    }

    public void ShowUnitUpgradeMenu(GameObject unitToUpgrade)
    {
        if (mode != GameMode.Normal) return;
        UnitUpgradeMenu menu = unitUpgradeMenu.GetComponent<UnitUpgradeMenu>();
        menu.SetUnitToUpgrade(unitToUpgrade);
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

    /// <summary>
    /// モード変更以外の処理も行う(例：マウスについたユニットアイコンを消す)
    /// </summary>
    public void ChangeToNormalMode()
    {
        unitToPlace = null;
        ChangeGameMode(GameMode.Normal);
    }

    // ゲームをクリア(Waveが全て終わり、敵が盤面におらず、HPが1以上)したときに呼ばれる
    private void ClearGame()
    {
        DisplayClearScreen();
        SaveScore();
    }

    private void DisplayGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }
    private void DisplayClearScreen()
    {
        if (ClearScreen.activeInHierarchy) return;
        ClearScreen.SetActive(true);

        // 3つの星を表示させる
        DisplayStar(-100.0f, 0.0f);
        DisplayStar(0.0f, 0.5f);
        DisplayStar(100.0f, 1.0f);
    }

    /// <summary>
    /// 星を表示させ、HPが一定の閾値以下であれば黒くする
    /// </summary>
    /// <param name="xOffset">横方向のオフセット</param>
    /// <param name="hpRatio">HP閾値</param>
    private void DisplayStar(float xOffset, float hpRatio)
    {
        //ベースのライフをもとに星を表示
        float baseLife = FindObjectOfType<PlayersBase>().HP;
        float maxLife = FindObjectOfType<PlayersBase>().maxHP;

        //配置の際はキャンバスのscaleを考慮して場所を決める(デザインが楽なので)
        Vector3 canvasScale = GameObject.Find("Canvas").transform.localScale;

        GameObject starObject = Instantiate(Star, ClearScreen.transform.position + new Vector3(xOffset * canvasScale.x, 5 * canvasScale.y, 0), Quaternion.identity, ClearScreen.transform);

        // HPが閾値以下であれば黒くする
        if (baseLife / maxLife < hpRatio)
            starObject.GetComponent<Image>().color = new Color(0, 0, 0);
    }

    private void SaveScore()
    {
        float baseLife = FindObjectOfType<PlayersBase>().HP, MaxLife = 10f;
        string stageName = SceneManager.GetActiveScene().name;
        int thisScore;

        if (baseLife == MaxLife)
            thisScore = 3;
        else if (baseLife / MaxLife >= 0.5)
            thisScore = 2;
        else
            thisScore = 1;

        // ログイン済の場合はサーバーにデータを保存
        if (UserInfoManager.instance.UserName != null)
        {
            StartCoroutine(PostRecord(stageName.Replace("Stage", ""), thisScore));
        }
        else
        {
            // ログインしていない場合はPlayerPrefsにデータを保存
            if (!PlayerPrefs.HasKey(stageName) || thisScore > PlayerPrefs.GetInt(stageName))
            {
                PlayerPrefs.SetInt(stageName, thisScore);
            }
        }
    }

    private IEnumerator PostRecord(string stageName, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserName", UserInfoManager.instance.UserName);
        form.AddField("StageNum", stageName);
        form.AddField("Stars", score);

        UnityWebRequest request = UnityWebRequest.Post(UserInfoManager.instance.ApiBaseUrl + "api/records", form);
        yield return request.Send();
        print("post " + form.ToString());
    }

    public void OnClickToReturn()
    {
        SceneManager.LoadScene("StageSelection");

    }
    public void OnClickToRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// ゲームを止めてステージメニューを表示
    /// </summary>
    public void OnClickToStageMenu()
    {
        PauseGame();
        StageMenu.SetActive(true);
    }

    public void OnClickCloseStageMenu()
    {
        // TODO:音量のデータをAudioManagerを使ってPlayerPrefsに保存する

        StageMenu.SetActive(false);
    }

    /// <summary>
    /// UnitUpgradeMenuのボタンとunitが重なっているかどうか
    /// </summary>
    public bool UnitIsOverlappedByUpgradeMenu()
    {
        if (!unitUpgradeMenu.activeInHierarchy) return false;
        return unitUpgradeMenu.GetComponent<UnitUpgradeMenu>().ButtonMouseHover();
    }

}
