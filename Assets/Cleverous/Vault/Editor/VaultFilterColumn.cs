// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public abstract class VaultFilterColumn : VaultDashboardColumn
    {
        protected string CurrentTypeName;
        protected bool IsSubscribed;
        protected static List<Type> AllValidTypesCache;
        protected static List<IVaultTypeButton> AllButtonsCache;

        /// <summary>
        /// <para>Vault will not read types from any assemblies starting with these prefixes.</para>
        /// <para>This is done to improve compile times by ignoring namespaces that will never
        /// contain a Type which inherits from DataEntity.</para>
        /// <para>Please check your assembly names if you aren't seeing content in the Type List.</para>
        /// </summary>
        protected readonly string[] Blacklist =
        {
            "System",
            "Mono.",
            "Unity.",
            "UnityEngine",
            "UnityEditor",
            "mscorlib",
            "SyntaxTree",
            "netstandard",
            "nunit"
        };
        protected static Assembly VaultAssy;
        protected static AssemblyName VaultAssyName;

        public abstract void ScrollTo(VisualElement button);
        public abstract void Filter(string f);
        public abstract void FilterBySearchBar();
    }
}