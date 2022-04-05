// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cleverous.VaultSystem;
using UnityEditor; 
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard 
{
    public class VaultAssetColumn : VaultDashboardColumn
    {
        public Action<DataEntity> OnPick;

        public ListView ListElement;
        private List<DataEntity> m_allAssetsOfFilteredType;
        private List<DataEntity> m_searchFilteredList;
        private bool m_isSearchFiltering;

        public List<DataEntity> CurrentSelections;

        public VaultAssetColumn()
        {
            Rebuild();
        }

        public override void Rebuild()
        {
            Clear();
            m_allAssetsOfFilteredType = new List<DataEntity>();

            this.style.flexGrow = 1;
            this.name = "Asset List Wrapper";
            this.viewDataKey = "asset_list_wrapper";

            ListElement = new ListView(m_allAssetsOfFilteredType, 16, ListMakeItem, ListBindItem);
            ListElement.name = "Asset List View";
            ListElement.viewDataKey = "asset_list";
            ListElement.style.flexGrow = 1;
            ListElement.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
            ListElement.selectionType = SelectionType.Multiple;

#if UNITY_2020_3_OR_NEWER
            ListElement.onSelectionChange += SelectAssetsInternal;
            ListElement.onItemsChosen += ChooseAssetInternal;
#else
            ListElement.onSelectionChanged += SelectAssetsInternal;
            ListElement.onItemChosen += ChooseAssetInternal;
#endif
            // plug into events for updates.
            VaultDashboard.OnAssetSearch += CallbackListBySearch;
            VaultDashboard.OnTypeFilterChangeComplete += CallbackListByType;

            Add(ListElement);
            
            if (!string.IsNullOrEmpty(VaultDashboard.SearchFieldForAsset.value))
            {
                // must pre-load the type matches list first as the search filter uses it.
                ListAssetsByType();
                ListAssetsBySearch();
            } 
            else ListAssetsByType();

            GetSelectionPersistence();
            Pick(VaultEditorSettings.GetInt(VaultEditorSettings.VaultData.CurrentAssetIndex));
        }

        private void CallbackListBySearch() {ListAssetsBySearch(true);}
        private void CallbackListByType() {ListAssetsByType(true);}
        
        public void ListAssetsByType(bool scrollToTop = false)
        {
            if (VaultDashboard.CurrentTypeFilter == null) return;

            //Debug.Log($"Match: Cache: <color=red>{m_typeCache}</color>, Target: <color=lime>{VaultDashboard.CurrentTypeFilter}</color>");
            m_isSearchFiltering = false;
            if (!Vault.IsReady) Vault.InitData();
            if (Vault.Data.Items.Count != 0)
            {
                m_allAssetsOfFilteredType = DatabaseBuilder.GetAllAssetsInProject(VaultDashboard.CurrentTypeFilter).ToList();
                m_allAssetsOfFilteredType.Sort((x, y) => string.CompareOrdinal(x.Title, y.Title));
                ListElement.itemsSource = m_allAssetsOfFilteredType;
                ListElement.Refresh();
                if (scrollToTop) ListElement.ScrollToItem(0);
            }
        }
        public void ListAssetsBySearch(bool scrollToTop = false)
        {
            if (m_allAssetsOfFilteredType.Count == 0) ListAssetsByType();

            //Debug.Log($"<color=yellow>Searching {m_allAssetsOfFilteredType.Count} assets.</color>");

            m_isSearchFiltering = true;
            m_searchFilteredList = m_allAssetsOfFilteredType.FindAll(SearchMatchesItem);
            m_searchFilteredList.Sort((x, y) => String.CompareOrdinal(x.Title, y.Title));

            //Debug.Log($"<color=green>Found {m_searchFilteredList.Count} matches.</color>"); 

            ListElement.itemsSource = m_searchFilteredList;
            ListElement.Refresh();
            if (scrollToTop) ListElement.ScrollToItem(0);
        }
        
        /// <summary>
        /// ONLY for use when you want something external to change the list selection.
        /// This will change the list index and subsequently trigger the internal method
        /// to fire the global changed event so everything else catches up.
        /// </summary>
        /// <param name="asset"></param>
        public void Pick(DataEntity asset)
        {
            // fail out
            if (asset == null) return;
            if (asset == VaultDashboard.CurrentAsset) return;
            if (!m_allAssetsOfFilteredType.Contains(asset)) Rebuild();

            // set index
            int index = ListElement.itemsSource.IndexOf(asset);
            ListElement.selectedIndex = index;
            ListElement.ScrollToItem(index);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.CurrentAssetIndex, ListElement.selectedIndex);
            OnPick?.Invoke(asset); 
        }        
        public void Pick(int index)
        {
            if (index < 0) return;
            index = Mathf.Clamp(index, 0, ListElement.itemsSource.Count-1);
            ListElement.selectedIndex = index;
            ListElement.ScrollToItem(index);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.CurrentAssetIndex, ListElement.selectedIndex);
        }

        private static bool SearchMatchesItem(DataEntity entity)
        {
            bool result = entity.Title.ToLower().Contains(VaultDashboard.SearchFieldForAsset.value.ToLower());
            return result;
        }
        /// <summary>
        /// ONLY for use when the list has chosen something.
        /// </summary>
        /// <param name="obj"></param>
        private void ChooseAssetInternal(object obj)
        {
            // fail
            DataEntity entity;
            if (ListElement.selectedIndex < 0) return;
            if (obj == null) return;
            if (obj is IList)
            {
                entity = ((List<object>) obj)[0] as DataEntity;
                if (entity == VaultDashboard.CurrentAsset) return;
            }
            else
            {
                entity = (DataEntity) obj;
                if (entity == VaultDashboard.CurrentAsset) return;
            }

            // set index in prefs
            int index = ListElement.itemsSource.IndexOf(entity);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.CurrentAssetIndex, ListElement.selectedIndex);

            // broadcast change
            VaultDashboard.SetCurrentInspectorAsset(entity);
        }
#if UNITY_2020_3_OR_NEWER
        private void SelectAssetsInternal(IEnumerable<object> input)
        {
            List<object> objs = (List<object>)input;
#else
        private void SelectAssetsInternal(List<object> objs)
        {
#endif
            CurrentSelections = objs.ConvertAll(asset => (DataEntity)asset);
            StringBuilder sb = new StringBuilder();
            foreach (DataEntity assetFile in CurrentSelections)
            {
                sb.Append(AssetDatabase.GetAssetPath(assetFile) + "|");
            }

            VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SelectedAssetGuids, sb.ToString());
            ChooseAssetInternal(objs[0]);
        }
        private void GetSelectionPersistence()
        {
            string selected = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SelectedAssetGuids);
            if (string.IsNullOrEmpty(selected)) return;

            CurrentSelections = new List<DataEntity>();
            string[] split = selected.Split('|');
            foreach (string path in split)
            {
                if (path == string.Empty || path.Contains('|')) continue;
                CurrentSelections.Add(AssetDatabase.LoadAssetAtPath<DataEntity>(path));
            }
            VaultDashboard.CurrentAsset = CurrentSelections[0];
        }
        
        private void ListBindItem(VisualElement element, int listIndex)
        {
            // find the serialized property
            Editor ed = Editor.CreateEditor(m_isSearchFiltering ? m_searchFilteredList[listIndex] : m_allAssetsOfFilteredType[listIndex]);
            SerializedObject so = ed.serializedObject;
            SerializedProperty prop = so.FindProperty("m_title");

            // build a prefix
            ((Label) element.ElementAt(0)).text = listIndex.ToString(" ▪ ");

            // bind the label to the serialized target target property title
            ((Label) element.ElementAt(1)).BindProperty(prop);
        }
        private static VisualElement ListMakeItem()
        {
            VisualElement selectableItem = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                    flexBasis = 1,
                    flexShrink = 1,
                    flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap)
                }
            };
            selectableItem.Add(new Label {name = "Prefix", text = "error", style = {unityFontStyleAndWeight = FontStyle.Bold}});
            selectableItem.Add(new Label {name = "DB Title", text = "unknown"});
            return selectableItem;
        }

        /// <summary>
        /// Creates a new asset of the provided type, then focuses the dashboard on it.
        /// </summary>
        /// <param name="t">Type to create. Must derive from DataEntity.</param>
        /// <returns>The newly created asset object</returns>
        public DataEntity NewAsset(Type t) 
        {
            if (!Vault.IsReady) Vault.InitData();
            if (t == null)
            {
                Debug.LogError("Type for new asset cannot be null.");
                return null;
            }
            if (t.IsAbstract)
            {
                Debug.LogError("Cannot create instances of abstract classes.");
                return null;
            }

            const string prefix = "Data-";
            const string suffix = ".asset";
            string timeHash = Math.Abs(DateTime.Now.GetHashCode()).ToString();
            string filename = $"{prefix}{t.Name}-{timeHash}{suffix}";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{Vault.VaultItemPath}{filename}");

            ScriptableObject asset = ScriptableObject.CreateInstance(t);

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            DatabaseBuilder.BuildDatabase();

            DataEntity real = (DataEntity)AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathAndName);

            VaultDashboard.SearchFieldForAsset.SetValueWithoutNotify(string.Empty);
            if (t != VaultDashboard.CurrentTypeFilter) VaultDashboard.SetTypeFilter(t);

            Rebuild();
            Pick(real);

            return real;
        }

        public void CloneSelection()
        {
            if (VaultDashboard.CurrentTypeFilter == null) return;
            if (VaultDashboard.CurrentAsset == null) return;

            const string prefix = "Data-";
            const string suffix = ".asset";
            string timeHash = Math.Abs(DateTime.Now.GetHashCode()).ToString();
            string filename = $"{prefix}{VaultDashboard.CurrentAsset.GetType().Name}-{timeHash}{suffix}";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{Vault.VaultItemPath}{filename}");

            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(VaultDashboard.CurrentAsset), assetPathAndName);
            DataEntity newEntity = AssetDatabase.LoadAssetAtPath<DataEntity>(assetPathAndName);
            newEntity.Title += " (CLONED)";
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            DataEntity real = (DataEntity)AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathAndName);
            VaultDashboard.SearchFieldForAsset.SetValueWithoutNotify(string.Empty);
            
            Rebuild();

            int i = Mathf.Clamp(ListElement.itemsSource.IndexOf(real), 0, ListElement.itemsSource.Count);
            ListElement.ScrollToItem(i);
            ListElement.selectedIndex = i;
        }
        public void DeleteSelection()
        {
            StringBuilder sb = new StringBuilder();
            if (CurrentSelections.Count == 0) return;
            if (VaultDashboard.CurrentAsset == null) return;

            foreach (DataEntity asset in CurrentSelections)
            {
                if (asset == null) continue;
                sb.Append(asset.Title + "\n");
            }
            bool confirm = EditorUtility.DisplayDialog("Purge warning!", $"Delete assets from the disk?\n\n{sb}", "Yes", "Cancel");
            if (!confirm) return;

            foreach (DataEntity asset in CurrentSelections)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            }
            VaultDashboard.CurrentAsset = null;

            Rebuild();
        }
    }
} 