using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButton : MonoBehaviour {

    public GameObject unit;

    public void OnClick()
    {
        GameManager.instance.ChangeGameModeToUnitPlacing(unit);
    }
}
