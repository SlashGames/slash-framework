namespace Slash.Unity.Common.UI
{
    public class SetAnimatorBoolParameter : SetAnimatorParameter<bool>
    {
        protected override void InternalSetValue(bool value)
        {
            this.Animator.SetBool(this.Parameter, value);
        }
    }
}