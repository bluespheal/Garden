// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using Cleverous.VaultSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Cleverous.VaultDashboard
{
    public class VaultDashboard : EditorWindow
    {
        // current values
        private const string UxmlAssetName = "vault_dashboard_uxml";
        public static DataEntity CurrentAsset; 

        public static Type CurrentTypeFilter
        {
            get
            {
                if (m_editorWindow.m_currentTypeFilter == null)
                {
                    m_editorWindow.m_currentTypeFilter = Type.GetType(VaultEditorSettings.GetString(VaultEditorSettings.VaultData.CurrentTypeFullName));
                }
                return m_editorWindow.m_currentTypeFilter;
            }
            set
            {
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.CurrentTypeName, value.Name); 
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.CurrentTypeFullName, value.AssemblyQualifiedName);
                m_editorWindow.m_currentTypeFilter = value;
            }
        }
        private Type m_currentTypeFilter;
         
        // toolbar
        protected static Historizer Historizer;
        public static ToolbarSearchField SearchFieldForType; // TODO move these.
        public static ToolbarSearchField SearchFieldForAsset;// TODO move these.
        public static bool SearchTypeIsDirty => SearchFieldForType != null && SearchFieldForType.value != m_typeSearchCache;
        public static bool SearchAssetIsDirty => SearchFieldForAsset != null && SearchFieldForAsset.value != m_assetSearchCache;
        private static string m_assetSearchCache;
        private static string m_typeSearchCache;

        // columns
        public static VaultFilterColumn FilterColumn;
        public static VaultAssetColumn AssetColumn;
        public static VaultAssetInspector InspectorColumn;

        protected enum FilterColumnType { Namespace, Inheritance, Groups }
        protected static FilterColumnType CurrentFilterColumnView;

        // action callbacks
        public static Action OnTypeFilterChangeComplete;

        public static Action OnCurrentAssetChanged;
        public static Action OnAssetSearch;
        public static Action OnTypeSearch;

        public static Action OnDeleteAssetStart;
        public static Action OnDeleteAssetComplete;  

        public static Action OnCreateNewAssetStart;
        public static Action OnCreateNewAssetComplete;  
        
        public static Action OnCloneAssetStart;
        public static Action OnCloneAssetComplete;

        // wrappers for views
        protected static VisualElement WrapperForFilterColumn;
        protected static VisualElement WrapperForAssetList;
        protected static VisualElement WrapperForAssetContent;
        protected static VisualElement WrapperForInspector;

        private static Button m_newButton;
        private static Button m_deleteButton;
        private static Button m_cloneButton;

        private static VaultDashboard m_editorWindow;

        [MenuItem("Tools/Cleverous/Vault Dashboard %#i", priority = 0)]
        public static void Open()
        {
            if (m_editorWindow != null)
            {
                FocusWindowIfItsOpen(typeof(VaultDashboard));
                return;
            }

            m_editorWindow = GetWindow<VaultDashboard>();
            m_editorWindow.minSize = new Vector2(850, 200);

            CurrentFilterColumnView = (FilterColumnType)VaultEditorSettings.GetInt(VaultEditorSettings.VaultData.FilterView);
        }
        public void OnEnable()
        {
            RebuildFull();
        }
        public void OnGUI()
        {
            if (SearchAssetIsDirty)
            {
                m_assetSearchCache = SearchFieldForAsset.value;
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SearchAssets, m_assetSearchCache);
                OnAssetSearch?.Invoke();
            }

            if (SearchTypeIsDirty)
            {
                m_typeSearchCache = SearchFieldForType.value;
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SearchType, m_typeSearchCache);
                OnTypeSearch?.Invoke();
            }

            // BUG required for some reason - state lost on domain reload
            if (CurrentTypeFilter != null && CurrentTypeFilter.IsAbstract) m_newButton.SetEnabled(false); 
        }
        
        private void LoadUxmlTemplate()
        {
            // load uxml and elements
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>(UxmlAssetName);
            visualTree.CloneTree(rootVisualElement);

            // find important parts and reference them
            WrapperForFilterColumn = rootVisualElement.Q<VisualElement>("TYPE_COLUMN");
            WrapperForAssetList = rootVisualElement.Q<VisualElement>("AssetColumnWrapper");
            WrapperForAssetContent = rootVisualElement.Q<VisualElement>("ASSET_COLUMN");
            WrapperForInspector = rootVisualElement.Q<VisualElement>("INSPECT_COLUMN");
            SearchFieldForType = rootVisualElement.Q<ToolbarSearchField>("TYPE_SEARCH");
            SearchFieldForAsset = rootVisualElement.Q<ToolbarSearchField>("ASSET_SEARCH");

            Historizer = new Historizer();
            rootVisualElement.Q<VisualElement>("TB_HISTORY").Add(Historizer);

            // init buttons
            m_newButton = WrapperForAssetList.Q<Button>("BUTTON_NEW");
            m_deleteButton = WrapperForAssetList.Q<Button>("BUTTON_DELETE");
            m_cloneButton = WrapperForAssetList.Q<Button>("BUTTON_CLONE");
            m_newButton.clicked += CreateNewAssetCallback;
            m_deleteButton.clicked += DeleteSelectedAsset;
            m_cloneButton.clicked += CloneSelectedAsset;
            rootVisualElement.Q<ToolbarButton>("TB_RELOAD").clicked += RebuildFull;
            rootVisualElement.Q<ToolbarButton>("TB_HELP").clicked += Help;
            rootVisualElement.Q<ToolbarButton>("TB_LIST").clicked += SetFilterViewType;
            rootVisualElement.Q<ToolbarButton>("TB_GROUP").clicked += ShowGroups;

            WrapperForFilterColumn.Add(FilterColumn);
            WrapperForAssetContent.Add(AssetColumn);
            WrapperForInspector.Add(InspectorColumn);

            // init split pane draggers
            // BUG - basically we have to do this because there is no proper/defined initialization for the drag anchor position.
            SplitView mainSplit = rootVisualElement.Q<SplitView>("MAIN_SPLIT");
            SplitView columnSplit = rootVisualElement.Q<SplitView>("FILTERS_PICK_SPLIT");
            mainSplit.fixedPaneInitialDimension = 549;
            columnSplit.fixedPaneInitialDimension = 250;
        }
        public void RebuildFull()
        {
            if (m_editorWindow == null) Open();

            // template and window
            m_editorWindow.titleContent.text = "Vault Dashboard";
            m_editorWindow.Focus();
            rootVisualElement.Clear();
            LoadUxmlTemplate();

            // full rebuild of the non-template content
            Rebuild(true);
        }
        public void Rebuild(bool fullRebuild = false)
        {
            // search data
            SearchFieldForType.SetValueWithoutNotify(VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SearchType));
            SearchFieldForAsset.SetValueWithoutNotify(VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SearchAssets));
            m_typeSearchCache = SearchFieldForType.value;
            m_assetSearchCache = SearchFieldForAsset.value;

            // rebuild
            RebuildFilterColumn(fullRebuild);
            RebuildAssetColumn(fullRebuild);
            RebuildInspectorColumn(fullRebuild);
        }

        private void RebuildFilterColumn(bool fullRebuild = false)
        {
            if (fullRebuild || FilterColumn == null)
            {
                FilterColumn?.RemoveFromHierarchy();
                FilterColumn = GetNewFilterColumnView();
                WrapperForFilterColumn.Add(FilterColumn);
            }
            FilterColumn.Rebuild();
        }
        private void RebuildAssetColumn(bool fullRebuild = false)
        {
            if (fullRebuild || AssetColumn == null)
            {
                AssetColumn?.RemoveFromHierarchy();
                AssetColumn = new VaultAssetColumn();
                WrapperForAssetContent.Add(AssetColumn);
            }
            AssetColumn.Rebuild();
        }
        private void RebuildInspectorColumn(bool fullRebuild = false)
        {
            if (fullRebuild || InspectorColumn == null)
            {
                InspectorColumn?.RemoveFromHierarchy();
                InspectorColumn = new VaultAssetInspector();
                WrapperForInspector.Add(InspectorColumn);
            }
            InspectorColumn.Rebuild();
        }

        private VaultFilterColumn GetNewFilterColumnView()
        {
            VaultFilterColumn column = CurrentFilterColumnView switch
            {
                FilterColumnType.Namespace => new VaultFilterColumnNamespace(),
                FilterColumnType.Inheritance => new VaultFilterColumnInheritance(),
                FilterColumnType.Groups => new VaultFilterColumnNamespace(), // TODO
                _ => throw new ArgumentOutOfRangeException()
            };
            return column;
        }

        public static void SetTypeFilter(Type t)
        {
            if (CurrentTypeFilter == t || t == null) return;
            if (!Vault.IsReady) Vault.InitData();

            CurrentTypeFilter = t; 
            m_newButton.SetEnabled(!t.IsAbstract);
            SearchFieldForAsset.value = string.Empty;
            OnTypeFilterChangeComplete?.Invoke();
        }
        public static void SetCurrentInspectorAsset(DataEntity asset)
        {
            CurrentAsset = asset;
            OnCurrentAssetChanged?.Invoke();
        }
        public static void InspectAssetRemote(Object asset, Type t)
        {
            if (asset == null && t == null) return;
            if (t == null) return;

            if (m_editorWindow == null) Open();
            m_editorWindow.Focus();
            SearchFieldForAsset.SetValueWithoutNotify(string.Empty);

            VisualElement button = WrapperForFilterColumn.Q<VisualElement>(t.Name);
            IVaultTypeButton buttonInterface = (IVaultTypeButton) button;
            buttonInterface.SetAsFilter();
            FilterColumn.ScrollTo(button);

            if (asset != null) AssetColumn.Pick((DataEntity)asset);
        }

        private static void CreateNewAssetCallback()
        {
            CreateNewAsset();
        }
        public static DataEntity CreateNewAsset()
        {
            OnCreateNewAssetStart?.Invoke();
            DataEntity newAsset = AssetColumn.NewAsset(CurrentTypeFilter);
            OnCreateNewAssetComplete?.Invoke();
            return newAsset;
        }
        public static DataEntity CreateNewAsset(Type t)
        {
            OnCreateNewAssetStart?.Invoke();
            DataEntity newAsset = AssetColumn.NewAsset(t);
            OnCreateNewAssetComplete?.Invoke();
            return newAsset;
        }
        public static void CloneSelectedAsset()
        {
            OnCloneAssetStart?.Invoke();
            AssetColumn.CloneSelection();
            OnCloneAssetComplete?.Invoke();
        }
        public static void DeleteSelectedAsset()
        {
            OnDeleteAssetStart?.Invoke();
            AssetColumn.DeleteSelection();
            OnDeleteAssetComplete?.Invoke();
        }
        public static void Help()
        {
            Application.OpenURL("https://lanefox.gitbook.io/vault/");
        }

        public void SetFilterViewType()
        {
            CurrentFilterColumnView = CurrentFilterColumnView switch
            {
                FilterColumnType.Namespace => FilterColumnType.Inheritance,
                FilterColumnType.Inheritance => FilterColumnType.Namespace,
                FilterColumnType.Groups => FilterColumnType.Inheritance,
                _ => throw new ArgumentOutOfRangeException()
            };
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.FilterView, (int)CurrentFilterColumnView);
            RebuildFilterColumn(true);
        }
        public void ShowGroups()
        {
            CurrentFilterColumnView = FilterColumnType.Groups;
        }
    }
}