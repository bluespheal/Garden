using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _mainMenu;
    private VisualElement _root;
    

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
        _mainMenu = _root.Q<VisualElement>("TitleScreen");


        GameManager.Instance.ForestUIManager.SetUIDocForMainMenu();
    }

    void OnSubmit()
    {
        ChangeSceneFromMenu("MainMenu");
    }

    void ChangeSceneFromMenu(string scene)
    {
        GameManager.Instance.SceneChanger.ChangeLevel(scene);
    }

}
