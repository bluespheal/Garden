using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Instance
    static GameManager instance;

    [SerializeField] public Inventory inventory;

    [SerializeField] public bool paused;

    //ForestUI
    [SerializeField] ForestUIManager forestUI;

    private void Awake()
    {
        //Si ya existe otro Game Manager se autodestruye
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //Se referencia a si mismo
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        if (SceneManager.GetActiveScene().name == "Garden")
        {
            forestUI = GameObject.Find("---UI---").GetComponent<ForestUIManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBeans(bool adding, int number)
    {
        if (adding)
        {
            inventory.AddBeans(number);
        }
        else
        {
            inventory.SpendBeans(number);
        }

        forestUI.UpdateBeanLabel();

    }

    [RuntimeInitializeOnLoadMethod]
    static void Autogenerate()
    {
        GameObject go = new GameObject("GameManager");
        instance = go.AddComponent<GameManager>();
        go.AddComponent<Inventory>();
        
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
}
