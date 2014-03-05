// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewEventDelegateDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.ViewModels
{
    using System.Collections.Generic;
    using System.Reflection;

    using Slash.Unity.Common.ViewModels;
    using Slash.Unity.Editor.Common.Inspectors.Utils;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(ViewEventDelegate))]
    public class ViewEventDelegateDrawer : MemberReferenceDrawer
    {
        #region Properties

        protected override string MemberProperty
        {
            get
            {
                return "field";
            }
        }

        protected override string SourceProperty
        {
            get
            {
                return "source";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Collect a list of usable routed events from the specified target game object.
        /// </summary>
        protected override List<Entry> GetApplicableMembers(GameObject target)
        {
            MonoBehaviour[] components = target.GetComponents<MonoBehaviour>();

            List<Entry> list = new List<Entry>();

            foreach (MonoBehaviour monoBehaviour in components)
            {
                if (monoBehaviour == null)
                {
                    continue;
                }

                FieldInfo[] fields = monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

                foreach (FieldInfo info in fields)
                {
                    if (info.FieldType != typeof(ViewEvent))
                    {
                        continue;
                    }

                    Entry entry = new Entry { Target = monoBehaviour, MemberName = info.Name };
                    list.Add(entry);
                }
            }
            return list;
        }

        #endregion
    }
}