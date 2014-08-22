// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorVectorAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Attributes
{
    public class InspectorVectorAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        public InspectorVectorAttribute(string name)
            : base(name)
        {
        }

        #endregion
    }
}