// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Context
{
    using Slash.GameBase.Blueprints;

    public class EditorContext
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public EditorContext()
        {
            this.BlueprintManager = new BlueprintManager();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blueprint manager which is edited.
        /// </summary>
        public BlueprintManager BlueprintManager { get; set; }

        #endregion
    }
}