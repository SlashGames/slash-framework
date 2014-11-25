// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodDelegateDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Delegates
{
    using System.Collections.Generic;
    using System.Reflection;
    
    using Slash.Unity.Common.Delegates;
    using Slash.Unity.Editor.Common.Inspectors.Utils;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(MethodDelegate))]
    public class MethodDelegateDrawer : MemberReferenceDrawer
    {
        #region Properties

        protected override string MemberProperty
        {
            get
            {
                return "Method";
            }
        }

        protected override string SourceProperty
        {
            get
            {
                return "Target";
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

                MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                foreach (MethodInfo methodInfo in methods)
                {
                    Entry entry = new Entry { Target = monoBehaviour, MemberName = methodInfo.Name };
                    list.Add(entry);
                }
            }
            return list;
        }

        #endregion
    }
}