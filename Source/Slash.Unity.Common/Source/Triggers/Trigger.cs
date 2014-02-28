namespace Slash.Unity.Common.Triggers
{
    using System.Collections.Generic;

    using Slash.Unity.Common.Triggers.Actions;
    using Slash.Unity.Common.Triggers.Conditions;

    using UnityEngine;

    public class Trigger : MonoBehaviour
    {
        #region Methods

        public List<ICondition> Conditions;

        public List<IAction> Actions; 

        /// <summary>
        ///   Called before first Update call.
        /// </summary>
        private void Start()
        {
            // Find conditions/actions.
            this.Conditions = new List<ICondition>();
            this.Actions = new List<IAction>();
            MonoBehaviour[] children = this.GetComponentsInChildren<MonoBehaviour>();
            foreach (var child in children)
            {
                ICondition condition = child as ICondition;
                if (condition != null)
                {
                    this.Conditions.Add(condition);
                }
                IAction action = child as IAction;
                if (action != null)
                {
                    this.Actions.Add(action);
                }
            }

            // Register for condition fulfillment.
            if (this.Conditions != null)
            {
                foreach (var condition in this.Conditions)
                {
                    condition.Fulfilled += this.OnConditionFulfilled;
                }
            }
        }

        private void OnConditionFulfilled()
        {
            // TODO(co): Check if all conditions are fulfilled.

            // Trigger actions.
            this.TriggerActions();
        }

        private void TriggerActions()
        {
            if (this.Actions == null || this.Actions.Count == 0)
            {
                return;
            }

            foreach (var action in this.Actions)
            {
                action.Execute(null);
            }
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        private void Update()
        {
        }

        #endregion
    }
}
