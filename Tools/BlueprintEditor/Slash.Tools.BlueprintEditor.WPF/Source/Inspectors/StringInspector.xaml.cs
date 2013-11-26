namespace BlueprintEditor.Inspectors
{
    using System;
    using System.Windows.Controls;

    using Slash.GameBase.Attributes;

    /// <summary>
    /// Interaction logic for StringInspector.xaml
    /// </summary>
    public sealed partial class StringInspector : IInspectorControl
    {
        public StringInspector()
        {
            this.InitializeComponent();

            this.TbValue.TextChanged += TbValueOnTextChanged;
            this.value = this.TbValue.Text;
        }

        private string value;

        private void TbValueOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            this.OnValueChanged(this.InspectorProperty, this.TbValue.Text, this.value);
            this.value = this.TbValue.Text;
        }

        public InspectorPropertyAttribute InspectorProperty { get; set; }

        public event InspectorControlValueChangedDelegate ValueChanged;

        private void OnValueChanged(InspectorPropertyAttribute inspectorproperty, object newValue, object oldValue)
        {
            InspectorControlValueChangedDelegate handler = this.ValueChanged;
            if (handler != null)
            {
                handler(inspectorproperty, newValue, oldValue);
            }
        }
    }
}
