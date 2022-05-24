using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ForestUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    [SerializeField] private VisualTreeAsset _heart;
    [SerializeField] private VisualTreeAsset _heartFill;

    private Label beanLabel;
    private Label enemyLabel;   

    VisualElement heartBar;

    private string beanText;
    private string enemyText;

    private int hearts;
    private void Awake()
    {
        var rootElement = uiDoc.rootVisualElement;
        beanLabel = rootElement.Q<Label>("bean_text");
        enemyLabel = rootElement.Q<Label>("enemy_text");
        heartBar = rootElement.Q<VisualElement>("hp_bar");
    }

    private void Start()
    {
        SetHearts();
    }

    void SetHearts()
    {
        hearts = 2 + GameManager.Instance.Inventory.GetApples();
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
        beanText = GameManager.Instance.Inventory.GetBeans().ToString();
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


}
