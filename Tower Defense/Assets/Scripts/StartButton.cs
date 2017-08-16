using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

	public void LoadMain()
    {
        SceneManager.LoadScene("Main");
        //Application.LoadLevel("Main");
    }
}
