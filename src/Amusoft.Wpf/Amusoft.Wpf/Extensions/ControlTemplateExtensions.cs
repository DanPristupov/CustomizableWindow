using System.Windows;
using System.Windows.Controls;

namespace Amusoft.Wpf.Extensions
{
	public static class ControlTemplateExtensions
	{
		public static bool TryFindName<T>(this ControlTemplate source, string name, FrameworkElement templatedParent, out T match) where T : class
		{
			var result = source.FindName(name, templatedParent);
			match = result as T;
			if (match != default(T))
				return true;

			return false;
		}
	}
}