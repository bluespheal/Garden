﻿// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using UnityEngine;

namespace Cleverous.VaultSystem
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.GenericParameter | AttributeTargets.Property)]
    public class SpritePreviewAttribute : PropertyAttribute
    {
        public SpritePreviewAttribute() { }
    }
}