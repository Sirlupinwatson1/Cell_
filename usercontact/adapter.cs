using controllibrary;
using Dna.userdata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace usercontact
{
    class adapter : comboadapter
    {
        public override List<string> itemsource { get; } = new List<string>()
        {
            "Disconnected",
            "Connected",
            "Favorite",
            "Blocked"
        };
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return itemsource[(int)value];
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dv = itemsource.IndexOf(value.ToString());
            return (e_contactsetting)dv;
        }
    }
}