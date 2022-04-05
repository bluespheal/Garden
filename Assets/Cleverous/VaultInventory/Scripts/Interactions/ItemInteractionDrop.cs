// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultInventory.Example;

namespace Cleverous.VaultInventory
{
    public class ItemInteractionDrop : Interaction
    {
        protected override void Reset()
        {
            base.Reset();
            Title = "Interact Drop";
            Description = "Drop the item immediately.";
            InteractLabel = "Drop";
        }

        public override bool IsValid(IInteractableTransform target)
        {
            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return false;

            // You can only use this command from the Player inventory. Because reasons!
            VaultExampleCharacter player = plug.Ui.TargetInventory.GetComponent<VaultExampleCharacter>();
            return player != null;
        }

        public override void DoInteract(IInteractableTransform target)
        {
            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return;

            plug.Ui.TargetInventory.CmdRequestDrop(plug.ReferenceInventoryIndex);
        }
    }
}