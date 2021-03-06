﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public GameObject unit;
    public GameObject unitButtonTooltip;
    [Multiline]public string tooltipText;
    private GameObject tooltip;

    public void OnClick()
    {
        if (GameManager.instance.IsUsableUnit(unit))
        {
            GameManager.instance.ChangeGameModeToUnitPlacing(unit);
        }
    }

    public void RemoveTooltip()
    {
        Destroy(tooltip);
    }

    public void ShowTooltip()
    {
        tooltip = Instantiate(unitButtonTooltip, new Vector3(0, 0, 0), Quaternion.identity);
        SetTooltipPos();
        SetTooltipText();
        tooltip.transform.SetParent(GameObject.Find("Canvas").transform, true);
    }

    private void SetTooltipText()
    {
        Text text = tooltip.GetComponentInChildren<Text>();
        text.text = tooltipText;
    }

    private void SetTooltipPos()
    {
        tooltip.transform.position = new Vector2(Input.mousePosition.x + 110, Input.mousePosition.y - 85);
    }

    private void Update()
    {
        if (tooltip != null) SetTooltipPos();
    }

}
