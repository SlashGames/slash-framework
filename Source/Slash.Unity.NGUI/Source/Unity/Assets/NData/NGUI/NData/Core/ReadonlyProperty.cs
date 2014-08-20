namespace EZData
{
    public class ReadonlyProperty<T> : Property<T>
    {
		public override void SetValue(T value)
        {

        }

        internal void InternalSetValue(T value)
        {
            base.SetValue(value);
        }
    }
}
