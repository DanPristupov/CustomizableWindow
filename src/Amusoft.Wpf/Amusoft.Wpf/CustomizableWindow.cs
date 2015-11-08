using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interactivity;
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
	[TemplatePart(Name = PartNameMinimizeButton, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = PartNameMaximizeButton, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = PartNameRestoreButton, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = PartNameCloseButton, Type = typeof(FrameworkElement))]
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

		public static readonly DependencyProperty CloseButtonTemplateProperty = DependencyProperty.Register(
			nameof(CloseButtonTemplate), typeof (DataTemplate), typeof (CustomizableWindow), new PropertyMetadata(default(DataTemplate)));

		public DataTemplate CloseButtonTemplate
		{
			get { return (DataTemplate) GetValue(CloseButtonTemplateProperty); }
			set { SetValue(CloseButtonTemplateProperty, value); }
		}

		public static readonly DependencyProperty RestoreButtonTemplateProperty = DependencyProperty.Register(
			nameof(RestoreButtonTemplate), typeof (DataTemplate), typeof (CustomizableWindow), new PropertyMetadata(default(DataTemplate)));

		public DataTemplate RestoreButtonTemplate
		{
			get { return (DataTemplate) GetValue(RestoreButtonTemplateProperty); }
			set { SetValue(RestoreButtonTemplateProperty, value); }
		}

		public static readonly DependencyProperty MaximizeButtonTemplateProperty = DependencyProperty.Register(
			nameof(MaximizeButtonTemplate), typeof (DataTemplate), typeof (CustomizableWindow), new PropertyMetadata(default(DataTemplate)));

		public DataTemplate MaximizeButtonTemplate
		{
			get { return (DataTemplate) GetValue(MaximizeButtonTemplateProperty); }
			set { SetValue(MaximizeButtonTemplateProperty, value); }
		}

		public static readonly DependencyProperty MinimizeButtonTemplateProperty = DependencyProperty.Register(
			nameof(MinimizeButtonTemplate), typeof (DataTemplate), typeof (CustomizableWindow), new PropertyMetadata(default(DataTemplate)));

		public DataTemplate MinimizeButtonTemplate
		{
			get { return (DataTemplate) GetValue(MinimizeButtonTemplateProperty); }
			set { SetValue(MinimizeButtonTemplateProperty, value); }
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

		public static readonly DependencyProperty IsMinimizeButtonVisibleProperty = DependencyProperty.Register(
			nameof(IsMinimizeButtonVisible), typeof (bool), typeof (CustomizableWindow), new PropertyMetadata(default(bool)));

		public bool IsMinimizeButtonVisible
		{
			get { return (bool) GetValue(IsMinimizeButtonVisibleProperty); }
			set { SetValue(IsMinimizeButtonVisibleProperty, value); }
		}

		public static readonly DependencyProperty IsMaximizeButtonVisibleProperty = DependencyProperty.Register(
			nameof(IsMaximizeButtonVisible), typeof (bool), typeof (CustomizableWindow), new PropertyMetadata(default(bool)));

		public bool IsMaximizeButtonVisible
		{
			get { return (bool) GetValue(IsMaximizeButtonVisibleProperty); }
			set { SetValue(IsMaximizeButtonVisibleProperty, value); }
		}

		public static readonly DependencyProperty IsRestoreButtonVisibleProperty = DependencyProperty.Register(
			nameof(IsRestoreButtonVisible), typeof (bool), typeof (CustomizableWindow), new PropertyMetadata(default(bool)));

		public bool IsRestoreButtonVisible
		{
			get { return (bool) GetValue(IsRestoreButtonVisibleProperty); }
			set { SetValue(IsRestoreButtonVisibleProperty, value); }
		}

		public static readonly DependencyProperty IsCloseButtonVisibleProperty = DependencyProperty.Register(
			nameof(IsCloseButtonVisible), typeof (bool), typeof (CustomizableWindow), new PropertyMetadata(default(bool)));

		public bool IsCloseButtonVisible
		{
			get { return (bool) GetValue(IsCloseButtonVisibleProperty); }
			set { SetValue(IsCloseButtonVisibleProperty, value); }
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

		public static readonly DependencyProperty WindowButtonBackgroundProperty = DependencyProperty.Register(
			nameof(WindowButtonBackground), typeof (Brush), typeof (CustomizableWindow), new PropertyMetadata(default(Brush)));

		public Brush WindowButtonBackground
		{
			get { return (Brush) GetValue(WindowButtonBackgroundProperty); }
			set { SetValue(WindowButtonBackgroundProperty, value); }
		}

		public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register(
			nameof(HeaderStyle), typeof (Style), typeof (CustomizableWindow), new PropertyMetadata(default(Style)));

		public Style HeaderStyle
		{
			get { return (Style) GetValue(HeaderStyleProperty); }
			set { SetValue(HeaderStyleProperty, value); }
		}

		public static readonly DependencyProperty HeaderTitleBarStyleProperty = DependencyProperty.Register(
			nameof(HeaderTitleBarStyle), typeof (Style), typeof (CustomizableWindow), new PropertyMetadata(default(Style)));

		public Style HeaderTitleBarStyle
		{
			get { return (Style) GetValue(HeaderTitleBarStyleProperty); }
			set { SetValue(HeaderTitleBarStyleProperty, value); }
		}

		public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(
			nameof(TitleTemplate), typeof (DataTemplate), typeof (CustomizableWindow), new PropertyMetadata(default(DataTemplate)));

		public DataTemplate TitleTemplate
		{
			get { return (DataTemplate) GetValue(TitleTemplateProperty); }
			set { SetValue(TitleTemplateProperty, value); }
		}

		public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
			nameof(IconTemplate), typeof (DataTemplate), typeof (CustomizableWindow), new PropertyMetadata(default(DataTemplate)));

		public DataTemplate IconTemplate
		{
			get { return (DataTemplate) GetValue(IconTemplateProperty); }
			set { SetValue(IconTemplateProperty, value); }
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
					IsMaximizeButtonVisible = true;
					IsRestoreButtonVisible = false;
					break;
				case WindowState.Minimized:
					break;
				case WindowState.Maximized:
					IsMaximizeButtonVisible = false;
					IsRestoreButtonVisible = true;
					break;
			}
		}

		protected virtual Thickness GetMaximizedWindowFrameThickness()
		{
			return new Thickness(SystemParameters.WindowResizeBorderThickness.Left*2, SystemParameters.WindowResizeBorderThickness.Top*2, SystemParameters.WindowResizeBorderThickness.Right*2, SystemParameters.WindowResizeBorderThickness.Bottom*2);
		}

		private FrameworkElement _templatePartWindowHeader;
		private FrameworkElement _templatePartButtonClose;
		private FrameworkElement _templatePartButtonMinimize;
		private FrameworkElement _templatePartButtonMaximize;
		private FrameworkElement _templatePartButtonRestore;

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

			var visibilityConverter = new BooleanToVisibilityConverter();

			if (Template.TryFindName(PartNameCloseButton, this, out _templatePartButtonClose))
			{
				_templatePartButtonClose.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonClose.PreviewMouseLeftButtonUp += TemplatePartButtonCloseOnMouseUp;
				_templatePartButtonClose.SetBinding(VisibilityProperty, IsCloseButtonVisibleProperty, this, visibilityConverter);
			}

			if (Template.TryFindName(PartNameMinimizeButton, this, out _templatePartButtonMinimize))
			{
				_templatePartButtonMinimize.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonMinimize.PreviewMouseLeftButtonUp += TemplatePartButtonMinimizeOnMouseUp;
				_templatePartButtonMinimize.SetBinding(VisibilityProperty, IsMinimizeButtonVisibleProperty, this, visibilityConverter);
			}

			if (Template.TryFindName(PartNameMaximizeButton, this, out _templatePartButtonMaximize))
			{
				_templatePartButtonMaximize.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonMaximize.PreviewMouseLeftButtonUp += TemplatePartButtonMaximizeOnMouseUp;
				_templatePartButtonMaximize.SetBinding(VisibilityProperty, IsMaximizeButtonVisibleProperty, this, visibilityConverter);
			}

			if (Template.TryFindName(PartNameRestoreButton, this, out _templatePartButtonRestore))
			{
				_templatePartButtonRestore.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
				_templatePartButtonRestore.PreviewMouseLeftButtonUp += TemplatePartButtonRestoreOnMouseUp;
				_templatePartButtonRestore.SetBinding(VisibilityProperty, IsRestoreButtonVisibleProperty, this, visibilityConverter);
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

	public static class FrameworkElementExtensions
	{
		public static void SetBinding(this FrameworkElement target, DependencyProperty targetProperty, DependencyProperty sourceProperty, DependencyObject source, IValueConverter converter = null)
		{
			var binding = new Binding(sourceProperty.Name) {Mode = BindingMode.OneWay};
			binding.Converter = converter;
			target.SetBinding(targetProperty, binding);
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
