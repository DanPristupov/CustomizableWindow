using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Amusoft.Wpf.Extensions;
using Binding = System.Windows.Data.Binding;
using Button = System.Windows.Controls.Button;

namespace Amusoft.Wpf
{
	[ContentProperty(nameof(Content))]
	[TemplatePart(Name = PartNameMinimizeButton, Type = typeof(Button))]
	[TemplatePart(Name = PartNameMaximizeButton, Type = typeof(Button))]
	[TemplatePart(Name = PartNameRestoreButton, Type = typeof(Button))]
	[TemplatePart(Name = PartNameCloseButton, Type = typeof(Button))]
	[TemplatePart(Name = PartNameWindowHeader, Type = typeof(FrameworkElement))]
	public class CustomizableWindow : Window
	{
		protected const string PartNameWindowHeader = "PART_WindowHeader";
		protected const string PartNameCloseButton = "PART_CloseButton";
		protected const string PartNameRestoreButton = "PART_RestoreButton";
		protected const string PartNameMinimizeButton = "PART_MinimizeButton";
		protected const string PartNameMaximizeButton = "PART_MaximizeButton";

		static CustomizableWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizableWindow), new FrameworkPropertyMetadata(typeof(CustomizableWindow)));
		}

		public static readonly DependencyProperty WindowFrameThicknessProperty = DependencyProperty.Register(
			nameof(WindowFrameThickness), typeof(Thickness), typeof(CustomizableWindow), new PropertyMetadata(default(Thickness)));

		public Thickness WindowFrameThickness
		{
			get { return (Thickness)GetValue(WindowFrameThicknessProperty); }
			private set { SetValue(WindowFrameThicknessProperty, value); }
		}

		public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(
			nameof(HeaderVisibility), typeof(Visibility), typeof(CustomizableWindow), new PropertyMetadata(default(Visibility)));

		public Visibility HeaderVisibility
		{
			get { return (Visibility)GetValue(HeaderVisibilityProperty); }
			set { SetValue(HeaderVisibilityProperty, value); }
		}

		public static readonly DependencyProperty MinimizeButtonVisibilityProperty = DependencyProperty.Register(
			nameof(MinimizeButtonVisibility), typeof (Visibility), typeof (CustomizableWindow), new PropertyMetadata(default(Visibility)));

		public Visibility MinimizeButtonVisibility
		{
			get { return (Visibility) GetValue(MinimizeButtonVisibilityProperty); }
			set { SetValue(MinimizeButtonVisibilityProperty, value); }
		}

		public static readonly DependencyProperty MaximizeButtonVisibilityProperty = DependencyProperty.Register(
			nameof(MaximizeButtonVisibility), typeof (Visibility), typeof (CustomizableWindow), new PropertyMetadata(default(Visibility)));

		public Visibility MaximizeButtonVisibility
		{
			get { return (Visibility) GetValue(MaximizeButtonVisibilityProperty); }
			set { SetValue(MaximizeButtonVisibilityProperty, value); }
		}

		public static readonly DependencyProperty RestoreButtonVisibilityProperty = DependencyProperty.Register(
			nameof(RestoreButtonVisibility), typeof (Visibility), typeof (CustomizableWindow), new PropertyMetadata(default(Visibility)));

		public Visibility RestoreButtonVisibility
		{
			get { return (Visibility) GetValue(RestoreButtonVisibilityProperty); }
			set { SetValue(RestoreButtonVisibilityProperty, value); }
		}

		public static readonly DependencyProperty CloseButtonVisibilityProperty = DependencyProperty.Register(
			nameof(CloseButtonVisibility), typeof (Visibility), typeof (CustomizableWindow), new PropertyMetadata(default(Visibility)));

		public Visibility CloseButtonVisibility
		{
			get { return (Visibility) GetValue(CloseButtonVisibilityProperty); }
			set { SetValue(CloseButtonVisibilityProperty, value); }
		}

		public static readonly DependencyProperty MinimizeButtonStyleProperty = DependencyProperty.Register(
			nameof(MinimizeButtonStyle), typeof (Style), typeof (CustomizableWindow), new PropertyMetadata(default(Style)));

		public Style MinimizeButtonStyle
		{
			get { return (Style) GetValue(MinimizeButtonStyleProperty); }
			set { SetValue(MinimizeButtonStyleProperty, value); }
		}

		public static readonly DependencyProperty MaximizeButtonStyleProperty = DependencyProperty.Register(
			nameof(MaximizeButtonStyle), typeof (Style), typeof (CustomizableWindow), new PropertyMetadata(default(Style)));

		public Style MaximizeButtonStyle
		{
			get { return (Style) GetValue(MaximizeButtonStyleProperty); }
			set { SetValue(MaximizeButtonStyleProperty, value); }
		}

		public static readonly DependencyProperty RestoreButtonStyleProperty = DependencyProperty.Register(
			nameof(RestoreButtonStyle), typeof (Style), typeof (CustomizableWindow), new PropertyMetadata(default(Style)));

		public Style RestoreButtonStyle
		{
			get { return (Style) GetValue(RestoreButtonStyleProperty); }
			set { SetValue(RestoreButtonStyleProperty, value); }
		}

		public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(
			nameof(CloseButtonStyle), typeof (Style), typeof (CustomizableWindow), new PropertyMetadata(default(Style)));

		public Style CloseButtonStyle
		{
			get { return (Style) GetValue(CloseButtonStyleProperty); }
			set { SetValue(CloseButtonStyleProperty, value); }
		}

		public static readonly DependencyProperty WindowButtonForegroundProperty = DependencyProperty.Register(
			nameof(WindowButtonForeground), typeof (Brush), typeof (CustomizableWindow), new PropertyMetadata(default(Brush)));

		public Brush WindowButtonForeground
		{
			get { return (Brush) GetValue(WindowButtonForegroundProperty); }
			set { SetValue(WindowButtonForegroundProperty, value); }
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property.Name == nameof(WindowState))
			{
				AdjustVisibilityToWindowState();

				if (WindowState == WindowState.Maximized)
				{
					WindowFrameThickness = GetMaximizedWindowFrameThickness();
				}
				else
				{
					WindowFrameThickness = new Thickness(0);
				}
			}
		}

		protected void AdjustVisibilityToWindowState()
		{
			switch (WindowState)
			{
				case WindowState.Normal:
					MaximizeButtonVisibility = Visibility.Visible;
					RestoreButtonVisibility = Visibility.Collapsed;
					break;
				case WindowState.Minimized:
					break;
				case WindowState.Maximized:
					MaximizeButtonVisibility = Visibility.Collapsed;
					RestoreButtonVisibility = Visibility.Visible;
					break;
			}
		}

		protected virtual Thickness GetMaximizedWindowFrameThickness()
		{
			return new Thickness(SystemParameters.WindowResizeBorderThickness.Left*2, SystemParameters.WindowResizeBorderThickness.Top*2, SystemParameters.WindowResizeBorderThickness.Right*2, SystemParameters.WindowResizeBorderThickness.Bottom*2);
		}

		private FrameworkElement _templatePartWindowHeader;
		private Button _templatePartButtonClose;
		private Button _templatePartButtonMinimize;
		private Button _templatePartButtonMaximize;
		private Button _templatePartButtonRestore;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			FrameworkElement windowHeaderElement;
			if (Template.TryFindName(PartNameWindowHeader, this, out windowHeaderElement))
			{
				_templatePartWindowHeader = windowHeaderElement;
				var chrome = WindowChrome.GetWindowChrome(this);

				Binding captionHeightBinding = new Binding();
				captionHeightBinding.Source = windowHeaderElement;
				captionHeightBinding.Path = new PropertyPath(nameof(FrameworkElement.ActualHeight));
				captionHeightBinding.Mode = BindingMode.OneWay;
				captionHeightBinding.NotifyOnSourceUpdated = true;
				BindingOperations.SetBinding(chrome, WindowChrome.CaptionHeightProperty, captionHeightBinding);
			}

			if (Template.TryFindName(PartNameCloseButton, this, out _templatePartButtonClose))
			{
				_templatePartButtonClose.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonClose.PreviewMouseLeftButtonUp += TemplatePartButtonCloseOnMouseUp;
			}

			if (Template.TryFindName(PartNameMinimizeButton, this, out _templatePartButtonMinimize))
			{
				_templatePartButtonMinimize.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonMinimize.PreviewMouseLeftButtonUp += TemplatePartButtonMinimizeOnMouseUp;
			}

			if (Template.TryFindName(PartNameMaximizeButton, this, out _templatePartButtonMaximize))
			{
				_templatePartButtonMaximize.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonMaximize.PreviewMouseLeftButtonUp += TemplatePartButtonMaximizeOnMouseUp;
			}

			if (Template.TryFindName(PartNameRestoreButton, this, out _templatePartButtonRestore))
			{
				_templatePartButtonRestore.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonRestore.PreviewMouseLeftButtonUp += TemplatePartButtonRestoreOnMouseUp;
			}

			AdjustVisibilityToWindowState();
		}

		protected override void OnClosed(EventArgs e)
		{
			_templatePartButtonMaximize.PreviewMouseLeftButtonUp -= TemplatePartButtonMaximizeOnMouseUp;
			_templatePartButtonRestore.PreviewMouseLeftButtonUp -= TemplatePartButtonRestoreOnMouseUp;
			_templatePartButtonMinimize.PreviewMouseLeftButtonUp -= TemplatePartButtonMinimizeOnMouseUp;
			_templatePartButtonClose.PreviewMouseLeftButtonUp -= TemplatePartButtonCloseOnMouseUp;
			base.OnClosed(e);
		}

		protected virtual void  OnWindowButtonAction(WindowButtonActionEventArgs args)
		{

		}

		private void TemplatePartButtonCloseOnMouseUp(object sender, MouseButtonEventArgs args)
		{
			var windowActionArgs = new WindowButtonActionEventArgs(WindowAction.Close);
			OnWindowButtonAction(windowActionArgs);
			if (windowActionArgs.IsCanceled)
				return;

			Close();
		}

		private void TemplatePartButtonRestoreOnMouseUp(object sender, MouseButtonEventArgs args)
		{
			var windowActionArgs = new WindowButtonActionEventArgs(WindowAction.Restore);
			OnWindowButtonAction(windowActionArgs);
			if (windowActionArgs.IsCanceled)
				return;

			WindowState = WindowState.Normal;
		}

		private void TemplatePartButtonMinimizeOnMouseUp(object sender, MouseButtonEventArgs args)
		{
			var windowActionArgs = new WindowButtonActionEventArgs(WindowAction.Minimize);
			OnWindowButtonAction(windowActionArgs);
			if (windowActionArgs.IsCanceled)
				return;

			WindowState = WindowState.Minimized;
		}

		private void TemplatePartButtonMaximizeOnMouseUp(object sender, MouseButtonEventArgs args)
		{
			var windowActionArgs = new WindowButtonActionEventArgs(WindowAction.Maximize);
			OnWindowButtonAction(windowActionArgs);
			if (windowActionArgs.IsCanceled)
				return;

			WindowState = WindowState.Maximized;
		}
	}

	public enum WindowAction
	{
		Minimize,
		Maximize,
		Restore,
		Close
	}

	public class WindowButtonActionEventArgs : EventArgs
	{
		public WindowButtonActionEventArgs(WindowAction action)
		{
			Action = action;
		}

		public WindowAction Action { get; set; }

		public bool IsCanceled { get; private set; }

		public void Cancel()
		{
			IsCanceled = true;
		}
	}
}
