using System;

namespace EZData
{
    using Slash.Unity.NGUIExt.Util;

    public delegate void NotifyPropertyChanged();

    public class Property : IBindingPathTarget
    {
        public event NotifyPropertyChanged OnChange;
		
        protected void OnValueChanged()
        {
            if (OnChange != null)
            {
                OnChange();
            }
        }
    }

    public class Property<T> : Property
    {
        public Property()
        {
#if UNITY_FLASH
			_isValue = false;
#else
            _isValue = ReflectionUtils.IsValueType(typeof(T));
#endif
        }
		
		public bool IsOfType(System.Type t)
		{
			return t == typeof(T);
		}

        public Property(T value)
            : this()
        {
            _value = value;
        }
				
        public T GetValue()
        {
            return _value;
        }

        protected virtual bool IsValueDifferent(T value)
        {
			return !_value.Equals(value);
        }

        private bool IsClassDifferent(T value)
        {
            return !_value.Equals(value);
        }

        public virtual void SetValue(T value)
        {
            if (_changing)
                return;
            _changing = true;

            bool changed;

            if (_isValue)
            {
                changed = IsValueDifferent(value);
            }
            else
            {
// Value types are handled differently via cached typeof(T).IsValueType checkup
// ReSharper disable CompareNonConstrainedGenericWithNull
                changed = (value == null && _value != null) ||
                          (value != null && _value == null) ||
                          (_value != null && IsClassDifferent(value));
// ReSharper restore CompareNonConstrainedGenericWithNull
            }
            if (changed)
            {
                _value = value;
                OnValueChanged();
            }
            _changing = false;
        }


        private bool _changing;
        private T _value;
        private readonly bool _isValue;
    }



    public class FloatProperty : Property<float>
    {
        protected override bool IsValueDifferent(float value)
        {
            return Math.Abs(GetValue() - value) > 0.0001f;
        }
    }

    public class IntProperty : Property<int>
    {
        protected override bool IsValueDifferent(int value)
        {
            return GetValue() != value;
        }
    }

    public class DoubleProperty : Property<double>
    {
        protected override bool IsValueDifferent(double value)
        {
            return Math.Abs(GetValue() - value) > 0.000001;
        }
    }

    public class StringProperty : Property<string>
    {
        protected override bool IsValueDifferent(string value)
        {
            return GetValue() != value;
        }
    }

    public class BoolProperty : Property<bool>
    {
        protected override bool IsValueDifferent(bool value)
        {
            return GetValue() != value;
        }
    }

}
