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
            var dialog = new OpenFileDialog();
            _items = DataLoader.LoadItems(dialog.FileName);
            _view = CollectionViewSource.GetDefaultView(_items);
            ItemsDataGrid.ItemsSource = _view;
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
                    return //itemData.ItemID.StartsWith(FilterTextBoxId.Text, StringComparison.OrdinalIgnoreCase) ||
                           (itemData.ItemID.StartsWith(FilterTextBoxId.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxCategory.Text, StringComparison.OrdinalIgnoreCase));
                };
            };
            FilterTextBoxName.TextChanged += (s, e) =>
            {
                _view.Filter = item =>
                {
                    if (string.IsNullOrEmpty(FilterTextBoxName.Text))
                        return true;
                    
                    var itemData = item as Item;
                    return //itemData.ItemID.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) ||
                           (itemData.ItemID.StartsWith(FilterTextBoxId.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxCategory.Text, StringComparison.OrdinalIgnoreCase));
                };
            };
            FilterTextBoxCategory.TextChanged += (s, e) =>
            {
                _view.Filter = item =>
                {
                    if (string.IsNullOrEmpty(FilterTextBoxCategory.Text))
                        return true;

                    var itemData = item as Item;
                    return //itemData.ItemID.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) ||
                           (itemData.ItemID.StartsWith(FilterTextBoxId.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxName.Text, StringComparison.OrdinalIgnoreCase) &&
                           itemData.ItemName.StartsWith(FilterTextBoxCategory.Text, StringComparison.OrdinalIgnoreCase));
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

    public class DataLoader
    {
        public static List<Item> LoadItems(string filePath)
        {
            XDocument xaml = XDocument.Load(filePath);
            var items = xaml.Descendants("Item").Select(elem => new Item
            {
                ItemID = elem.Element("ItemID")?.Value,
                ItemName = elem.Element("ItemName")?.Value,
                CategoryName = elem.Element("CategoryName")?.Value,
                ColorName = elem.Element("ColorName")?.Value,
                Qty = int.Parse(elem.Element("Qty")?.Value ?? "0")
            }).ToList();
            return items;
        }
    }
}