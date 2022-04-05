// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public interface IVaultTypeButton
    {
        Type SourceType { get; set; }
        VisualElement MainElement { get; set; }
        VisualElement InternalElement { get; set; }
        void SetSelectionColor(bool isActive);
        void SetAsFilter();
        void SetVisible(bool state);
        void SetFoldoutVisibility(bool state);
        void SetIndentPx(int px);
    }
}