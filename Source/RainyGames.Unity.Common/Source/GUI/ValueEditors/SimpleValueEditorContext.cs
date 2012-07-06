// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleValueEditorContext.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace RainyGames.Unity.Common.GUI.ValueEditors
{
    using System;

    public class SimpleValueEditorContext : IValueEditorContext
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }

        public object Key { get; set; }

        public Type Type { get; set; }
    }
}