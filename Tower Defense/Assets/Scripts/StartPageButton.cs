using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StartPageButton : MonoBehaviour
{
    public GameObject MainButtons;
    public GameObject ConfirmClearScreen;

    public AudioSource startSound;

    public void StartGame()
    {
        AudioManager.instance.PlaySound(startSound);
        SceneManager.LoadScene("StageSelection");
    }

    /// <summary>
    /// ClearSaveを確認する画面を出す
    /// </summary>
    public void ShowClearConfirmation()
    {
        ConfirmClearScreen.SetActive(true);
        MainButtons.SetActive(false);
    }

    /// <summary>
    /// ClearSave画面のYes
    /// </summary>
    public void DoConfirmClearYes()
    {
        ConfirmClearScreen.SetActive(false);
        MainButtons.SetActive(true);

        if (UserInfoManager.instance.UserName != null)
        {
            // ログイン済ならサーバーからユーザーのデータを削除する
            StartCoroutine(DeleteRecords());
        }
        else
        {
            // ログインしていなければPlayerPrefsのデータを削除する
            PlayerPrefs.DeleteAll();
        }

    }

    private IEnumerator DeleteRecords()
    {
        UnityWebRequest request = UnityWebRequest.Delete(UserInfoManager.instance.ApiBaseUrl + "api/records/" + UserInfoManager.instance.UserName);
        yield return request.Send();
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 201)
            {
                // ユーザー作成成功
                Debug.Log("deleted user data: " + UserInfoManager.instance.UserName);
            }
        }
    }

    /// <summary>
    /// ClearSave画面のNo
    /// </summary>
    public void DoConfirmClearNo()
    {
        ConfirmClearScreen.SetActive(false);
        MainButtons.SetActive(true);
    }
}
