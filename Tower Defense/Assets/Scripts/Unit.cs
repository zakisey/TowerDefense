using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // 各レベルでの射程距離や攻撃力などのスタッツ
    public float[] ranges;
    public float[] atks;
    public int initialCost;
    public int[] upgradeCosts;
    public int[] sellPrices;
    /// <summary>
    /// (coolTimeSec)秒ごとに弾を1発撃つ
    /// </summary>
    public float[] coolTimeSecs;
    public int maxLevel;
    public bool canAttackGround;
    public bool canAttackFloat;

    // Prefabs
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

    public GameObject NumberSprite;

    [System.NonSerialized]
    public float range;
    [System.NonSerialized]
    public float atk;
    [System.NonSerialized]
    public int upgradeCost;
    [System.NonSerialized]
    public int sellPrice;
    [System.NonSerialized]
    public float coolTimeSec;
    /// <summary>
    /// これがcoolTimeSec以上になると攻撃
    /// </summary>
    private float coolTimeChargedSec;
    private int level;
    private GameObject target;
    private Transform canon;
    private Vector2 canonVector;

    // Use this for initialization
    void Start()
    {
        level = 0;
        UpdateStats();

        //範囲
        rangeCircleCollider = Instantiate(rangeCircleCollider, transform.position, Quaternion.identity);
        rangeCircleCollider.transform.parent = this.gameObject.transform;

        //画像
        rangeCirclePicture = Instantiate(rangeCirclePicture, transform.position, Quaternion.identity);
        rangeCirclePicture.transform.parent = this.gameObject.transform;
        rangeCirclePicture.SetActive(false);

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

    public bool IsAtMaxLevel()
    {
        return level == maxLevel;
    }

    public bool IsUpgradable()
    {
        return level < maxLevel && upgradeCosts[level] <= GameManager.instance.Money;
    }

    public void Upgrade()
    {
        GameManager.instance.Money -= upgradeCosts[level];
        level++;
        UpdateStats();
    }

    // atkなどのスタッツをレベルにあわせて変更する。レベルをセットしたあとに呼ぶ
    private void UpdateStats()
    {
        range = ranges[level];
        atk = atks[level];
        // レベルが最大のときはアップグレードのコストは存在しない
        if (level < maxLevel) upgradeCost = upgradeCosts[level];
        sellPrice = sellPrices[level];
        coolTimeSec = coolTimeSecs[level];

        rangeCircleCollider.GetComponent<SphereCollider>().radius = range;
        coolTimeChargedSec = 0;

        NumberSprite.GetComponent<NumberSprite>().Number = level + 1;
    }

    // 射程距離内の一番近い敵を見つける
    private GameObject GetNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minimumDistance = float.MaxValue;
        foreach (GameObject enemy in enemies)
        {
            bool isFloat = enemy.GetComponent<Enemy>().IsFloat;
            // 敵が飛行で、このユニットが飛行を攻撃できない
            if (isFloat && !canAttackFloat) continue;
            // 敵が地上で、このユニットが地上を攻撃できない
            if (!isFloat && !canAttackGround) continue;
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

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == target)
        {
            target = null;
        }
    }

    // ターゲットしている敵を砲撃する
    private void Fire()
    {
        coolTimeChargedSec += Time.deltaTime;
        if (coolTimeChargedSec < coolTimeSec || target == null) return;
        GameObject shot = Instantiate(this.shot, this.transform.position, Quaternion.identity);
        Shot shotScript = shot.GetComponent<Shot>();
        shotScript.target = target;
        shotScript.atk = this.atk;
        StartCoroutine(PlaySound(cannonAudio));
        coolTimeChargedSec = 0f;
    }

    private IEnumerator PlaySound(AudioSource audioSource)
    {
        AudioSource audio = Instantiate(audioSource, this.transform);
        audio.Play();

        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(audio.gameObject);
    }

    private void OnMouseDown()
    {
        GameManager.instance.ShowUnitUpgradeMenu(gameObject);
    }

    //マウスホバーで射程範囲を表示する
    void OnMouseEnter()
    {
        rangeCirclePicture.SetActive(true);
        rangeCirclePicture.transform.localScale = new Vector3(range, range);
    }

    void OnMouseExit()
    {
        rangeCirclePicture.SetActive(false);
    }
}
