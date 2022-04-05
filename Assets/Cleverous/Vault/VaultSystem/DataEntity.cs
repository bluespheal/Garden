// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;
using UnityEngine.Serialization;

namespace Cleverous.VaultSystem
{
    public abstract class DataEntity : ScriptableObject
    {
        [FormerlySerializedAs("Title")]
        [SerializeField] 
        private string m_title;
        public string Title { get => m_title; set => m_title = value; }

        [TextArea]
        [FormerlySerializedAs("Description")]
        [SerializeField] 
        private string m_description;
        public string Description { get => m_description; set => m_description = value; }

        protected virtual void Reset()
        {
            Title = $"UNASSIGNED.{System.DateTime.Now.TimeOfDay.TotalMilliseconds}";
            Description = "";
        }
    }
}