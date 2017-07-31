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

    // 左クリックされたオブジェクトを取得
    private GameObject GetClickedObject()
    {
        GameObject result = null;
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
            if (collition2d)
            {
                result = collition2d.transform.gameObject;
            }
        }

        return result;
    }

    void InitGame()
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
