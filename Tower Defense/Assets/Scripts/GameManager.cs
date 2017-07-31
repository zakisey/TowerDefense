using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//コメント
public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        Normal,
        UnitPlacing
    }

    public static GameManager instance = null;
    private GameMode mode = GameMode.Normal;
    private GameObject unitToPlace = null;
    private GameObject title;

    /*comment for testing conflicting pull request*/

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
                    print("socket clicked");
                    if (mode == GameMode.UnitPlacing)
                    {
                        BoardManager.instance.SetUnitOnSocket(unitToPlace);
                        ChangeGameMode(GameMode.Normal);
                    }
                    break;
            }
        }
    }

    private void OnGUI()
    {
        ShowUnitToPlaceOnCursor();
    }

    // タイトル画面
    private void SetTitlePage()
    {
        title = GameObject.Find("Title");
    }

    // ユニットを設置するモードのとき、マウスカーソルの位置に設置するユニットを表示する
    private void ShowUnitToPlaceOnCursor()
    {
        if (unitToPlace == null) return;

        Texture texture = unitToPlace.GetComponent<Renderer>().GetComponent<Texture>();
        GUI.DrawTexture(new Rect(10, 10, 64, 64), texture);
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
    }

    public void EndGame()
    {
        print("game over!");
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
}
