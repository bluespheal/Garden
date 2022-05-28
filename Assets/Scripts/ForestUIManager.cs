using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ForestUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    [SerializeField] private VisualTreeAsset _heart;
    [SerializeField] private VisualTreeAsset _heartFill;
    [SerializeField] private GameObject pauseMenu;
    

    private Label beanLabel;
    private Label enemyLabel;   

    VisualElement heartBar;

    private string beanText;
    private string enemyText;

    private int hearts;
    private void Awake()
    {
        uiDoc = GameObject.Find("MainUI").GetComponent<UIDocument>();
        var rootElement = uiDoc.rootVisualElement;
        beanLabel = rootElement.Q<Label>("bean_text");
        enemyLabel = rootElement.Q<Label>("enemy_text");
        heartBar = rootElement.Q<VisualElement>("hp_bar");
    }

    public void SetUIDocForForest()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
        uiDoc = GameObject.Find("MainUI").GetComponent<UIDocument>();
        var rootElement = uiDoc.rootVisualElement;
        beanLabel = rootElement.Q<Label>("bean_text");
        enemyLabel = rootElement.Q<Label>("enemy_text");
        heartBar = rootElement.Q<VisualElement>("hp_bar");
        UpdateBeanLabel();
    }

    public void SetUIDocForMainMenu()
    {
        uiDoc = GameObject.Find("MainUI").GetComponent<UIDocument>();
        var rootElement = uiDoc.rootVisualElement;
        beanLabel = rootElement.Q<Label>("bean_text");
        UpdateBeanLabel();
    }

    public void SetHearts()
    {
        hearts = 2 + GameManager.Instance.currentInventory.Inventory.Apples;
        AddHearts();
    }

    public void AddHearts()
    {
        for (int i = 0; i < hearts; i++)
        {
            heartBar.Insert(0, _heart.Instantiate());  
        }
    }

    public void UpdateBeanLabel()
    {
        beanText = GameManager.Instance.currentInventory.Inventory.Beans.ToString();
        while (beanText.Length < 5)
        {
            beanText = beanText.Insert(0, "0");
        }
        beanLabel.text = beanText;
    }

    public void UpdateEnemyLabel(int enemies_defeated)
    {
        enemyText = enemies_defeated.ToString();
        while (enemyText.Length < 5)
        {
            enemyText = enemyText.Insert(0, "0");
        }
        enemyLabel.text = enemyText;
    }

    public void UpdateHeartBar(int current_hearts, bool damage)
    {
        if (current_hearts <= 0)
            return;

        if (damage)
        {
            VisualElement som = heartBar.ElementAt(current_hearts - 1);
            som.ElementAt(0).Clear();
        }
        else
        {
            VisualElement som = heartBar.ElementAt(current_hearts - 1);
            _heartFill.CloneTree(som.ElementAt(0));
        }
        
    }
    public void ShowPauseMenu()
    {
        if (pauseMenu)
            pauseMenu.SetActive(true);

    }

    public void HidePauseMenu()
    {
        if (pauseMenu)
            pauseMenu.SetActive(false);
    }


}
