using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public GameObject lockPicture;
    public GameObject starPicture;
    public int index;

    private string stageName;
    private bool locked;
    private int starNumber;

    // Use this for initialization
    void Start()
    {
        locked = StageManager.instance.stageList[index].locked;
        starNumber = StageManager.instance.stageList[index].starNumber;
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
            float xAdjustment = -this.transform.GetComponent<RectTransform>().rect.width / 4;
            float yAdjustment = -this.transform.GetComponent<RectTransform>().rect.height / 2;
            for (int i = 0; i < starNumber; i++)
            {
                Instantiate(starPicture, new Vector2(this.transform.position.x + xAdjustment, this.transform.position.y + yAdjustment), Quaternion.identity, this.transform);
                xAdjustment += this.transform.GetComponent<RectTransform>().rect.width / 4;
            }
        }
    }

    public void LoadStage()
    {
        SceneManager.LoadScene(stageName);
    }
}
