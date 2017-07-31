using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject target;
    private float speed = 7.0f;
    public float atk;

    private void Update()
    {
        // targetの座標に向かって移動
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        if (transform.position == target.transform.position)
        {
            Enemy enemyScript = target.GetComponent<Enemy>();
            enemyScript.TakeDamage(this.atk);
            Destroy(gameObject);
        }
    }
}