using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage = 1.0f;
    private float speed = 0.05f;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rigidBody;
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
	void Start ()
    {
        /*
        //ここはプレハブに移譲
        circleCollider = GetComponent<CircleCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        */
        HP = 1.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector2(transform.position.x + speed, transform.position.y);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Base")
        {
            Destroy(this.gameObject);
        }

       
    }
}
