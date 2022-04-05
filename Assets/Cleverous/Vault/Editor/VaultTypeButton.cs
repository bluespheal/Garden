// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultTypeButton : Button, IVaultTypeButton
    {
        public static VaultTypeButton CurrentSelected;

        public Type SourceType { get; set; }
        public VisualElement MainElement { get; set; }
        public VisualElement InternalElement { get; set; }

        public Foldout Foldout;

        private static Color InactiveColor => EditorGUIUtility.isProSkin ? DarkInactive : LightInactive;
        private static Color ActiveColor => EditorGUIUtility.isProSkin ? DarkActive : LightActive;

        private static readonly Color LightInactive = new Color(0.8941177f, 0.8941177f, 0.8941177f);
        private static readonly Color LightActive = new Color(0.5664399f, 0.8584906f, 0.3644536f);
        private static readonly Color DarkInactive = new Color(0.3647059f, 0.3647059f, 0.3647059f);
        private static readonly Color DarkActive = new Color(0.1602882f, 0.3647059f, 0.1568235f);

        public VaultTypeButton(Type sourceType, Foldout parentFoldout)
        {
            Foldout = parentFoldout;
            name = sourceType.Name;
            viewDataKey = sourceType.Name;
            SourceType = sourceType;
            style.backgroundColor = new StyleColor(InactiveColor);

            MainElement = this;
            InternalElement = this;
        }

        public virtual void SetVisible(bool state)
        {
            if (state)
            {
                if (Foldout != null) Foldout.value = true;
                parent.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
            else
            {
                parent.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
        }
        public virtual void SetAsFilter()
        {
            if (VaultDashboard.CurrentTypeFilter == SourceType) return;

            CurrentSelected?.SetSelectionColor(false);
            SetSelectionColor(true);
            CurrentSelected = this;

            VaultEditorSettings.SetString(VaultEditorSettings.VaultData.CurrentTypeName, SourceType.Name);
            VaultDashboard.SetTypeFilter(SourceType);
        }
        public virtual void SetSelectionColor(bool isCurrentType)
        {
            style.backgroundColor = isCurrentType ? ActiveColor : InactiveColor;
        }        
        
        public void SetFoldoutVisibility(bool state)
        {
            throw new NotImplementedException();
        }
        public void SetIndentPx(int px)
        {
            throw new NotImplementedException();
        }
    }
}