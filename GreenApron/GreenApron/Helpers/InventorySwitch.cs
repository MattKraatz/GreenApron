using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GreenApron
{
    public class InventorySwitch : Switch
    {
        public static readonly BindableProperty InventoryItemProperty = BindableProperty.Create(
            propertyName: "InventoryItem",
            returnType: typeof(InventoryItem),
            declaringType: typeof(InventorySwitch),
            defaultValue: null);

        public InventoryItem InventoryItem
        {
            get { return (InventoryItem)GetValue(InventoryItemProperty); }
            set { SetValue(InventoryItemProperty, value); }
        }
    }
}
