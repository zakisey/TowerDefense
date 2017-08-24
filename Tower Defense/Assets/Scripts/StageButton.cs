using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public int index;

    private GameObject lockPicture;
    private GameObject starPicture;
    private string stageName;
    private bool locked;
    private int starNumber;
    public int StarNumber
    {
        get
        {
            return starNumber;
        }
        set
        {
            starNumber = value;
            if (starNumber > 3)
            {
                starNumber = 3;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        lockPicture = StageManager.instance.lockPicture;
        starPicture = StageManager.instance.starPicture;
        locked = StageManager.instance.stageList[index].locked;
        StarNumber = StageManager.instance.stageList[index].starNumber;
        stageName = StageManager.instance.stageList[index].stageName;
        this.gameObject.GetComponent<Image>().sprite = StageManager.instance.stageList[index].stageImage;

        ShowPictures();
    }

    private void ShowPictures()
    {
        if (locked)
        {
            this.GetComponent<Button>().interactable = false;
            Instantiate(lockPicture, this.transform);
        }
        else
        {
            float xAdjustment = -this.transform.GetComponent<RectTransform>().rect.width / 3;
            float yAdjustment = -this.transform.GetComponent<RectTransform>().rect.height / 1.5f;
            Debug.Log(transform.position.y);
            for (int i = 0; i < StarNumber; i++)
            {
                Instantiate(starPicture, new Vector2(this.transform.position.x + xAdjustment, this.transform.position.y + yAdjustment), Quaternion.identity, this.transform);
                xAdjustment += this.transform.GetComponent<RectTransform>().rect.width / 3;
            }
        }
    }

    public void LoadStage()
    {
        SceneManager.LoadScene(stageName);
    }
}
