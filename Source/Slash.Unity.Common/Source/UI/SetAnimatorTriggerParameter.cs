namespace Slash.Unity.Common.UI
{
    public class SetAnimatorTriggerParameter : SetAnimatorParameter<bool>
    {
        protected override void InternalSetValue(bool value)
        {
            if (value)
            {
                this.Animator.SetTrigger(this.Parameter);
            }
            else
            {
                this.Animator.ResetTrigger(this.Parameter);
            }
        }
    }
}