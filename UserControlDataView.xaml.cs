using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace gtForensics
{
    /// <summary>
    /// UserControlDashBoard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControlDataView : UserControl
    {
        MainWindow _context;
        string _t_name;

        public UserControlDataView(MainWindow context, string t_name)
        {
            InitializeComponent();
            _context = context;
            _t_name = t_name;
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if ((_context.tables.ContainsKey(_t_name) == true) && (_context.tables[_t_name].Rows.Count > 0))
            {
                dataGrid.ItemsSource = _context.tables[_t_name].DefaultView;
            }
            else
            {
                DataTable dt = new DataTable();
                DataColumn dt_col = new DataColumn("INFO");
                DataRow dt_row = dt.NewRow();

                dt.Columns.Add(dt_col);
                dt_row[dt_col] = "No Data!";
                dt.Rows.Add(dt_row);

                dataGrid.ItemsSource = dt.DefaultView;
            }
        }
    }
}
