using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public GameObject unit;

    public void OnClick()
    {
        if (BoardManager.instance.IsUsableUnit(unit))
        {
            GameManager.instance.ChangeGameModeToUnitPlacing(unit);
        }
    }
}
