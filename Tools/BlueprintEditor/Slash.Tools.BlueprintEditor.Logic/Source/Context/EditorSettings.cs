// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorSettings.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Context
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class EditorSettings
    {
        #region Constants

        /// <summary>
        ///   Language tag that shows raw localization keys.
        /// </summary>
        public const string LanguageTagRawLocalizationKeys = "RAW";

        private const int MaximumRecentProjects = 10;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Normalized BCP-47 language tag for the language to show all localized strings in.
        /// </summary>
        public string ProjectLanguage { get; set; }

        public List<string> RecentProjects { get; set; }

        #endregion

        #region Public Methods and Operators

        public void AddRecentProject(string projectPath)
        {
            if (this.RecentProjects == null)
            {
                this.RecentProjects = new List<string>();
            }

            // Remove project from recent projects (and add at beginning later).
            if (this.RecentProjects.Contains(projectPath))
            {
                this.RecentProjects.Remove(projectPath);
            }

            // Reverse order - most recent projects are now last in list.
            this.RecentProjects.Reverse();

            // Clamp list size - remove oldest project.
            if (this.RecentProjects.Count >= MaximumRecentProjects)
            {
                this.RecentProjects.RemoveAt(0);
            }

            // Add project.
            this.RecentProjects.Add(projectPath);

            // Reverse order - most recent projects are now first in list.
            this.RecentProjects.Reverse();
        }

        public void RemoveRecentProject(string projectPath)
        {
            this.RecentProjects.Remove(projectPath);
        }

        #endregion
    }
}