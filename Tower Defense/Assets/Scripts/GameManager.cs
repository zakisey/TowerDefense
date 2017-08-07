﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//コメント
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
    private GameMode mode = GameMode.Normal;
    private GameObject unitToPlace = null;
    private GameObject title;

    public bool isGameOver = false;

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
        Time.timeScale = 0.0f;
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

        if (isGameOver && Input.anyKey)
        {
            SceneManager.LoadScene("Start");
        }
    }

    private void OnGUI()
    {
        ShowUnitToPlaceOnCursor();
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
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
        
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
    }

    public void EndGame()
    {
        isGameOver = true;
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
