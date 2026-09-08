using UnityEngine;

[CreateAssetMenu(fileName = "CombatLevelData", menuName = "Scriptable Objects/CombatLevelData")]
public class CombatLevelData : ScriptableObject
{
    [System.Serializable]
    public class WaveData
    {
        public GameObject[] enemyPrefabs;
    }

    public WaveData[] waves;
    public int level;
}
