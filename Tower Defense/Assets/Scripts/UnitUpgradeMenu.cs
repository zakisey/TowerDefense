using System.Collections;
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
        gameObject.SetActive(true);
        unitToUpgrade = unit;
        SetPricesText();
        SetButtonState();
        SetPosition();
    }

    // 実際の表示位置と違う位置に表示されるバグがあったので、これを書いたら直った…
    private void Update()
    {
        transform.position = transform.position;
        SetButtonState();
    }

    private void SetButtonState()
    {
        upgradeButton.interactable = unitToUpgrade.GetComponent<Unit>().IsUpgradable();
    }

    private void SetPricesText()
    {
        Unit unitScript = unitToUpgrade.GetComponent<Unit>();
        if (unitScript.IsAtMaxLevel())
        {
            upgradeText.text = "Max\nLevel!";
        }
        else
        {
            upgradeText.text = "Upgrade\n - $" + unitScript.upgradeCost;
        }
        sellText.text = "Sell\n - $" + unitScript.sellPrice;
    }

    // メニューがユニットの横に出るように位置を調整
    private void SetPosition()
    {
        Vector3 unitPos = Camera.main.WorldToScreenPoint(unitToUpgrade.transform.position);
        Rect menuSize = this.gameObject.GetComponent<RectTransform>().rect;
        float screenWidth = Camera.main.pixelWidth;

        // カメラのサイズが違う時に自動で調整する(Stage 1のサイズ9.0fを基準とする)
        float cameraScale = Camera.main.orthographicSize / 9.0f;
        float xOffset = 2.5f * cameraScale;

        if (menuSize.width > (screenWidth - unitPos.x)) // 右に寄りすぎる時（縦方向は現在考慮しない）
        {
            transform.position = Camera.main.WorldToScreenPoint(unitToUpgrade.transform.position + new Vector3(-xOffset, 0));
        }
        else
        {
            transform.position = Camera.main.WorldToScreenPoint(unitToUpgrade.transform.position + new Vector3(xOffset, 0));
        }
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
