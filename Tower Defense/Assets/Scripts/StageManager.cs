using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    public GameObject[] stageButtons;
    public GameObject lockPicture;
    public GameObject starPicture;
    public List<StageData> stageList;

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

        // ログイン済の場合はサーバーからステージのデータを取り出す
        if (UserInfoManager.instance.UserName != null)
        {
            StartCoroutine(LoadDataFromServer());
        }
        // ログインしていない場合はPlayerPrefsからステージのデータを取り出す
        else
        {
            LoadDataFromPlayerPrefs();
        }
    }

    private IEnumerator LoadDataFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(UserInfoManager.instance.ApiBaseUrl + "api/records/" + UserInfoManager.instance.UserName);
        yield return request.Send();
        JSONObject json = new JSONObject(request.downloadHandler.text);
        for (int i = 0; i < json.Count; i++)
        {
            int stageNum = int.Parse(json[i].GetField("StageNum").ToString()) - 1;
            int stars = int.Parse(json[i].GetField("Stars").ToString());
            stageList[stageNum].starNumber = stars;
        }
        UnlockStages();
        ShowButtonPictures();
    }

    /// <summary>
    /// 各ステージの星の数を取得し、次のステージを解除していく
    /// </summary>
    private void LoadDataFromPlayerPrefs()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            if (PlayerPrefs.HasKey(stageList[i].stageName))
            {
                stageList[i].starNumber = PlayerPrefs.GetInt(stageList[i].stageName);
            }
        }
        UnlockStages();
        ShowButtonPictures();
    }

    private void UnlockStages()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            // 最後のステージ以外次のステージを解除
            if (stageList[i].starNumber >= 1 && i < stageList.Count - 1)
            {
                stageList[i + 1].locked = false;
            }
        }
    }

    private void ShowButtonPictures()
    {
        foreach (GameObject button in stageButtons)
        {
            button.GetComponent<StageButton>().ShowPictures();
        }
    }
}
