using System.Collections.Generic;
using System.Linq;

namespace EZData
{
    public delegate void VariableContextChangeDelegate();
    
    public abstract class VariableContext : Context
    {
        public event VariableContextChangeDelegate OnChange;
		protected void InvokeOnChange()
		{
			if (OnChange != null)
               	OnChange();
		}

		private readonly List<IBinding> _dependencies = new List<IBinding>();
		
		public abstract Context BaseValue { get; }
		
        protected override void AddBindingDependency(IBinding binding)
        {
            if (binding == null)
                return;

            if (_dependencies.Contains(binding))
                return;
            _dependencies.Add(binding);
        }

        public List<IBinding> GetDependenciesAndCleanup()
        {
            var result = new List<IBinding>(_dependencies);
            _dependencies.Clear();
            return result;
        }
    }

    public class VariableContext<T> : VariableContext
	{
		public override Context BaseValue
		{
			get { return _value as Context; }
		}
		
        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                var dependencies = GetDependenciesAndCleanup();
                _value = value;
                foreach (var dependency in dependencies)
                {
					try
					{
                    	dependency.OnContextChange();
					}
					catch(UnityEngine.MissingReferenceException)
					{
					}
                }

                InvokeOnChange();
            }
        }

        public VariableContext(T value)
		{
			_value = value;
		}
		public VariableContext(T value, VariableContextChangeDelegate onChange)
			: this(value)
		{
			OnChange += onChange;
		}
    }
}
