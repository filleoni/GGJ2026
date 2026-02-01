using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWave", menuName = "SO/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public float initialWait = 1;

    public List<GameObject> enemies;
}
