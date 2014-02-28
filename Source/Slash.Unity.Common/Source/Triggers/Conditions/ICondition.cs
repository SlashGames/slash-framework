// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.Unity.Common.Triggers.Conditions
{
    public delegate void ConditionFulfilledDelegate();

    public interface ICondition
    {
        event ConditionFulfilledDelegate Fulfilled;
    }
}