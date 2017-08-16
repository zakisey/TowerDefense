using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// プレイヤーの基地のクラス
public class PlayersBase : MonoBehaviour
{
    /// <summary>
    /// HPバー描画用
    /// </summary>
    public Slider HpBar;
    private float hp;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            HpBar.value = hp;
            BoardManager.instance.SetLifeText((int)hp);
            if (hp <= 0)
            {
                GameManager.instance.EndGame();
            }
        }
    }

    void Start()
    {
        HP = 10;
        Vector3 hpBarPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.6f));
        HpBar = Instantiate(HpBar, hpBarPos, Quaternion.identity, GameObject.Find("Canvas").transform);
        HpBar.maxValue = HP;
        HpBar.value = HP;
        RectTransform hpBarRect = HpBar.GetComponent<RectTransform>();
        hpBarRect.sizeDelta = new Vector2(80, hpBarRect.sizeDelta.y);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
    }
}
