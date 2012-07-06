// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributeEditor.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace RainyGames.Unity.Common.GUI.ValueEditors
{
    /// <summary>
    ///   Editor to change a value.
    /// </summary>
    public interface IValueEditor
    {
        /// <summary>
        ///   Edits the specified context.
        /// </summary>
        /// <param name="context">Editor context to work with.</param>
        void Edit(IValueEditorContext context);
    }
}