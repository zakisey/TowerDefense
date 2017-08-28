using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

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
        InitialiseState();
    }

    /// <summary>
    /// 各ステージの星の数を取得し、次のステージを解除していく
    /// </summary>
    private void InitialiseState()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            if (PlayerPrefs.HasKey(stageList[i].stageName))
            {
                stageList[i].starNumber = PlayerPrefs.GetInt(stageList[i].stageName);
            }

            if (stageList[i].starNumber >= 1 && i < stageList.Count - 1) // 最後のステージ以外次のステージを解除
                stageList[i + 1].locked = false;
        }

    }
}
