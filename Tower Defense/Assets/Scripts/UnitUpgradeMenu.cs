using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ButtonMouseHoverに必要なので
//継承がMonoBehaviour->Selectableに変更
public class UnitUpgradeMenu : Selectable
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
        sellText.text = "Sell\n + $" + unitScript.sellPrice;
    }

    // メニューがユニットの横に出るように位置を調整
    private void SetPosition()
    {
        float menuWidth = GetComponent<RectTransform>().sizeDelta.x;
        float canvasWidth = 800;
        // 15f:調整のための数値
        float xOffset = (menuWidth / 2 + 15f) / canvasWidth;

        Vector3 unitPos = Camera.main.WorldToViewportPoint(unitToUpgrade.transform.position);

        // 基本的にはユニットの右にmenuを表示
        // menuがゲーム画面を出てしまうとき（縦方向は現在考慮しない）
        if (unitPos.x + xOffset + menuWidth / canvasWidth > 1)
        {
            //ユニットの左に表示する
            unitPos += new Vector3(-xOffset, 0, 0);
        }
        else
        {
            unitPos += new Vector3(xOffset, 0, 0);
        }

        // 調整したviewport座標をキャンバスの座標に変換
        transform.position = new Vector2(
            (unitPos.x * Camera.main.pixelWidth),
            (unitPos.y * Camera.main.pixelHeight));
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

    /// <summary>
    /// ボタンがマウスホバーされているかどうか
    /// </summary>
    /// <returns></returns>
    public bool ButtonMouseHover()
    {
        if (currentSelectionState == SelectionState.Highlighted) return true;
        return false;
    }

}
