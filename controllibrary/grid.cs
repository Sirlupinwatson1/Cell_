using stemcell;
using controllibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace controllibrary
{
    public abstract class grid : uiapp
    {
        public DataGrid body = new DataGrid()
        {
            AutoGenerateColumns = false,
            CanUserAddRows = false,
            Margin = new Thickness(20, 0, 20, 20)
        };
        public IEnumerable source
        {
            get => body.ItemsSource;
            set => body.ItemsSource = value;
        }
        public void refresh()
        {
            body.Items.Refresh();
        }
        public override FrameworkElement element => body;
        public void add(heder heder, DataGridColumn column)
        {
            column.Header = heder;
            heder.reset_e += resetsearch;
            body.Columns.Add(column);
        }
        protected abstract void resetsearch(bool online);
    }
}