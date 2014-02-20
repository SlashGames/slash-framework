// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILocalizationTableSerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Localization
{
    using System.IO;

    public interface ILocalizationTableSerializer
    {
        #region Public Methods and Operators

        ILocalizationTable Deserialize(Stream stream);

        void Serialize(Stream stream, ILocalizationTable table);

        #endregion
    }
}