using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVarSetter : MonoBehaviour
{
    public PlayMakerFSM enemySFM;
    public float veggie_power;
    public GameObject player;

    public GameplayManager gameplayManager;
    
    

    [Header("EnemyStats")]
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float beanNumber;

    [Header("Multipliers")]
    [SerializeField] private float healthMultiplier;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float beanMultiplier;

    [Header("Current Stats")]
    [SerializeField] private float enemyCount;
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthAdd;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float speedAdd;
    [SerializeField] private int currentBean;
    [SerializeField] private float beanAdd;


    void Start()
    {
        player = GameObject.Find("Millet");
        enemySFM = gameObject.GetComponent<PlayMakerFSM>();
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        veggie_power = GameObject.Find("Veggie").GetComponent<Veggie>().veggie_power;
        SetEnemyStats();
    }
    private void SetMultipliers()
    {
        healthAdd = enemyCount * healthMultiplier;
        speedAdd = enemyCount * speedMultiplier;
        beanAdd = enemyCount * beanMultiplier;
    }

    public void SetEnemyStats()
    {
        enemyCount = GetEnemyCount();
        SetMultipliers();

        enemySFM.FsmVariables.GetFsmFloat("Veggie_Power").Value = veggie_power;
        enemySFM.FsmVariables.GetFsmGameObject("Player").Value = player;



        //Enemy stats in relation to enemies defeated.
        currentHealth = baseHealth + healthAdd;
        currentSpeed = baseSpeed + speedAdd;
        currentBean = (int)(beanNumber + healthAdd);

        enemySFM.FsmVariables.GetFsmInt("BeanNumber").Value = currentBean;
        enemySFM.FsmVariables.GetFsmFloat("BaseSpeed").Value = currentSpeed;
        enemySFM.FsmVariables.GetFsmFloat("EnemyHealth").Value = currentHealth;
    }


    private int GetEnemyCount()
    {
        return gameplayManager.enemiesDefeated;
    }
    public void AddEnemyCount()
    {
        gameplayManager.IncreaseEnemyCount();
    }

    
}
