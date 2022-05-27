using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ShopMenu : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _mainMenu;
    private VisualElement _root;
    private ScrollView _item_list_container;
    private Button first_item;
    [SerializeField]
    private List<Button> _item_list;
    [SerializeField]
    private List<ShopItem> _shopItem_list;

    private Button _back;

    [SerializeField]
    private StyleSheet _styleSheet;
    [SerializeField]
    private VisualTreeAsset _visualTreeAssetItem;

    public PlayerInput playerInput;

    public bool side;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        GameManager.Instance.ResumeTheGame();
        GameManager.Instance.canTogglePause = true;

        _uIDocument = GetComponent<UIDocument>();
        _root = _uIDocument.rootVisualElement;
        _item_list_container = _root.Q<ScrollView>("ItemListContainer");


        _back = _root.Q<Button>("BackButton");

        foreach (ShopItem shopItem in _shopItem_list)
        {
            print(shopItem._name);
            TemplateContainer myUI = _visualTreeAssetItem.Instantiate();
            Button _item = myUI.Q<Button>("Item");
            VisualElement _itemImage = _item.Q<VisualElement>("ItemImage");
            Label _itemName = _item.Q<Label>("ItemName");
            Label _itemNumber = _item.Q<Label>("ItemNumber");
            Label _itemPrice = _item.Q<Label>("ItemPrice");
            Label _itemDescription = _item.Q<Label>("ItemDescription");

            _itemImage.style.backgroundImage = new StyleBackground(shopItem._sprite);
            _itemName.text = shopItem._name;
            _itemNumber.text = "x0";
            _itemPrice.text = shopItem._price;
            _itemDescription.text = shopItem._description;
            _item.focusable = true;

            _item_list_container.Insert(0, _item);
        }

        GameManager.Instance.ForestUIManager.SetUIDocForMainMenu();

        first_item = (Button)_item_list_container.ElementAt(0);

        if (first_item != null)
        {
            first_item.Focus();
            first_item.clickable.clicked += () =>
            {
                ChangeSceneFromMenu("Garden");
            };
        }

    }

    void OnSwitchSide(InputValue value)
    {
        if (!side)
        {
            side = !side;
            _back.Focus();
        }
        else
        {
            first_item.Focus();
            side = !side;
        }
    }

    void ChangeSceneFromMenu(string scene)
    {
        GameManager.Instance.SceneChanger.ChangeLevel(scene);
    }

    private void OnEnable()
    {
    }


}
