using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    public PlayMakerFSM enemyspawnerSFM;
    public ItemSpawner itemSpawner;
    public int enemiesDefeated;


    [Header("Current Levels")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private int minEnemyIndex;
    [SerializeField] private int maxEnemyIndex;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AudioManager.PlaySong(1);
        GameManager.Instance.ForestUIManager.SetUIDocForForest();

        GameManager.Instance.ForestUIManager.SetHearts();

        if (!itemSpawner)
            itemSpawner = GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>();
    }

    public void IncreaseEnemyCount()
    {
        enemiesDefeated++;
        GameManager.Instance.ForestUIManager.UpdateEnemyLabel(enemiesDefeated);
        RecalculateStats();
        if( enemiesDefeated % 20 == 0)
            itemSpawner.SpawnItem();
    }

    public void RecalculateStats()
    {
        if (enemiesDefeated < 10)
        {
            RecalculateEnemyLevels(0, 0);
            RecalculateSpawnTimer(5, 9);
            UpdateEnemyLevels();
            return;
        }
        else if (enemiesDefeated < 30) 
        {
            RecalculateEnemyLevels(0, 1);
            RecalculateSpawnTimer(5, 9);
            UpdateEnemyLevels();
            return;
        }
        else if (enemiesDefeated < 60)
        {
            RecalculateEnemyLevels(0, 2);
            RecalculateSpawnTimer(4, 8);
            UpdateEnemyLevels();
            return;
        }
        else if (enemiesDefeated < 100)
        {
            RecalculateEnemyLevels(0, 3);
            RecalculateSpawnTimer(4, 8);
            UpdateEnemyLevels();
            return;
        }
        else if (enemiesDefeated < 150)
        {
            RecalculateEnemyLevels(0, 3);
            RecalculateSpawnTimer(4, 7);
            UpdateEnemyLevels();
            return;
        }
        else if (enemiesDefeated < 200)
        {
            RecalculateEnemyLevels(1, 3);
            RecalculateSpawnTimer(3, 7);
            UpdateEnemyLevels();
            return;
        }
        else if (enemiesDefeated < 300)
        {
            RecalculateEnemyLevels(1, 3);
            RecalculateSpawnTimer(2, 6);
            UpdateEnemyLevels();
            return;
        }

    }

    public void RecalculateEnemyLevels(int minLevel, int maxLevel)
    {
        minEnemyIndex = minLevel;
        maxEnemyIndex = maxLevel;
    }

    public void RecalculateSpawnTimer(float minTime, float maxTime)
    {
        minWaitTime = minTime;
        maxWaitTime = maxTime;
    }


    public void UpdateEnemyLevels()
    {
        //Enemy Levels
        enemyspawnerSFM.FsmVariables.GetFsmInt("EnemyMinLevel").Value = minEnemyIndex;
        enemyspawnerSFM.FsmVariables.GetFsmInt("EnemyMaxLevel").Value = maxEnemyIndex;

        //Wait times
        enemyspawnerSFM.FsmVariables.GetFsmFloat("MinWaitTime").Value = minWaitTime;
        enemyspawnerSFM.FsmVariables.GetFsmFloat("MaxWaitTime").Value = maxWaitTime;

    }

}
