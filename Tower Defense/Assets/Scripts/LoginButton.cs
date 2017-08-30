using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButton : MonoBehaviour
{
    public GameObject usernameForm;

    public void OnClick()
    {
        usernameForm.SetActive(true);
    }
}
