using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    private const int StageWidth = 12;
    private const int StageHeight = 8;
    private const float mas = 1f;

    /// <summary>
    /// 敵が進む目的地のリスト
    /// </summary>
    public List<int> position;
    private int nowNum = 0;
    private float goalX;
    private float goalY;

    public float damage = 1.0f;
    private float speed = 0.05f;
    private float hp;

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
        HP = 1.0f;
        TransPosition(position[nowNum], out goalX, out goalY);
    }

    // Update is called once per frame
    void Update()
    {
        Move(position);
        //this.transform.position = new Vector2(transform.position.x + speed, transform.position.y);
    }

    private void Move(List<int> pList)
    {
        if (Arrive(goalX, goalY) && nowNum < position.Count - 1)
        {
            nowNum++;
            TransPosition(position[nowNum], out goalX, out goalY);
        }
        Go(goalX, goalY);
    }

    /// <summary>
    /// positionの数字を(x,y)座標に変換する
    /// </summary>
    /// <param name="p_num">positionの数</param>
    /// <param name="px">x座標</param>
    /// <param name="py">y座標</param>
    void TransPosition(int p_num, out float px, out float py)
    {
        int p_numY = p_num / StageWidth;
        int p_numX = p_num - p_numY * StageWidth;
        px = p_numX * mas;
        py = p_numY * mas;
    }

    bool Arrive(float gx, float gy)
    {
        return gx == gameObject.transform.position.x && gy == gameObject.transform.position.y;
    }

    /// <summary>
    /// 速さspeedで目的地まで行く
    /// </summary>
    /// <param name="gx">目的地のx座標</param>
    /// <param name="gy">目的地のy座標</param>
    void Go(float gx, float gy)
    {
        float dx, dy;
        Distance(gx, gy, out dx, out dy);
        dx = CheckSpeed(dx);
        dy = CheckSpeed(dy);
        gameObject.transform.position += new Vector3(dx, dy);
    }

    /// <summary>
    /// 目的地とthis.gameObjectまでの距離を出す
    /// </summary>
    /// <param name="gx">目的地のx座標</param>
    /// <param name="gy">目的地のy座標</param>
    /// <param name="dx">目的地までのxの距離</param>
    /// <param name="dy">目的地までのyの距離</param>
    void Distance(float gx, float gy, out float dx, out float dy)
    {
        dx = gx - gameObject.transform.position.x;
        dy = gy - gameObject.transform.position.y;
    }

    /// <summary>
    /// 距離から速さをいくらにするか決める
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    float CheckSpeed(float distance)
    {
        if (speed >= Math.Abs(distance))
        {
            return distance;
        }
        else
        {
            return (distance > 0) ? speed : -speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Base")
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        this.HP -= damage;
    }
}
