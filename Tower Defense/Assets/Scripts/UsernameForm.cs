using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UsernameForm : MonoBehaviour
{
    public GameObject confirmRegisterPanel;
    public GameObject userInfoText;
    private string userName;

    public void OnValueChanged(string s)
    {
        userName = s;
    }

    public void OnClickLoginOk()
    {
        if (userName == "" || userName == null) return;
        StartCoroutine(TryLogin());
    }

    public void OnClickLoginCancel()
    {
        gameObject.SetActive(false);
    }

    public void OnClickRegisterYes()
    {
        StartCoroutine(TryRegister());
    }

    public void OnClickRegisterNo()
    {
        confirmRegisterPanel.SetActive(false);
    }

    private IEnumerator TryRegister()
    {
        WWWForm form = new WWWForm();
        form.AddField("Name", userName);
        UnityWebRequest request = UnityWebRequest.Post(UserInfoManager.instance.ApiBaseUrl + "api/users/", form);
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
                Debug.Log("created new user: " + userName);
                UserInfoManager.instance.UserName = userName;
                gameObject.SetActive(false);
            }
        }
        confirmRegisterPanel.SetActive(false);
    }

    private IEnumerator TryLogin()
    {
        UnityWebRequest request = UnityWebRequest.Get(UserInfoManager.instance.ApiBaseUrl + "api/users/" + userName);
        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 404)
            {
                // ユーザーが存在しないので、新しく作るかどうか尋ねる
                confirmRegisterPanel.SetActive(true);
            }
            else
            {
                // ユーザーが存在するので、ログイン成功
                UserInfoManager.instance.UserName = userName;
                gameObject.SetActive(false);
            }
        }
    }
}
