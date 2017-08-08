using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float hp;
    private float atk;
    private float speed;
    private int money;
    /// <summary>
    /// 敵が進む目的地の座標のリスト。最初の要素がスポーン位置で最後の要素が基地の位置
    /// </summary>
    private List<Vector2> path = null;
    /// <summary>
    /// 敵が向かっている目的地の場所のリスト番号
    /// </summary>
    private int destinationNumber = 0;
    /// <summary>
    /// HPバーのPrefab
    /// </summary>
    public Slider HpBar;

    public float HP
    {
        get
        {
            return this.hp;
        }
        set
        {
            hp = value;
            HpBar.value = hp;
            if (hp <= 0)
            {
                GameManager.instance.Money += money;
                Destroy(gameObject);
            }
        }
    }

    public void Init(float atk, float speed, float hp, int money, List<Vector2> path)
    {
        HP = hp;
        this.atk = atk;
        this.speed = speed;
        this.money = money;
        this.path = path;
        HpBar = Instantiate(HpBar, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        SetHpBarPos();
        HpBar.maxValue = HP;
        HpBar.value = HP;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path != null)
        {
            Move();
            SetHpBarPos();
        }
    }

    // 自分(敵ユニット)の位置にあわせてHPバーの位置を移動させる
    private void SetHpBarPos()
    {
        // HPバーはUI要素なのでWorld座標からScreen座標に変換する
        HpBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, -0.3f, 0));
    }

    private void Move()
    {
        // 次の目的地(曲がり角)に到着したら、目的地の番号を更新して方向転換
        if (transform.position == (Vector3)path[destinationNumber] && destinationNumber < path.Count - 1)
        {
            destinationNumber++;
            transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), (Vector3)path[destinationNumber] - transform.position);
        }
        // 目的地の方向へ進む
        transform.position = Vector3.MoveTowards(transform.position, path[destinationNumber], speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Base")
        {
            collision.gameObject.GetComponent<PlayersBase>().TakeDamage(atk);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        this.HP -= damage;
    }

    private void OnDestroy()
    {
        if (HpBar != null) Destroy(HpBar.gameObject);
    }
}
