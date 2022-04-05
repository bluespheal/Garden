// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements; 
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultAssetInspector : VaultDashboardColumn
    {
        protected SerializedObject TargetSerializedObject;
        private readonly ScrollView m_contentWindow;

        public VaultAssetInspector()
        {
            TargetSerializedObject = GetSerializedObj();
            m_contentWindow = new ScrollView();
            m_contentWindow.style.flexShrink = 1f;
            m_contentWindow.style.flexGrow = 1f;
            m_contentWindow.style.paddingBottom = 10;
            m_contentWindow.style.paddingLeft = 10;
            m_contentWindow.style.paddingRight = 10;
            m_contentWindow.style.paddingTop = 10;

            VaultDashboard.OnCurrentAssetChanged += Rebuild;
            VaultDashboard.OnDeleteAssetStart += InspectNothing;

            this.name = "Asset Inspector";
            this.viewDataKey = "ASSET_INSPECTOR";
            this.style.flexShrink = 1f;
            this.style.flexGrow = 1f;
            this.style.paddingBottom = 10;
            this.style.paddingLeft = 10;
            this.style.paddingRight = 10;
            this.style.paddingTop = 10;
            this.Add(m_contentWindow);
        }

        public override void Rebuild()
        {
            m_contentWindow.Clear();
            if (VaultDashboard.CurrentAsset == null)
            {
                InspectNothing();
                return;
            }

            TargetSerializedObject = GetSerializedObj();

            bool success = BuildInspectorProperties(TargetSerializedObject, m_contentWindow);
            if (success) m_contentWindow.Bind(TargetSerializedObject); // TODO BUG
        }

        private void InspectNothing()
        {
            m_contentWindow.Clear();
            m_contentWindow.Add(new Label { text = " ⓘ Asset Inspector" });
            m_contentWindow.Add(new Label("\n\n    ⚠ Please select an asset from the column to the left."));
        }
        private static SerializedObject GetSerializedObj()
        {
            return VaultDashboard.CurrentAsset == null 
                ? null 
                : Editor.CreateEditor(VaultDashboard.CurrentAsset).serializedObject;
        }
        private static bool BuildInspectorProperties(SerializedObject obj, VisualElement wrapper)
        {
            if (obj == null || wrapper == null) return false;
            wrapper.Add(new Label { text = " ⓘ Asset Inspector" });

            // if Unity ever makes their InspectorElement work then we can just use that instead of
            // butchering through the object and making each field manually. (since 2019)

            /*
            InspectorElement inspector = new InspectorElement(obj);
            inspector.style.flexGrow = 1;
            inspector.style.flexShrink = 1;
            inspector.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            inspector.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            inspector.style.alignItems = new StyleEnum<Align>(Align.Stretch);
            inspector.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            wrapper.Add(inspector);
            return true;
            */

            SerializedProperty iterator = obj.GetIterator();
            Type targetType = obj.targetObject.GetType();
            List<MemberInfo> members = new List<MemberInfo>(targetType.GetMembers());

            if (!iterator.NextVisible(true)) return false;
            do
            {
                PropertyField propertyField = new PropertyField(iterator.Copy())
                {
                    name = "PropertyField:" + iterator.propertyPath
                };

                MemberInfo member = members.Find(x => x.Name == propertyField.bindingPath);
                if (member != null)
                {
                    // TODO [Header()] and [Space()] are manually added until Unity supports them.
                    IEnumerable<Attribute> headers = member.GetCustomAttributes(typeof(HeaderAttribute));
                    IEnumerable<Attribute> spaces = member.GetCustomAttributes(typeof(SpaceAttribute));

                    foreach (Attribute x in headers)
                    {
                        HeaderAttribute actual = (HeaderAttribute)x;
                        Label header = new Label { text = actual.header };
                        header.style.unityFontStyleAndWeight = FontStyle.Bold;
                        wrapper.Add(new Label { text = " " });
                        wrapper.Add(header);
                    }
                    foreach (Attribute unused in spaces)
                    {
                        wrapper.Add(new Label { text = " " });
                    }
                }

                // if this property is the script field
                if (iterator.propertyPath == "m_Script" && obj.targetObject != null)
                {
                    // build the container
                    VisualElement container = new VisualElement();
                    container.style.flexGrow = 1;
                    container.style.flexShrink = 1;
                    container.style.alignItems = new StyleEnum<Align>(Align.Stretch);
                    container.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

                    propertyField.SetEnabled(false);
                    propertyField.style.flexGrow = 1;
                    propertyField.style.flexShrink = 1;

                    // build the focus script button
                    Button focusButton = new Button(() => EditorGUIUtility.PingObject(obj.FindProperty("m_Script").objectReferenceValue));
                    focusButton.text = "☲";
                    focusButton.style.minWidth = 20;
                    focusButton.style.maxWidth = 20;
                    focusButton.tooltip = "Ping this Script";                    
                    
                    // build the focus object button
                    Button focusAsset = new Button(() => EditorGUIUtility.PingObject(obj.targetObject));
                    focusAsset.text = "☑";
                    focusAsset.style.minWidth = 20;
                    focusAsset.style.maxWidth = 20;
                    focusAsset.tooltip = "Ping this Asset";

                    // draw it
                    container.Add(propertyField);
                    container.Add(focusButton);
                    container.Add(focusAsset);
                    wrapper.Add(container);
                }
                // if it isn't the script field, just add the property field like normal.
                else wrapper.Add(propertyField);
            }
            while (iterator.NextVisible(false));
            return true;
        }
    }
}