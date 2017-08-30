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
        Vector2 menuSize = GetComponent<RectTransform>().sizeDelta;
        Vector2 canvasSize = new Vector2(800f, 600f);

        Vector3 menuPos = Camera.main.WorldToViewportPoint(unitToUpgrade.transform.position);
        // 15f:調整のための数値
        float xOffset = (menuSize.x / 2 + 15f) / canvasSize.x;

        // 基本的にはユニットの右にmenuを表示
        // menuがゲーム画面を出てしまうとき
        if (menuPos.x + xOffset + menuSize.x / canvasSize.x > 1)
        {
            //ユニットの左に表示する
            menuPos += new Vector3(-xOffset, 0, 0);
        }
        else
        {
            menuPos += new Vector3(xOffset, 0, 0);
        }

        //下にはみ出した場合もチェック
        if (menuPos.y - menuSize.y / canvasSize.y / 2 < 0)
        {
            menuPos = new Vector3(menuPos.x, menuSize.y / canvasSize.y / 2, 0);
        }

        // 調整したviewport座標をキャンバスの座標に変換
        transform.position = new Vector2(
            (menuPos.x * Camera.main.pixelWidth),
            (menuPos.y * Camera.main.pixelHeight));
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
