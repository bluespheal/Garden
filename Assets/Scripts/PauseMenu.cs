using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _pauseMenu;
    private VisualElement _root;
    private Button _pause;
    private Button _quit;

    [SerializeField]
    private StyleSheet _styleSheet;
    [SerializeField]
    private VisualTreeAsset _visualTreeAssetItem;

    private void OnEnable()
    {
        _uIDocument = GetComponent<UIDocument>();
        _root = _uIDocument.rootVisualElement;
        _pauseMenu = _root.Q<VisualElement>("PauseMenu");
        _pause = _root.Q<Button>("Continue");
        _quit = _root.Q<Button>("Quit");
        
        if (_pause != null)
        {
            _pause.Focus();
            _pause.clickable.clicked += () =>
            {
                GameManager.Instance.ResumeTheGame();
            };
        }

        if (_quit != null)
        {
            _quit.clickable.clicked += () =>
            {
                GameManager.Instance.canTogglePause = false;
                GameObject.Find("Millet").GetComponent<PlayerMovement>().enabled = false;
                GameManager.Instance.SceneChanger.ChangeLevel("MainMenu");
            };
        }

    }

    //PopulateMenuItems();
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
