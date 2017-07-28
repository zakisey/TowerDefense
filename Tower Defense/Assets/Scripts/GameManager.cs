using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private int mode = 0;

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

    }

    void InitGame()
    {
        BoardManager.instance.SetupScene();
    }

    public void EndGame()
    {
        print("game over!");
    }

    public void ChangeGameMode(int mode)
    {
        this.mode = mode;
    }

}
