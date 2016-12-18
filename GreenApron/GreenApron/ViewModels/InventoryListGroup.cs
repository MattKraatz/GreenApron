using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class InventoryListGroup : ObservableCollection<InventoryItem>
    {
        public string Title { get; set; }
        public string ShortName { get; set; } //will be used for jump lists
        public string Subtitle { get; set; }
        public InventoryListGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
        }
    }
}
