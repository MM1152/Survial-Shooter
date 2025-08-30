using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHp;
    public float moveSpeed;
    public float damage;
    public float traceRadious;
    public float attackRadious;

    public float spawnPosibility;
}
