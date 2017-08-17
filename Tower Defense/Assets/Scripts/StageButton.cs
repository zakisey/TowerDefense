using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public GameObject lockPicture;
    public GameObject starPicture;
    
    public bool locked;
    public string stage;
    public int starNumber;

    // Use this for initialization
    void Start()
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
        SceneManager.LoadScene(stage);
    }
=======

public class StageButton : MonoBehaviour {

    public bool locked = false;
    public 

	// Use this for initialization
	void Start () {
		if (locked)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
>>>>>>> ac5519d8547680660f88567074bac46f257aa9de
}
