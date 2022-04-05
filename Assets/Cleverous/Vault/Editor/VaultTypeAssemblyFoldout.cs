// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultTypeAssemblyFoldout : Foldout
    {
        public string AssemblyName;
        public List<Type> OwnedTypes;

        public VaultTypeAssemblyFoldout(string assemblyName)
        {
            AssemblyName = assemblyName;
            text = assemblyName;
            style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            value = false;
            OwnedTypes = new List<Type>();
        }

        public VaultTypeButton AddTypeButton(Type t, Foldout parentFoldout)
        {
            OwnedTypes.Add(t);
            string label = t.Name;

            VisualElement wrapper = new VisualElement();
            wrapper.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            wrapper.style.alignItems = new StyleEnum<Align>(Align.Stretch);
            wrapper.style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);

            VaultTypeButton interactButton = new VaultTypeButton(t, parentFoldout);
            interactButton.text = label;
            interactButton.clicked += interactButton.SetAsFilter;
            interactButton.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft);
            interactButton.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Normal);
            interactButton.style.flexGrow = 1;

            interactButton.style.borderBottomLeftRadius = 0;
            interactButton.style.borderBottomRightRadius = 0;
            interactButton.style.borderTopLeftRadius = 0;
            interactButton.style.borderTopRightRadius = 0;

            Label prefix = new Label(t.IsAbstract ? "○" : "●");
            prefix.style.color = t.IsAbstract ? Color.gray : Color.green;

            if (interactButton.SourceType == VaultDashboard.CurrentTypeFilter)
            {
                if (VaultDashboard.CurrentTypeFilter != t)
                {
                    interactButton.SetVisible(true);
                }
                else interactButton.SetAsFilter();
            }

            wrapper.Add(prefix);
            wrapper.Add(interactButton);
            Add(wrapper);

            return interactButton;
        }

        public virtual void Reveal()
        {
            value = true;
        }
    }
}