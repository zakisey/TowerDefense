using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            this.hp = value;
            BoardManager.instance.SetLifeText((int)hp);
            if (hp <= 0) GameManager.instance.EndGame();
        }
    }
    // Use this for initialization
    void Start()
    {
        HP = 20;
        HpBar = Instantiate(HpBar, transform.position + new Vector3(0, -0.53f), Quaternion.identity, GameObject.Find("Canvas").transform);
        HpBar.maxValue = HP;
        HpBar.value = HP;
        RectTransform hpBarRect = HpBar.GetComponent<RectTransform>();
        hpBarRect.sizeDelta = new Vector2(80, hpBarRect.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.tag == "Enemy")
        {
            HP -= enemy.atk;
            HpBar.value = HP;
        }

        if (HP <= 0)
        {
            GameManager.instance.EndGame();
        }
    }

    private void OnDestroy()
    {
        if (HpBar != null) Destroy(HpBar.gameObject);
    }
}
