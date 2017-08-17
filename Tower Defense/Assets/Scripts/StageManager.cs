using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    // DBに向けてカラムごとリストに
    public List<bool> lockedList;
    public List<string> stageList;
    public List<int> starList;
    public List<Sprite> imageList;

    private List<StageButton> buttonList;
    
    // Use this for initialization
    void Start()
    {

    }

    private void InstantiateButtons()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].locked = lockedList[i];
            buttonList[i].stage = stageList[i];
            buttonList[i].starNumber = starList[i];
            buttonList[i].GetComponent<Image>().sprite = imageList[i];
        }
    }
}
