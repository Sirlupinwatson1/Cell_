using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace controllibrary
{
    public class messagebox : uibase
    {
        Border border = new Border()
        {
            BorderThickness = new Thickness(2),
            BorderBrush = Brushes.Gray,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Background = Brushes.White,
            CornerRadius = new CornerRadius(10)
        };
        StackPanel panel = new StackPanel();
        TextBlock textblock = new TextBlock() { Margin = new Thickness(5) };
        StackPanel buttompanel = new StackPanel()
        {
            Margin = new Thickness(5),
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        public messagebox()
        {
            border.Child = panel;
            border.Padding = new Thickness(10);
            panel.Children.Add(textblock);
            panel.Children.Add(buttompanel);
        }
        public override FrameworkElement element => border;
        TaskCompletionSource<int> rt;
        public async Task<int> dialog((string subject, string text) message, params string[] options)
        {
            rt = new TaskCompletionSource<int>();
            if (message.subject == null)
                message.subject = "Message";
            textblock.Inlines.Add(new Run() { Foreground = Brushes.Brown, Text = message.subject, FontSize = 14 });
            textblock.Inlines.Add(new LineBreak());
            textblock.Inlines.Add(new LineBreak());
            textblock.Inlines.Add(new Run() { Foreground = Brushes.Black, Text = message.text, FontSize = 12 });
            if (buttompanel == null || options.Length == 0)
                options = new string[] { "OK" };
            for (int i = 0; i < options.Length; i++)
            {
                string text = options[i];
                var dv = new button()
                {
                    Content = text,
                    Margin = new Thickness(2)
                };
                dv.Click += Dv_Click;
                dv.DataContext = i;
                buttompanel.Children.Add(dv);
            }
            var rsv = await rt.Task;
            foreach (button i in buttompanel.Children)
            {
                i.Click -= Dv_Click;
            }
            textblock.Inlines.Clear();
            buttompanel.Children.Clear();
            return rsv;
        }
        static messagebox main = new messagebox();
        public static async Task<int> maindilog((string subject, string text) message, params string[] options)
        {
            loadbox.maindialog(main);
            return await main.dialog(message, options);
        }

        public override event Action<uibase> close_e;
        private void Dv_Click(object sender, RoutedEventArgs e)
        {
            var dv = sender as Button;
            run(() =>
            {
                rt.SetResult((int)dv.DataContext);
                close_e?.Invoke(this);
                textblock.Inlines.Clear();
                buttompanel.Children.Clear();
            });
        }

        public static async Task maindilog(string v)
        {
            await maindilog((null, v));
        }
    }
}