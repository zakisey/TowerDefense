using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float range = 2.5f;//のち消去
    private float atk = 1.0f;
    public GameObject rangeCircle;
    private GameObject hoverInstance;
    private CircleCollider2D myCollider;
    private float colliderRadius;

    // Use this for initialization
    void Start ()
    {
        myCollider = transform.GetComponent<CircleCollider2D>();
        colliderRadius = myCollider.radius;
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

    //マウスホバーで射程範囲を表示する
    void OnMouseEnter()
    {
            hoverInstance = (GameObject)Instantiate(rangeCircle, transform.position, Quaternion.identity);
            hoverInstance.transform.localScale = new Vector3(colliderRadius, colliderRadius);
    }

    void OnMouseExit()
    {
        Destroy(hoverInstance);
    }
}
