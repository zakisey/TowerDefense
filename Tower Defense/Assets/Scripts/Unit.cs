using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float range = 2.5f;//のち消去
    private float atk = 1.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        //コライダーに移譲
        GameObject enemy = BoardManager.instance.SearchNearestEnemy();
        if (enemy == null)
        {
            return;
        }
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (Vector2.Distance(this.transform.position, enemy.transform.position) <= this.range)
        {
            enemyScript.HP -= this.atk;
        }
        */
	}


    //コライダーの関数を入れる
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().HP -= this.atk;
        }

        
    }
}
