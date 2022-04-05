// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEditor;
using UnityEngine;

namespace Cleverous.VaultDashboard
{
    public static class VaultEditorSettings
    {
        public enum VaultData
        {
            CurrentAssetIndex,      // index of the currently selected asset
            CurrentTypeName,        // name of the current type selected
            CurrentTypeFullName,    // full name of the current type selected
            BreadcrumbBarGuids,     // guids
            SelectedAssetGuids,     // guids
            SearchType,             // the content of the search type field
            SearchAssets,           // the content of the search asset field
            FilterView              // which filter view index to use
        }

        public static int GetInt(VaultData data)
        {
            int result = EditorPrefs.GetInt(data.ToString());
            return result;
        }
        public static string GetString(VaultData data)
        {
            string result = EditorPrefs.GetString(data.ToString());
            return result;
        }

        public static void SetInt(VaultData data, int value)
        {
            EditorPrefs.SetInt(data.ToString(), value);
        }
        public static void SetString(VaultData data, string value)
        {
            EditorPrefs.SetString(data.ToString(), value);
        }
    }
}