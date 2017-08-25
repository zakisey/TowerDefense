using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unitが発射する弾丸のクラス
public class Shot : MonoBehaviour
{
    public GameObject target;
    private float speed = 7.0f;
    public float atk;

    private void Update()
    {
        // targetが存在しなければ消滅
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        // targetの方へ移動
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        // targetの方へ向きを変える
        transform.rotation = Quaternion.FromToRotation(new Vector3(0, 1, 0), target.transform.position - transform.position);
        // targetにぶつかったらダメージを与えて消滅
        if (transform.position == target.transform.position)
        {
            Enemy enemyScript = target.GetComponent<Enemy>();
            enemyScript.TakeDamage(this.atk);
            Destroy(gameObject);
        }
    }
}