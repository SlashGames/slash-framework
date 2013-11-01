// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Blueprints
{
    using System.Collections.Generic;

    /// <summary>
    ///   Utility methods for blueprints.
    /// </summary>
    public static class BlueprintUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Resolves the parent reference in the specified blueprint by searching in the specified
        ///   available blueprint manager.
        /// </summary>
        /// <param name="blueprint">Blueprint to resolve parent reference for.</param>
        /// <param name="blueprintManager">Blueprint manager to use to search for parent blueprints.</param>
        /// <exception cref="KeyNotFoundException">Thrown if parent couldn't be resolved.</exception>
        public static void resolveParent(Blueprint blueprint, IBlueprintManager blueprintManager)
        {
            if (string.IsNullOrEmpty(blueprint.ParentId) || blueprint.Parent != null)
            {
                return;
            }

            blueprint.Parent = blueprintManager.getBlueprint(blueprint.ParentId);
        }

        /// <summary>
        ///   Resolves the parent references in the blueprints of the specified manager by searching in the specified
        ///   available blueprint manager.
        /// </summary>
        /// <param name="blueprintManager">Blueprint manager to resolve parent references from.</param>
        /// <param name="availableBlueprintManager">Blueprint manager to use to search for parent blueprints.</param>
        /// <exception cref="KeyNotFoundException">Thrown if a parent couldn't be resolved.</exception>
        public static void resolveParents(
            IBlueprintManager blueprintManager, IBlueprintManager availableBlueprintManager)
        {
            foreach (Blueprint blueprint in blueprintManager)
            {
                resolveParent(blueprint, availableBlueprintManager);
            }
        }

        /// <summary>
        ///   Tries to resolve the parent reference in the specified blueprint by searching in the specified
        ///   available blueprint manager.
        /// </summary>
        /// <param name="blueprint">Blueprint to try to resolve parent reference for.</param>
        /// <param name="blueprintManager">Blueprint manager to use to search for parent blueprints.</param>
        public static bool tryResolveParent(Blueprint blueprint, IBlueprintManager blueprintManager)
        {
            if (string.IsNullOrEmpty(blueprint.ParentId) || blueprint.Parent != null)
            {
                return true;
            }

            Blueprint parentBlueprint;
            bool foundParent = blueprintManager.tryGetBlueprint(blueprint.ParentId, out parentBlueprint);
            blueprint.Parent = parentBlueprint;
            return foundParent;
        }

        /// <summary>
        ///   Tries to resolve the parent references in the blueprints of the specified manager by searching in the
        ///   specified available blueprint manager.
        /// </summary>
        /// <param name="blueprintManager">Blueprint manager to resolve parent references from.</param>
        /// <param name="availableBlueprintManager">Blueprint manager to use to search for parent blueprints.</param>
        public static void tryResolveParents(
            IBlueprintManager blueprintManager, IBlueprintManager availableBlueprintManager)
        {
            foreach (Blueprint blueprint in blueprintManager)
            {
                tryResolveParent(blueprint, availableBlueprintManager);
            }
        }

        #endregion
    }
}