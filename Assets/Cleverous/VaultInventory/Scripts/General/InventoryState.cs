// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using Cleverous.VaultSystem;
using UnityEngine;

namespace Cleverous.VaultInventory
{
    [Serializable]
    public partial class InventoryState
    {  
        /// <summary>
        /// The Vault Index ID of the Inventory Configuration.
        /// </summary>
        public int ConfigIndex;
        /// <summary>
        /// A list of Vault Index IDs to identify items in each slot.
        /// </summary>
        public List<int> ItemIndexes;
        /// <summary>
        /// A list of int's to indicate the stack size in each slot.
        /// </summary>
        public List<int> ItemStackCounts;

        public InventoryState(Inventory source, List<RootItemStack> content)
        {
            ConfigIndex = Vault.GetIndex(source.Configuration);
            ItemIndexes = new List<int>();
            ItemStackCounts = new List<int>();

            foreach (RootItemStack t in content)
            {
                ItemIndexes.Add(t != null ? Vault.GetIndex(t.Source) : -1);
                ItemStackCounts.Add(t != null ? t.StackSize : 0);
            }
        }

        public virtual string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }
        public static InventoryState FromJson(string json)
        {
            return JsonUtility.FromJson<InventoryState>(json);
        }
    }
}