// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine.EventSystems;

namespace Cleverous.VaultInventory
{
    public class MerchantUiSellSlot : ItemUiPlug
    {
        public override void OnDrop(PointerEventData eventData)
        {
            base.OnDrop(eventData);

            if (!InventoryUi.DragOrigin.GetType().IsAssignableFrom(typeof(HotbarUiPlug)))
            {
                MerchantUi.Instance.ClientSell(InventoryUi.DragOrigin.ReferenceInventoryIndex);
            }

            if (InventoryUi.DragFloater != null) Destroy(InventoryUi.DragFloater);
            InventoryUi.DragOrigin = null;
            InventoryUi.DragDestination = null;
        }
    }
}