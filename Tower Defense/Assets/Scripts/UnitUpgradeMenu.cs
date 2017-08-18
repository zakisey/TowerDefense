﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeMenu : MonoBehaviour
{
    public Text upgradeText, sellText;
    public Button upgradeButton;
    private GameObject unitToUpgrade;

    public void SetUnitToUpgrade(GameObject unit)
    {
        unitToUpgrade = unit;
        SetPricesText();
        SetButtonState();
        SetPosition();
    }

    private void SetButtonState()
    {
        upgradeButton.interactable = unitToUpgrade.GetComponent<Unit>().IsUpgradable();
    }

    private void SetPricesText()
    {
        Unit unitScript = unitToUpgrade.GetComponent<Unit>();
        upgradeText.text = "Upgrade - $" + unitScript.upgradeCost;
        sellText.text = "Sell - $" + unitScript.sellPrice;
    }

    // メニューがユニットの横に出るように位置を調整
    private void SetPosition()
    {
        transform.position = Camera.main.WorldToScreenPoint(unitToUpgrade.transform.position + new Vector3(3, 0));
    }

    public void OnClickUpgrade()
    {
        Unit unitScript = unitToUpgrade.GetComponent<Unit>();
        if (unitScript.IsUpgradable())
        {
            unitScript.Upgrade();
        }
        SetButtonState();
        SetPricesText();
    }

    public void OnClickSell()
    {
        GameManager.instance.Money += unitToUpgrade.GetComponent<Unit>().sellPrice;
        Destroy(unitToUpgrade);
        gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
