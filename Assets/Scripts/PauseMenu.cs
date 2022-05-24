using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _pauseMenu;
    private VisualElement _root;

    [SerializeField]
    private StyleSheet _styleSheet;
    [SerializeField]
    private VisualTreeAsset _visualTreeAssetItem;

    private void Awake()
    {
        _uIDocument = GetComponent<UIDocument>();
        _root = _uIDocument.rootVisualElement;
        _pauseMenu = _root.Q<VisualElement>("PauseMenu");

        PopulateMenuItems();
    }

    private void PopulateMenuItems()
    {
        for (int i = 0; i < 5; i++)
        {
            //VisualElement itemSlot = new VisualElement();
            //itemSlot.style.width = 100;
            //itemSlot.style.height = 100;
            //itemSlot.style.backgroundColor = Color.red;
            //itemSlot.AddToClassList("furnitureClass");
            //itemSlot.styleSheets.Add(_styleSheet);

            //_pauseMenu.Add(itemSlot);
        }

        //if (GameManager.Instance.inventory.GetItemsCount > 0)
        //{
        //    GameManager.Instance.inventory.FurnitureList.ForEach(item =>
        //    {
        //        VisualElement itemSlot = new VisualElement();
        //        itemSlot.style.width = 100;
        //        itemSlot.style.height = 100;
        //        itemSlot.style.backgroundColor = Color.red;
        //        //itemSlot.AddToClassList("furnitureClass");
        //        //itemSlot.styleSheets.Add(_styleSheet);

        //        _pauseMenu.Add(itemSlot);
        //    });
        //}
    }
}
