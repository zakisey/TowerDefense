using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float range = 3.0f;
    public float atk = 1.0f;
    /// <summary>
    /// (atkTime)フレームごとに弾を1発撃つ
    /// </summary>
    public int atkTime = 60;
    /// <summary>
    /// これがatkTimeと同じ数になると攻撃
    /// </summary>
    private int chargeTime;

    public GameObject shot;
    private bool shotFlag = false;
    /// <summary>
    /// 攻撃範囲用
    /// </summary>
    public GameObject rangeCircleCollider;
    /// <summary>
    /// 画像用
    /// </summary>
    public GameObject rangeCirclePicture;

    //private GameObject hoverInstance;
    //private CircleCollider2D myCollider;

   
    private float colliderRadius;

    // Use this for initialization
    void Start ()
    {
        //範囲
        rangeCircleCollider = Instantiate(rangeCircleCollider, transform.position, Quaternion.identity);
        rangeCircleCollider.transform.parent = this.gameObject.transform;
        rangeCircleCollider.GetComponent<CircleCollider2D>().radius = range;

        //画像
        rangeCirclePicture = Instantiate(rangeCirclePicture, transform.position, Quaternion.identity);
        rangeCirclePicture.transform.parent = this.gameObject.transform;
        rangeCirclePicture.transform.localScale = new Vector3(range, range);
        rangeCirclePicture.SetActive(false);

        //弾
        chargeTime = atkTime;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Charge();
	}

    /// <summary>
    /// chargeTimeを増やす
    /// </summary>
    void Charge()
    {
        if (chargeTime < atkTime)
        {
            chargeTime++;
        }


    }

    //コライダーの関数を入れる
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Fire(collision.gameObject);
        }
    }

    private void Fire(GameObject target)
    {
        if (chargeTime<atkTime) return;
        GameObject shot = Instantiate(this.shot, this.transform.position, Quaternion.identity);
        Shot shotScript = shot.GetComponent<Shot>();
        shotScript.target = target;
        shotScript.atk = this.atk;
        chargeTime = 0;
    }

    //マウスホバーで射程範囲を表示する
    void OnMouseEnter()
    {
        rangeCirclePicture.SetActive(true);
    }

    void OnMouseExit()
    {
        rangeCirclePicture.SetActive(false);
    }
}
