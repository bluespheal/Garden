using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Couscous : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _root;
    private Label _dialogue;

    public string _neutralDialogue;
    public string _notEnoughMoneyDialogue;
    public string _purchaseDialogue;
    public string _limitDialogue;

    void Start()
    {
        _uIDocument = GetComponent<UIDocument>();
        _root = _uIDocument.rootVisualElement;
        _dialogue = _root.Q<Label>("Dialogue");

        NeutralDialogue();
    }

    public void NeutralDialogue()
    {
        _dialogue.text = _neutralDialogue;
    }
    public void NotEnoughMoneyDialogue()
    {
        _dialogue.text = _notEnoughMoneyDialogue;
    }
    public void PurchaseDialogue()
    {
        _dialogue.text = _purchaseDialogue;
    }

    public void LimitDialogue()
    {
        _dialogue.text = _limitDialogue;
    }
}
