using Microsoft.Maps.MapControl.WPF;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace gtForensics
{
    /// <summary>
    /// UserControlAnalysis_Map.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControlAnalysis_Map : UserControl
    {
        MainWindow _context;
        string _api_key;

        public UserControlAnalysis_Map(MainWindow context)
        {
            InitializeComponent();
            _context = context;

            time1.DisplayDateStart = new DateTime(1970, 1, 1);
            time2.DisplayDateStart = new DateTime(1970, 1, 1);
        }

        private Location Calc_Center()
        {
            double latitude = 0;
            double longitude = 0;

            return new Location(latitude, longitude);
        }

        private string Query_Builder(string timestamp_1, string timestamp_2)
        {
            string query = String.Format("SELECT {0}, latitude as Latitude, longitude as Longitude, altitude as Altitude, accuracy as Accuracy FROM parse_location_history WHERE timestamp >= {1} AND timestamp <= {2};", _context.Datetime_Builder("parse_location_history", 0), timestamp_1, timestamp_2);
            return query;
        }

        private void Load_Data(string timestamp_1, string timestamp_2)
        {
            map_view.Children.Clear();
            string strConn = String.Format("Data Source={0}", _context.sqlite_file_name);
            SQLiteConnection conn = new SQLiteConnection(strConn);
            conn.Open();

            string query = Query_Builder(timestamp_1, timestamp_2);
            SQLiteCommand _cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader _rdr = _cmd.ExecuteReader();

            DataTable t_data = new DataTable();
            t_data.Load(_rdr);

            conn.Close();

            foreach (DataRow row in t_data.Rows)
            {
                Pushpin pin = new Pushpin();
                pin.Location = new Location(Double.Parse(row[1].ToString()) / 10000000, Double.Parse(row[2].ToString()) / 10000000);
                map_view.Children.Add(pin);
            }
        }

        private void Grid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _api_key = _context.api_key;
            map_view.CredentialsProvider = new ApplicationIdCredentialsProvider(_api_key);
            map_view.Children.Clear();
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(time1.SelectedDate.HasValue)
            {
                time2.DisplayDateStart = time1.SelectedDate.Value;
            }

            if (time2.SelectedDate.HasValue)
            {
                time1.DisplayDateEnd = time2.SelectedDate.Value;
            }
        }

        private void View_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DateTime timestamp_1 = time1.SelectedDate.Value;
            DateTime timestamp_2 = time2.SelectedDate.Value;

            string t1 = ((timestamp_1.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000).ToString();
            string t2 = ((timestamp_2.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000).ToString();

            Load_Data(t1, t2);
        }
    }
}
