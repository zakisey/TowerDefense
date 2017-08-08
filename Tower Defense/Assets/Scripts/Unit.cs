using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float range = 3.0f;
    public float atk = 1.0f;
    public int cost = 10;
    /// <summary>
    /// (atkTime)フレームごとに弾を1発撃つ
    /// </summary>
    public int atkTime = 60;
    /// <summary>
    /// これがatkTimeと同じ数になると攻撃
    /// </summary>
    private int chargeTime;

    public GameObject shot;
    /// <summary>
    /// 攻撃範囲用
    /// </summary>
    public GameObject rangeCircleCollider;
    /// <summary>
    /// 画像用
    /// </summary>
    public GameObject rangeCirclePicture;

    /// <summary>
    /// 効果音用
    /// </summary>
    public AudioSource cannonAudio;

    //private GameObject hoverInstance;
    //private CircleCollider2D myCollider;


    private float colliderRadius;
    private GameObject target;
    private Transform canon;
    private Vector2 canonVector;

    // Use this for initialization
    void Start()
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

        target = null;
        canon = this.transform.Find("Canon");
        canonVector = new Vector2(0, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            target = GetNearestTarget();
        }
        RotateCanon();
        Fire();
    }

    // 射程距離内の一番近い敵を見つける
    private GameObject GetNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minimumDistance = float.MaxValue;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(this.transform.position, enemy.transform.position);
            if (distance < minimumDistance)
            {
                nearestEnemy = enemy;
                minimumDistance = distance;
            }
        }
        return (minimumDistance <= this.range) ? nearestEnemy : null;
    }

    // 砲身がターゲットしている敵の方へ向くように回転する
    private void RotateCanon()
    {
        if (target == null) return;
        Vector2 newCanonVector = target.transform.position - this.transform.position;
        float angle = Vector2.Angle(canonVector, newCanonVector);
        // 直積を使って角度の正負を判定する
        var cross = Vector3.Cross(canonVector, newCanonVector);
        if (cross.z < 0) angle = -angle;
        canonVector = newCanonVector;
        canon.RotateAround(this.transform.position, new Vector3(0, 0, 1), angle);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            target = null;
        }
    }

    // ターゲットしている敵を砲撃する
    private void Fire()
    {
        if (chargeTime++ < atkTime || target == null) return;
        GameObject shot = Instantiate(this.shot, this.transform.position, Quaternion.identity);
        Shot shotScript = shot.GetComponent<Shot>();
        shotScript.target = target;
        shotScript.atk = this.atk;
        StartCoroutine(PlaySound(cannonAudio));
        chargeTime = 0;
    }

    private IEnumerator PlaySound(AudioSource audioSource)
    {
        AudioSource audio = Instantiate(audioSource, this.transform);
        audio.Play();

        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(audio.gameObject);
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
