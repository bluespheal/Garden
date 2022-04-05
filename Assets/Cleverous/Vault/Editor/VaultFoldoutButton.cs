// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultFoldoutButton : VisualElement, IVaultTypeButton
    {
        public static VaultFoldoutButton CurrentSelected;
        public Foldout FoldoutElement;
        public VisualElement FoldoutCheckmark;
        public Toggle FoldoutToggle;

        public Label PrefixElement;
        public Button ButtonElement;
        public Type SourceType { get; set; }
        public VisualElement MainElement { get; set; }
        public VisualElement InternalElement { get; set; }

        private static Color InactiveColor => EditorGUIUtility.isProSkin ? DarkInactive : LightInactive;
        private static Color ActiveColor => EditorGUIUtility.isProSkin ? DarkActive : LightActive;
        private static readonly Color LightInactive = new Color(0.8941177f, 0.8941177f, 0.8941177f);
        private static readonly Color LightActive = new Color(0.5664399f, 0.8584906f, 0.3644536f);
        private static readonly Color DarkInactive = new Color(0.3647059f, 0.3647059f, 0.3647059f);
        private static readonly Color DarkActive = new Color(0.1602882f, 0.3647059f, 0.1568235f);

        public VaultFoldoutButton(Type sourceType)
        {
            name = sourceType.Name;
            viewDataKey = sourceType.Name;
            SourceType = sourceType;
            MainElement = this;

            //MainElement.style.left = 3;
            MainElement.style.flexDirection = FlexDirection.Column;
            MainElement.style.alignItems = Align.Stretch;
            MainElement.style.justifyContent = Justify.FlexStart;

            ButtonElement = new Button();
            ButtonElement.text = sourceType.Name;
            ButtonElement.clicked += SetAsFilter;
            ButtonElement.style.unityTextAlign = TextAnchor.MiddleLeft;
            ButtonElement.style.unityFontStyleAndWeight = FontStyle.Normal;
            ButtonElement.style.flexGrow = 1;
            ButtonElement.style.position = Position.Absolute;
            ButtonElement.style.left = 28;
            ButtonElement.style.width = 350;

            ButtonElement.style.borderBottomLeftRadius = 0;
            ButtonElement.style.borderBottomRightRadius = 0;
            ButtonElement.style.borderTopLeftRadius = 0;
            ButtonElement.style.borderTopRightRadius = 0;

            FoldoutElement = new Foldout();
            FoldoutElement.style.width = 20;
            FoldoutElement.style.flexGrow = 0;
            FoldoutElement.text = string.Empty;
            FoldoutElement.contentContainer.style.marginLeft = 10;
            FoldoutCheckmark = FoldoutElement.Q<VisualElement>("unity-checkmark");
            FoldoutToggle = FoldoutElement.Q<Toggle>();
            FoldoutToggle.style.marginLeft = 0;

            PrefixElement = new Label(sourceType.IsAbstract ? "○" : "●");
            PrefixElement.style.unityOverflowClipBox = StyleKeyword.None;
            PrefixElement.style.position = Position.Absolute;
            PrefixElement.style.left = 16;
            PrefixElement.style.top = 3;
            PrefixElement.style.color = sourceType.IsAbstract ? Color.gray : Color.green;
            PrefixElement.style.unityTextAlign = TextAnchor.MiddleLeft;

            this.Add(PrefixElement);
            this.Add(ButtonElement);
            this.Add(FoldoutElement);

            InternalElement = FoldoutElement;
            SetFoldoutVisibility(false);
        }

        public virtual void SetVisible(bool state)
        {
            MainElement.style.display = state
                ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex)
                : new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        public void SetFoldoutVisibility(bool isVisible)
        {
            FoldoutCheckmark.style.width = isVisible ? 20 : 0;
        }

        public void SetIndentPx(int px)
        {
            MainElement.style.marginLeft = 0;
        }

        public virtual void SetAsFilter()
        {
            //Debug.Log($"Filter Button Clicked, current type: {VaultDashboard.CurrentTypeFilter}, and this type: {SourceType}");
            if (VaultDashboard.CurrentTypeFilter == SourceType) return;

            CurrentSelected?.SetSelectionColor(false);
            SetSelectionColor(true);
            CurrentSelected = this;

            VaultDashboard.SetTypeFilter(SourceType);
        }

        public virtual void SetSelectionColor(bool isActive)
        {
            ButtonElement.style.backgroundColor = isActive ? ActiveColor : InactiveColor;
        }
    }
}