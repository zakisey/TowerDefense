using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersBase : MonoBehaviour
{
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
        }
    }
    // Use this for initialization
    void Start()
    {
        HP = 1;
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
        }

        if (HP <= 0)
        {
            GameManager.instance.EndGame();
        }
    }
}
