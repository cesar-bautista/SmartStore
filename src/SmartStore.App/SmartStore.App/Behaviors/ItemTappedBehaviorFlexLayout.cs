﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamanimation;
using Xamarin.Forms;

namespace SmartStore.App.Behaviors
{
    public class ItemTappedBehaviorFlexLayout : Behavior<FlexLayout>
    {
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemTappedBehaviorFlexLayout), defaultBindingMode: BindingMode.OneWay);

		public ICommand Command
		{
			get => (ICommand)this.GetValue(CommandProperty);
			set => this.SetValue(CommandProperty, value);
		}

		protected override void OnAttachedTo(FlexLayout bindable)
		{
			base.OnAttachedTo(bindable);

			if (bindable.BindingContext != null)
			{
				this.BindingContext = bindable.BindingContext;
			}

			bindable.BindingContextChanged += this.OnFlexLayoutBindingChanged;
			bindable.ChildAdded += this.OnFlexLayoutChildAdded;
		}

		protected override void OnDetachingFrom(FlexLayout bindable)
		{
			base.OnDetachingFrom(bindable);

			bindable.BindingContextChanged -= this.OnFlexLayoutBindingChanged;
			bindable.ChildAdded -= this.OnFlexLayoutChildAdded;

			foreach (var child in bindable.Children)
			{
				if (child is View childView && childView.GestureRecognizers.Any())
				{
					var tappedGestureRecognizers = childView.GestureRecognizers.Where(x => x is TapGestureRecognizer).Cast<TapGestureRecognizer>();
					foreach (var tapGestureRecognizer in tappedGestureRecognizers)
					{
						tapGestureRecognizer.Tapped -= this.OnItemTapped;
						childView.GestureRecognizers.Remove(tapGestureRecognizer);
					}
				}
			}
		}

		private void OnFlexLayoutBindingChanged(object sender, EventArgs e)
		{
			if (sender is FlexLayout flexLayout)
			{
				this.BindingContext = flexLayout.BindingContext;
			}
		}

		private void OnFlexLayoutChildAdded(object sender, ElementEventArgs args)
		{
			if (args.Element is View view)
			{
				var tappedGestureRecognizer = new TapGestureRecognizer();
				tappedGestureRecognizer.Tapped += this.OnItemTapped;

				view.GestureRecognizers.Add(tappedGestureRecognizer);
			}
		}

		private async void OnItemTapped(object sender, EventArgs e)
		{
			if (sender is VisualElement visualElement)
			{
				var animations = new List<AnimationBase>();
				var scaleIn = new ScaleToAnimation
				{
					Target = visualElement,
					Scale = .95,
					Duration = "50"
				};
				animations.Add(scaleIn);

				var scaleOut = new ScaleToAnimation
				{
					Target = visualElement,
					Scale = 1,
					Duration = "50"
				};
				animations.Add(scaleOut);

				var storyBoard = new StoryBoard(animations);
				await storyBoard.Begin();
			}

			if (sender is BindableObject bindable && this.Command != null && this.Command.CanExecute(null))
			{
				this.Command.Execute(bindable.BindingContext);
			}
		}
	}
}
