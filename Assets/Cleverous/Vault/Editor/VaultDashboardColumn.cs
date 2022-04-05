// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public abstract class VaultDashboardColumn : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<VisualElement, UxmlTraits> { }
        public abstract void Rebuild();
    }
}