using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPageButton : MonoBehaviour
{
    private GameObject MainButtons;
    private GameObject ConfirmClearScreen;

    void Awake()
    {
        MainButtons = GameObject.Find("MainButtons");
        ConfirmClearScreen = GameObject.Find("ConfirmClear");
    }

    void Start()
    {
        ConfirmClearScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StageSelection");
    }

    public void ClearSave()
    {
        ConfirmClearScreen.SetActive(true);
        MainButtons.SetActive(false);
    }

    /// <summary>
    /// ClearSave画面のYes
    /// </summary>
    public void DoConfirmClearYes()
    {
        Debug.Log("Clear: Yes");

        ConfirmClearScreen.SetActive(false);
        MainButtons.SetActive(true);

        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// ClearSave画面のNo
    /// </summary>
    public void DoConfirmClearNo()
    {
        Debug.Log("Clear: No");

        ConfirmClearScreen.SetActive(false);
        MainButtons.SetActive(true);
    }
}
