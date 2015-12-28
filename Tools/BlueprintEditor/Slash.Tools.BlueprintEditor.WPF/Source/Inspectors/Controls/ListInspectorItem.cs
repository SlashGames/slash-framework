// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInspectorItem.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using Slash.ECS.Inspector.Attributes;

    [TemplatePart(Name = "PART_BtDelete", Type = typeof(Button))]
    [TemplatePart(Name = "PART_BtUp", Type = typeof(Button))]
    [TemplatePart(Name = "PART_BtDown", Type = typeof(Button))]
    public class ListInspectorItem : Control
    {
        #region Static Fields

        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register(
            "Control",
            typeof(InspectorControl),
            typeof(ListInspectorItem),
            new PropertyMetadata { PropertyChangedCallback = OnControlChanged });

        public static readonly RoutedEvent DeleteClickedEvent = EventManager.RegisterRoutedEvent(
            "DeleteClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListInspectorItem));

        public static readonly RoutedEvent DownClickedEvent = EventManager.RegisterRoutedEvent(
            "DownClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListInspectorItem));

        public static readonly RoutedEvent UpClickedEvent = EventManager.RegisterRoutedEvent(
            "UpClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListInspectorItem));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListInspectorItem));

        #endregion

        #region Fields

        /// <summary>
        ///   Button to delete item from list.
        /// </summary>
        private Button buttonDelete;

        /// <summary>
        ///   Button to move item down in list.
        /// </summary>
        private Button buttonDown;

        /// <summary>
        ///   Button to move button up in list.
        /// </summary>
        private Button buttonUp;

        #endregion

        #region Constructors and Destructors

        static ListInspectorItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ListInspectorItem), new FrameworkPropertyMetadata(typeof(ListInspectorItem)));
        }

        #endregion

        #region Public Events

        public event RoutedEventHandler DeleteClicked
        {
            add
            {
                this.AddHandler(DeleteClickedEvent, value);
            }
            remove
            {
                this.RemoveHandler(DeleteClickedEvent, value);
            }
        }

        public event RoutedEventHandler DownClicked
        {
            add
            {
                this.AddHandler(DownClickedEvent, value);
            }
            remove
            {
                this.RemoveHandler(DownClickedEvent, value);
            }
        }

        public event RoutedEventHandler UpClicked
        {
            add
            {
                this.AddHandler(UpClickedEvent, value);
            }
            remove
            {
                this.RemoveHandler(UpClickedEvent, value);
            }
        }

        public event RoutedEventHandler ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        #endregion

        #region Public Properties

        public InspectorControl Control
        {
            get
            {
                return (InspectorControl)this.GetValue(ControlProperty);
            }
            set
            {
                this.SetValue(ControlProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.buttonDelete = this.GetTemplateChild("PART_BtDelete") as Button;
            if (this.buttonDelete != null)
            {
                this.buttonDelete.Click += this.ButtonDeleteOnClick;
            }

            this.buttonDown = this.GetTemplateChild("PART_BtDown") as Button;
            if (this.buttonDown != null)
            {
                this.buttonDown.Click += this.ButtonDownOnClick;
            }

            this.buttonUp = this.GetTemplateChild("PART_BtUp") as Button;
            if (this.buttonUp != null)
            {
                this.buttonUp.Click += this.ButtonUpOnClick;
            }
        }

        #endregion

        #region Methods

        private static void OnControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Register for ValueChanged event.
            ListInspectorItem listInspectorItem = (ListInspectorItem)d;

            InspectorControl inspectorControl = (InspectorControl)e.OldValue;
            if (inspectorControl != null)
            {
                inspectorControl.ValueChanged -= listInspectorItem.OnValueChanged;
            }

            inspectorControl = (InspectorControl)e.NewValue;
            if (inspectorControl != null)
            {
                inspectorControl.ValueChanged += listInspectorItem.OnValueChanged;
            }
        }

        private void ButtonDeleteOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.RaiseEvent(new RoutedEventArgs(DeleteClickedEvent));
        }

        private void ButtonDownOnClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(DownClickedEvent));
        }

        private void ButtonUpOnClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(UpClickedEvent));
        }

        private void OnValueChanged(InspectorPropertyAttribute inspectorProperty, object newValue)
        {
            this.RaiseEvent(new ValueChangedEventArgs { RoutedEvent = ValueChangedEvent, NewValue = newValue });
        }

        #endregion

        public class ValueChangedEventArgs : RoutedEventArgs
        {
            #region Public Properties

            public object NewValue { get; set; }

            #endregion
        }
    }
}