using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace controllibrary
{
    public class style : Style
    {
        public style(Type type) : base(type) { }
        public void add(DependencyProperty property, object value)
        {
            Setters.Add(new Setter()
            {
                Property = property,
                Value = value
            });
        }
        public static style combobox(bool isreadonly = true)
        {
            style dv = new style(typeof(ComboBox));
            dv.add(FrameworkElement.MarginProperty, new Thickness(4));
            dv.add(ComboBox.IsEnabledProperty, isreadonly);
            return dv;
        }
        public static style checkbox(string value = default)
        {
            style dv = new style(typeof(CheckBox));
            dv.add(FrameworkElement.MarginProperty, new Thickness(4));
            dv.add(ContentControl.ContentProperty, value);
            dv.add(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            return dv;
        }
        public static style textblock()
        {
            style dv = new style(typeof(TextBlock));
            dv.add(FrameworkElement.MarginProperty, new Thickness(4));
            return dv;
        }
        public static style textbox(bool isreadonly = true)
        {
            style dv = new style(typeof(TextBox));
            dv.add(FrameworkElement.MarginProperty, new Thickness(4));
            dv.add(TextBoxBase.IsReadOnlyProperty, isreadonly);
            return dv;
        }
    }
}