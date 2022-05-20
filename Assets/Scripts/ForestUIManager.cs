using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ForestUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    private Label beanLabel;
    private Label enemyLabel;
    [SerializeField] public int pop;

    private string beanText;
    private void Awake()
    {
        var rootElement = uiDoc.rootVisualElement;
        beanLabel = rootElement.Q<Label>("bean_text");
        enemyLabel = rootElement.Q<Label>("enemy_text");
    }
    public void UpdateBeanLabel()
    {
        beanText = GameManager.Instance.inventory.GetBeans().ToString();
        while (beanText.Length < 6)
        {
            beanText = beanText.Insert(0, "0");
        }
        beanLabel.text = beanText;
    }
}
