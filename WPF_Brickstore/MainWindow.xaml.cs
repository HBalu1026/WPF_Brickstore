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

namespace WPF_Brickstore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Item> _items;
        private ICollectionView _view;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            SetUpFilter();
        }

        private void LoadData()
        {
            _items = DataLoader.LoadItems("C:\\Users\\hernadi.balazs\\Downloads\\brickstore_parts_3180-1-tank-truck.bsx");
            _view = CollectionViewSource.GetDefaultView(_items);
            ItemsDataGrid.ItemsSource = _view;
        }

        private void SetUpFilter()
        {
            FilterTextBox.TextChanged += (s, e) =>
            {
                _view.Filter = item =>
                {
                    if (string.IsNullOrEmpty(FilterTextBox.Text))
                        return true;

                    var itemData = item as Item;
                    return itemData.ItemID.StartsWith(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                           itemData.ItemName.StartsWith(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase);
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