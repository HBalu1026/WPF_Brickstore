using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;

namespace WPF_Brickstore
{
    public partial class MainWindow : Window
    {
        private List<Item> _items;
        private ICollectionView _view;

        public MainWindow()
        {
            InitializeComponent();
            SetUpFilter();
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<Brick> bricks = new List<Brick>();
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    XDocument xaml = XDocument.Load(ofd.FileName);
                    foreach (var elem in xaml.Descendants("Item"))
                    {
                        bricks.Add(new Brick($"{elem.Element("ItemID").Value};{elem.Element("ItemName").Value};{elem.Element("CategoryName").Value};{elem.Element("ColorName").Value};{elem.Element("Qty").Value};"));
                    }
                    ItemsDataGrid.ItemsSource = bricks;
                }
                catch (System.Xml.XmlException)
                {
                    MessageBox.Show("Ez a fájl nem megfelelő fomrátumú!");
                }
            }
        }

        private void SetUpFilter()
        {
            FilterTextBoxId.TextChanged += (s, e) =>
            {
                _view.Filter = item =>
                {
                    if (string.IsNullOrEmpty(FilterTextBoxId.Text))
                        return true;

                    var itemData = item as Item;
                    return itemData.ItemID.StartsWith(FilterTextBoxId.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.CategoryName == cbCategory.SelectedValue;
                };
            };
            FilterTextBoxName.TextChanged += (s, e) =>
            {
                _view.Filter = item =>
                {
                    if (string.IsNullOrEmpty(FilterTextBoxName.Text))
                        return true;
                    
                    var itemData = item as Item;
                    return itemData.ItemID.StartsWith(FilterTextBoxId.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.CategoryName == cbCategory.SelectedValue;
                };
            };
            cbCategory.SelectionChanged += (s, e) => {
            {
                _view.Filter = item =>
                {
                    if (string.IsNullOrEmpty(cbCategory.SelectedItem))
                        return true;

                    var itemData = item as Item;
                    return itemData.ItemID.StartsWith(FilterTextBoxId.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.CategoryName == cbCategory.SelectedValue;
                };
            };
        }
    }

    public class Item
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string ColorName { get; set; }
        public int Qty { get; set; }
    }
}