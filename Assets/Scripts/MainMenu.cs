using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _mainMenu;
    private VisualElement _root;
    private Button _garden;
    private Button _forest;
    private Button _house;
    private Button _shop;
    private Button _help;
    private Button _credits;
    private Button _back;

    [SerializeField]
    private StyleSheet _styleSheet;
    [SerializeField]
    private VisualTreeAsset _visualTreeAssetItem;

    public PlayerInput playerInput;

    public bool side;
    private void Start()
    {
        GameManager.Instance.AudioManager.PlaySong(0);

        playerInput = GetComponent<PlayerInput>();

        GameManager.Instance.ResumeTheGame();
        GameManager.Instance.canTogglePause = true;
        _uIDocument = GetComponent<UIDocument>();
        _root = _uIDocument.rootVisualElement;
        _mainMenu = _root.Q<VisualElement>("MainMenu");

        _garden = _root.Q<Button>("Garden");
        _forest = _root.Q<Button>("Forest");
        _house = _root.Q<Button>("House");
        _shop = _root.Q<Button>("Shop");
        _help = _root.Q<Button>("Help");
        _credits = _root.Q<Button>("Credits");

        _back = _root.Q<Button>("BackButton");

        GameManager.Instance.ForestUIManager.SetUIDocForMainMenu();

        if (_forest != null)
        {
            _forest.Focus();

            _forest.clickable.clicked += () =>
            {
                ChangeSceneFromMenu("Forest");
            };
        }

        if (_shop != null)
        {
            _shop.clickable.clicked += () =>
            {
                ChangeSceneFromMenu("Shop");
            };
        }

        if (_credits != null)
        {
            _credits.clickable.clicked += () =>
            {
                ChangeSceneFromMenu("Credits");
            };
        }
    }
    void OnCancel()
    {
        ChangeSceneFromMenu("TitleScreen");
    }
    void ChangeSceneFromMenu(string scene)
    {
        GameManager.Instance.SceneChanger.ChangeLevel(scene);
    }

    private void OnEnable()
    {
    }


}
