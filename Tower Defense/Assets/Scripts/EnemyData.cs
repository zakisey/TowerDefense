
using System.Collections.Generic;
using UnityEngine;

// Enemyのデータ(攻撃力など)を格納しておくためのクラス 
[System.Serializable]
public class EnemyData : System.Object
{
    /// <summary>
    /// 敵が進む目的地の座標のリスト。最初の要素がスポーン位置で最後の要素が基地の位置
    /// </summary>
    public List<Vector2> path;
    public float atk;
    public float speed;
    public float hp;
    public int money;
}
