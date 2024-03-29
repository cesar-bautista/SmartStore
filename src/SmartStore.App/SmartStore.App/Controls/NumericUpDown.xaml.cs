﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartStore.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NumericUpDown : ContentView
    {
        public event EventHandler<int> OnValueChanged;

        public NumericUpDown()
        {
            InitializeComponent();
            ValueText.SetBinding(Label.TextProperty, new Binding(nameof(Value), BindingMode.TwoWay, source: this));
        }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(NumericUpDown), 1.0,
                propertyChanged: (bindable, oldValue, newValue) =>
                    ((NumericUpDown)bindable).Value = (double)newValue,
                defaultBindingMode: BindingMode.TwoWay
            );

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(Minimum), typeof(double), typeof(NumericUpDown), 0.0,
                propertyChanged: (bindable, oldValue, newValue) =>
                    ((NumericUpDown)bindable).Minimum = (double)newValue
            );

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(Maximum), typeof(double), typeof(NumericUpDown), 100.0,
                propertyChanged: (bindable, oldValue, newValue) =>
                    ((NumericUpDown)bindable).Maximum = (double)newValue
            );

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly BindableProperty StepProperty =
            BindableProperty.Create(nameof(Step), typeof(double), typeof(NumericUpDown), 1.0,
                propertyChanged: (bindable, oldValue, newValue) =>
                    ((NumericUpDown)bindable).Step = (double)newValue
            );

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NumericUpDown), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(NumericUpDown), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // PROPERTIES:
        // - Minimum
        // - Maximum
        // - Value
        // - Step
        // OTHER POSSIBLE PROPERTIES:
        // - Animate
        // - NumericBackgroundColor
        // - NumericBorderColor
        // - NumericTextColor
        // - NumericBorderThickness
        // - NumericCornerRadius

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ValueProperty.PropertyName)
            {
                ValueText.Text = Value.ToString();
                OnValueChanged?.Invoke(this, (int)Value);
            }
        }

        private async void MinusTapped(object sender, EventArgs e)
        {
            await AnimateAsync(MinusButton);

            if ((Value - Step) > Minimum)
                Value -= Step;
            else
                Value = Minimum;

            if (Command != null && Command.CanExecute(null))
                Command.Execute(CommandParameter);
        }

        private async void PlusTapped(object sender, EventArgs e)
        {
            await AnimateAsync(PlusButton);

            if (Value < Maximum)
                Value += Step;

            if (Command != null && Command.CanExecute(null))
                Command.Execute(CommandParameter);
        }

        private async Task AnimateAsync(VisualElement element)
        {
            await element.ScaleTo(0.9, 50, Easing.Linear);
            await Task.Delay(100);
            await element.ScaleTo(1, 50, Easing.Linear);
        }
    }
}