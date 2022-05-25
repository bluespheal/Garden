using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Instance
    public static GameManager Instance { get; private set; }
    public Inventory Inventory { get; private set; }
    //ForestUI
    public ForestUIManager ForestUIManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public SceneChanger SceneChanger { get; private set; }

    [SerializeField] public bool paused;
    [SerializeField] public bool canTogglePause;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        Inventory = GetComponentInChildren<Inventory>();
        AudioManager = GetComponentInChildren<AudioManager>();
        ForestUIManager = GetComponentInChildren<ForestUIManager>();
        SceneChanger = GetComponentInChildren<SceneChanger>();

        DontDestroyOnLoad(gameObject);

    }

    public void SetBeans(bool adding, int number)
    {
        if (adding)
        {
            Inventory.AddBeans(number);
        }
        else
        {
            Inventory.SpendBeans(number);
        }

        ForestUIManager.UpdateBeanLabel();

    }

    public void TogglePause()
    {
        if (!canTogglePause)
            return;
        if (paused) 
        {
            ResumeTheGame();
        }
        else
        {
            PauseTheGame();
        }
    }
    public void PauseTheGame()
    {
        paused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        ForestUIManager.ShowPauseMenu();

    }

    public void ResumeTheGame()
    {
        paused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        ForestUIManager.HidePauseMenu();
    }

    //[RuntimeInitializeOnLoadMethod]
    //static void Autogenerate()
    //{
    //    GameObject go = new GameObject("GameManager");
    //    Instance = go.AddComponent<GameManager>();        
    //}

    //public static GameManager Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}
}
