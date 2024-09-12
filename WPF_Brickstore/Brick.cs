using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Brickstore
{
    internal class Brick
    {
        string itemID;
        string itemName;
        string Category;
        string Color;
        int Qty;
        public Brick(string row)
        {
            string[] tomb = row.Split(';');
            itemID = tomb[0];
            itemName = tomb[1];
            Category = tomb[2];
            Color = tomb[3];
            Qty = int.Parse(tomb[4]);
        }
        public string ItemID => itemID;
        public string ItemName => itemName;
        public string CategoryName => Category;
        public string ColorName => Color;
        public int Qty1 => Qty;
    }
}
