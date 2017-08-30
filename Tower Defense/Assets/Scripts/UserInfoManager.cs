using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager instance;
    private string userName;
    public string UserName
    {
        get { return userName; }
        set
        {
            userName = value;
            SetUserInfoText();
        }
    }
    public string ApiBaseUrl;

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

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンロード時にユーザー名を表示する
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUserInfoText();
    }

    public void SetUserInfoText()
    {
        GameObject userInfoText = GameObject.Find("UserInfoText");
        if (userInfoText)
        {
            userInfoText.GetComponent<Text>().text = (userName == null || userName == "") ? "You are not logged in." : "Welcome, " + userName + "!";
        }
    }
}
