using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ShopMenu : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _root;
    private ScrollView _item_list_container;
    private Button first_item;
    [SerializeField]
    private List<InventoryItem> _shopItem_list;

    private Button _back;

    [SerializeField]
    private VisualTreeAsset _shopItemAsset;

    public PlayerInput playerInput;

    public Couscous couscous;

    public bool skip;
    private void Start()
    {
        _shopItem_list.Reverse();
        playerInput = GetComponent<PlayerInput>();
        GameManager.Instance.ResumeTheGame();
        GameManager.Instance.canTogglePause = true;

        _uIDocument = GetComponent<UIDocument>();
        _root = _uIDocument.rootVisualElement;
        _item_list_container = _root.Q<ScrollView>("ItemListContainer");

        GameManager.Instance.ForestUIManager.SetUIDocForMainMenu();

        _back = _root.Q<Button>("BackButton");

        SetupShopItems();

        first_item = (Button)_item_list_container.ElementAt(0);
        first_item.Focus();
    }

    void SetupShopItems()
    {
        foreach (InventoryItem inventoryItem in _shopItem_list)
        {
            TemplateContainer myUI = _shopItemAsset.Instantiate();

            Button _item = myUI.Q<Button>("Item");
            VisualElement _itemImage = _item.Q<VisualElement>("ItemImage");
            Label _itemName = _item.Q<Label>("ItemName");
            Label _itemNumber = _item.Q<Label>("ItemNumber");
            Label _itemPrice = _item.Q<Label>("ItemPrice");
            Label _itemDescription = _item.Q<Label>("ItemDescription");

            _itemImage.style.backgroundImage = new StyleBackground(inventoryItem.Sprite);
            _itemName.text = inventoryItem.Name;
            _itemNumber.text = "x" + inventoryItem.Amount.ToString();
            _itemPrice.text = inventoryItem.Price.ToString();
            _itemDescription.text = inventoryItem.Description;
            _item.focusable = true;
            _item.clickable.clicked += () =>
            {
                BuyItem(inventoryItem, _itemNumber);
            };
            _item_list_container.Insert(0, _item);
        }
    }

    void BuyItem(InventoryItem item, Label amount_label)
    {
        if(GameManager.Instance.currentInventory.Inventory.apples >= 3 && item.Name == "Golden Fruit")
        {
            couscous.LimitDialogue();
            return;
        }

        {
            if (GameManager.Instance.currentInventory.Inventory.beans < item.Price)
            {
                couscous.NotEnoughMoneyDialogue();
            }
            else
            {
                couscous.PurchaseDialogue();

                GameManager.Instance.currentInventory.SpendBeans(item.Price);
                GameManager.Instance.currentInventory.Inventory._items.Find(x => x.Name.Equals(item.Name)).Amount++;
                GameManager.Instance.ForestUIManager.SetUIDocForMainMenu();
                GameManager.Instance.currentInventory.Inventory.apples = GameManager.Instance.currentInventory.Inventory._items.Find(x => x.Name.Equals("Golden Fruit")).Amount;
                amount_label.text = "x" + GameManager.Instance.currentInventory.Inventory._items.Find(x => x.Name.Equals(item.Name)).Amount.ToString();
            }
        }

    }

    void OnReturn()
    {
        ChangeSceneFromMenu("MainMenu");
    }

    void ChangeSceneFromMenu(string scene)
    {
        GameManager.Instance.SceneChanger.ChangeLevel(scene);
    }

}
