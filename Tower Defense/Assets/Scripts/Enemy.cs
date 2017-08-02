using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    private const int StageWidth = 15;
    private const int StageHeight = 12;
    private const float mas = 1f;

    /// <summary>
    /// 敵が進む目的地のリスト
    /// </summary>
    public List<Vector2> destinationList;
    /// <summary>
    /// 敵が向かっている目的地の場所のリスト番号
    /// </summary>
    private int destinationNumber = 0;


    public float atk = 1.0f;
    public float speed = 0.05f;
    public float hp;

    /// <summary>
    /// HPバー描画用
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
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        HP = 2.0f;
        HpBar = Instantiate(HpBar, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        HpBar.maxValue = HP;
        HpBar.value = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Move(destinationList);
        HpBar.transform.position = this.transform.position + new Vector3(0,-0.3f);
    }

    private void Move(List<Vector2> dList)
    {
        if (Arrive(dList[destinationNumber]) && destinationNumber < destinationList.Count - 1)
        {
            destinationNumber++;
        }
        Go(dList[destinationNumber]);
    }

    /// <summary>
    /// 目的地に着いたかどうか
    /// </summary>
    /// <param name="goal"></param>
    /// <returns></returns>
    bool Arrive(Vector2 goal)
    {
        return goal == (Vector2)gameObject.transform.position;
    }

    /// <summary>
    /// 目的地に向かって敵を進ませる
    /// </summary>
    /// <param name="goal">目的地の座標</param>
    void Go(Vector2 goal)
    {
        Vector2 vec　= Distance(goal);
        vec = CheckSpeed(vec);
        gameObject.transform.position += (Vector3)vec;
        
    }

    /// <summary>
    /// 目的地とthis.gameObjectまでの距離を出す
    /// </summary>
    /// <param name="gx">目的地のx座標</param>
    /// <param name="gy">目的地のy座標</param>
    /// <param name="dx">目的地までのxの距離</param>
    /// <param name="dy">目的地までのyの距離</param>
    Vector2 Distance(Vector2 goal)
    {
        return goal - (Vector2)gameObject.transform.position;
    }

    /// <summary>
    /// 距離をもとに移動距離をいくらにするか決める
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    Vector2 CheckSpeed(Vector2 distance)
    {
        if (speed >= distance.magnitude)
        {
            return distance;
        }
        else
        {
            return speed * distance.normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Base")
        {
            HP = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        this.HP -= damage;
        HpBar.value = HP;
    }
    private void OnDestroy()
    {
        Destroy(HpBar.gameObject);
    }
}
