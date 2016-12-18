using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class GroceryListGroup : ObservableCollection<GroceryItem>
    {
        public string Title { get; set; }
        public string ShortName { get; set; } //will be used for jump lists
        public string Subtitle { get; set; }
        public GroceryListGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
        }
    }
}
