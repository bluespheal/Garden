// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cleverous.VaultSystem;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultFilterColumnInheritance : VaultFilterColumn
    {
        protected static ScrollView ScrollElement;
        protected List<string> AllAssemblyNames;
        private List<Type> m_completedTypes;
        private List<IVaultTypeButton> m_filteringMustShow = new List<IVaultTypeButton>();

        public override void Rebuild()
        {
            Clear();
            m_filteringMustShow = new List<IVaultTypeButton>();

            ScrollElement = new ScrollView();
            ScrollElement.style.flexGrow = 1;
            this.Add(ScrollElement);

            VaultAssy = Assembly.GetAssembly(typeof(DataEntity));
            VaultAssyName = VaultAssy.GetName();

            AllValidTypesCache = new List<Type>();
            AllButtonsCache = new List<IVaultTypeButton>();

            if (!IsSubscribed)
            {
                VaultDashboard.OnTypeSearch += FilterBySearchBar;
            }

            IsSubscribed = true;

            AllAssemblyNames = new List<string>();
            m_completedTypes = new List<Type>();

            // loop through the assemblies available
            foreach (Assembly assy in AppDomain.CurrentDomain.GetAssemblies())
            {
                string assyName = assy.GetName().Name;
                if (assy.IsDynamic) continue; // ignore dynamics
                if (Blacklist.Any(ignored => assyName.StartsWith(ignored))) continue; // ignore blacklisted
                if (AllAssemblyNames.Any(n => n == assyName)) continue; // ignore duplicates

                // ** ASSEMBLY LEVEL
                foreach (AssemblyName q in assy.GetReferencedAssemblies())
                {
                    if (q.Name != VaultAssyName.Name) continue; // ignore anything not explicitly referencing Vault.
                    ProcessAssembly(assy, q, assyName);
                    AllAssemblyNames.Add(q.Name);
                }
            }

            // Include the Vault assembly, which doesn't reference itself and wouldn't normally be included.
            ProcessAssembly(VaultAssy, VaultAssyName, VaultAssyName.Name);

            // alphabetically sort the list
            AllButtonsCache.Sort((x, y) => string.CompareOrdinal(x.SourceType.Name, y.SourceType.Name));

            // go through the buttons that have been created and nest them.
            foreach (IVaultTypeButton currentButton in AllButtonsCache)
            {
                // nesting process already completed it?
                if (m_completedTypes.Contains(currentButton.SourceType)) continue;

                // DataEntities are already added always top level.
                if (currentButton.SourceType.BaseType == typeof(DataEntity))
                {
                    // add it into the main element list.
                    ScrollElement.Add(currentButton.MainElement);
                    m_completedTypes.Add(currentButton.SourceType);
                    continue;
                }

                // climb through the base types until reaching the DataEntity core class.
                NestedParent(currentButton);
            }

            // persistence
            CurrentTypeName = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.CurrentTypeName);
        }
        private void NestedParent(IVaultTypeButton button)
        {
            if (button.SourceType.BaseType == typeof(DataEntity) || button.SourceType == typeof(DataEntity))
            {
                //Debug.Log($"<color=red>Base: {button.SourceType.Name}</color>");
                ScrollElement.Add(button.MainElement);
                return;
            }

            // find parent
            IVaultTypeButton targetParent = AllButtonsCache.Find(x => x.SourceType == button.SourceType.BaseType);

            // add as child, then check the parent.
            if (targetParent == null) return;

            targetParent.InternalElement.Add(button.MainElement);
            button.SetIndentPx(15);
            targetParent.SetFoldoutVisibility(true);

            //Debug.Log($"<color=lime>Completed {button.SourceType.Name}</color>");
            m_completedTypes.Add(button.SourceType);

            if (targetParent.SourceType != typeof(DataEntity))
            {
                NestedParent(targetParent);
            }
        }
        protected virtual void ProcessAssembly(Assembly assy, AssemblyName q, string assyName)
        {
            // ** ASSEMBLY LEVEL
            IEnumerable<IGrouping<string, Type>> groups = assy.GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof(DataEntity)) || t == typeof(DataEntity))
                .GroupBy(t => t.Namespace);

            // ** NAMESPACE LEVEL
            foreach (IGrouping<string, Type> namespaceGroup in groups)
            {
                // ** TYPE LEVEL
                foreach (Type type in namespaceGroup)
                {
                    //Debug.Log($"<color=orange>Caching {type.Name}</color>");
                    AllButtonsCache.Add(new VaultFoldoutButton(type));
                }
            }
        }

        public override void ScrollTo(VisualElement button)
        {
            ScrollElement.ScrollTo(button);
        }
        public override void Filter(string filter)
        {
            m_filteringMustShow.Clear();
            if (string.IsNullOrEmpty(filter))
            {
                foreach (IVaultTypeButton button in AllButtonsCache)
                {
                    button.SetVisible(true);
                }
            }
            else
            {
                foreach (IVaultTypeButton button in AllButtonsCache)
                {
                    // turn it off
                    button.SetVisible(false);

                    // if there's a name match, turn it back on
                    bool isNameMatch = button.SourceType.Name.ToLower().Contains(filter.ToLower());
                    if (!isNameMatch) continue;

                    button.SetVisible(true);
                    FilterUpHierarchy(button);
                }

                foreach (IVaultTypeButton button in m_filteringMustShow)
                {
                    button.SetVisible(true);
                }
            }
        }
        private void FilterUpHierarchy(IVaultTypeButton button)
        {
            IVaultTypeButton buttonParent = AllButtonsCache.Find(x => x.SourceType == button.SourceType.BaseType);
            if (buttonParent != null)
            {
                m_filteringMustShow.Add(buttonParent);
                FilterUpHierarchy(buttonParent);
            }
        }
        public override void FilterBySearchBar()
        {
            Filter(VaultDashboard.SearchFieldForType.value);
        }
    }
}