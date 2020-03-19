using BeautySolutions.View.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace gtForensics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection conn = null;
        public string sqlite_file_name = null;
        public Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();
        public string utc_time = "+0";
        public string api_key = "";
        public string map_site = "";

        public MainWindow(string dbpath, string _utc, string _key)
        {
            InitializeComponent();

            var menuDataView = new List<SubItem>();
            menuDataView.Add(new SubItem("Android", new UserControlDataView(this, "parse_my_activity_android")));
            menuDataView.Add(new SubItem("Assistant", new UserControlDataView(this, "parse_my_activity_assistant")));
            menuDataView.Add(new SubItem("Contacts", new UserControlDataView(this, "parse_contacts")));
            menuDataView.Add(new SubItem("Google Analytics", new UserControlDataView(this, "parse_my_activity_google_analytics")));
            menuDataView.Add(new SubItem("Google Drive", new UserControlDataView(this, "parse_drive")));
            menuDataView.Add(new SubItem("Google Photo", new UserControlDataView(this, "parse_google_photo")));
            menuDataView.Add(new SubItem("Google Chrome", new UserControlDataView(this, "parse_my_activity_chrome")));
            menuDataView.Add(new SubItem("Google Map", new UserControlDataView(this, "parse_my_activity_map")));
            menuDataView.Add(new SubItem("G-Mail", new UserControlDataView(this, "parse_my_activity_gmail")));
            menuDataView.Add(new SubItem("Location History", new UserControlDataView(this, "parse_location_history")));
            menuDataView.Add(new SubItem("Youtube", new UserControlDataView(this, "parse_my_activity_youtube")));
            menuDataView.Add(new SubItem("Video Search", new UserControlDataView(this, "parse_my_activity_video_search")));
            menuDataView.Add(new SubItem("Voice Audio", new UserControlDataView(this, "parse_my_activity_voice_audio")));
            menuDataView.Add(new SubItem("Semantic Location History", new UserControlDataView(this, "parse_semantic_location_history")));
            menuDataView.Add(new SubItem("Map", new UserControlAnalysis_Map(this)));

            var itemDataView = new ItemMenu("Data View", menuDataView, PackIconKind.Database);

            //var menuAnalysis = new List<SubItem>();
            //menuAnalysis.Add(new SubItem("Map", new UserControlAnalysis_Map(this)));
            //var ItemAnalysis = new ItemMenu("Analysis", menuAnalysis, PackIconKind.Apps);

            Menu.Children.Add(new UserControlMenuItem(itemDataView, this));
            //Menu.Children.Add(new UserControlMenuItem(ItemAnalysis, this));

            sqlite_file_name = dbpath;
            utc_time = _utc;
            api_key = _key;

            Load_DB();
        }

        internal void SwitchScreen(object sender)
        {
            var screen = ((UserControl)sender);

            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }

        public void OpenDatabase()
        {
            string strConn = String.Format("Data Source={0}", sqlite_file_name);

            conn = new SQLiteConnection(strConn);
            conn.Open();
        }

        public void CloseDatabase()
        {
            conn.Close();
        }

        public string Datetime_Builder(string t_name, int option)
        {
            string result = null;
            string _hours = int.Parse(utc_time.Split(":")[0]).ToString();
            string _min = utc_time.Split(":")[1];

            if (t_name == "parse_google_photo")
            {
                if (_min == "00")
                {
                    result = String.Format("datetime(datetime(album_created_time, 'unixepoch'), '{0} hours') as 'Album Created Time(UTC{1})', datetime(datetime(photo_taken_time, 'unixepoch'), '{0} hours') as 'Photo Taken Time(UTC{1})', datetime(datetime(photo_created_time, 'unixepoch'), '{0} hours') as 'Photo Created Time(UTC{1})', datetime(datetime(photo_modified_time, 'unixepoch'), '{0} hours') as 'Photo Modified Time(UTC{1})', datetime(datetime(file_modified_time, 'unixepoch'), '{0} hours') as 'File Modified Time(UTC{1})'", _hours, utc_time);
                }
                else
                {
                    result = String.Format("datetime(datetime(album_created_time, 'unixepoch'), '{0} hours', '{1} minutes') as 'Album Created Time(UTC{2})', datetime(datetime(photo_taken_time, 'unixepoch'), '{0} hours', '{1} minutes') as 'Photo Taken Time(UTC{2})', datetime(datetime(photo_created_time, 'unixepoch'), '{0} hours', '{1} minutes') as 'Photo Created Time(UTC{2})', datetime(datetime(photo_modified_time, 'unixepoch'), '{0} hours', '{1} minutes') as 'Photo Modified Time(UTC{2})', datetime(datetime(file_modified_time, 'unixepoch'), '{0} hours', '{1} minutes') as 'File Modified Time(UTC{2})'", _hours, _min, utc_time);
                }
            }
            else if(t_name == "parse_drive")
            {
                if (_min == "00")
                {
                    result = String.Format("datetime(datetime(modified_time, 'unixepoch'), '{0} hours') as 'Modified Time(UTC{1})'", _hours, utc_time);
                }
                else
                {
                    result = String.Format("datetime(datetime(modified_time, 'unixepoch'), '{0} hours', '{1} minutes') as 'Modified Time(UTC{2})'", _hours, _min, utc_time);
                }
            }
            else if(t_name == "parse_location_history")
            {
                if (_min == "00")
                {
                    if (option == 0)
                    {
                        result = String.Format("datetime(datetime(timestamp / 1000, 'unixepoch'), '{0} hours') as 'Timestamp'", _hours);
                    }
                    else
                    {
                        result = String.Format("datetime(datetime(timestamp / 1000, 'unixepoch'), '{0} hours') as 'Timestamp(UTC{1})'", _hours, utc_time);
                    }
                }
                else
                {
                    if (option == 0)
                    {
                        result = String.Format("datetime(datetime(timestamp / 1000, 'unixepoch'), '{0} hours', '{1} minutes') as 'Timestamp'", _hours, _min);
                    }
                    else
                    {
                        result = String.Format("datetime(datetime(timestamp / 1000, 'unixepoch'), '{0} hours', '{1} minutes') as 'Timestamp(UTC{2})'", _hours, _min, utc_time);
                    }
                }
            }
            else if(t_name == "parse_semantic_location_history")
            {
                if (_min == "00")
                {
                    if (option == 0)
                    {
                        result = String.Format("datetime(datetime(stimestamp, 'unixepoch'), '{0} hours') as 'STime', datetime(datetime(etimestamp, 'unixepoch'), '{0} hours') as 'ETime'", _hours);
                    }
                    else
                    {
                        result = String.Format("datetime(datetime(stimestamp, 'unixepoch'), '{0} hours') as 'Start Time(UTC{1})', datetime(datetime(etimestamp, 'unixepoch'), '{0} hours') as 'End Time(UTC{1})'", _hours, utc_time);
                    }
                }
                else
                {
                    if (option == 0)
                    {
                        result = String.Format("datetime(datetime(stimestamp, 'unixepoch'), '{0} hours', '{1} minutes') as 'STime', datetime(datetime(etimestamp, 'unixepoch'), '{0} hours', '{1} minutes') as 'ETime", _hours, _min);
                    }
                    else
                    {
                        result = String.Format("datetime(datetime(stimestamp, 'unixepoch'), '{0} hours', '{1} minutes') as 'Start Time(UTC{2})', datetime(datetime(etimestamp, 'unixepoch'), '{0} hours', '{1} minutes') as 'End Time(UTC{2})'", _hours, _min, utc_time);
                    }
                }
            }
            else
            {
                if (_min == "00")
                {
                    result = String.Format("datetime(datetime(timestamp, 'unixepoch'), '{0} hours') as 'Timestamp(UTC{1})'", _hours, utc_time);
                }
                else
                {
                    result = String.Format("datetime(datetime(timestamp, 'unixepoch'), '{0} hours', '{1} minutes') as 'Timestamp(UTC{2})'", _hours, _min, utc_time);
                }
            }

            return result;
        }

        private string Query_Builder(string t_name)
        {
            string query = null;

            if (t_name == "parse_my_activity_android")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', package_name as 'Package Name', used_device as 'Used Device' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_assistant")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', result as 'Result', result_url as 'Result URL', latitude as Latitude, longitude as Longitude, geodata_description as 'Geodata Desc', filepath as 'File Path', used_device as 'Used Device' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_contacts")
            {
                query = String.Format("SELECT category as Category, name as Name, tel as 'Phone Number', email as 'E-Mail', photo as Photo, note as Note FROM {0};", t_name);
            }
            else if (t_name == "parse_drive")
            {
                query = String.Format("SELECT parentpath as 'Parent Path', filename as 'File Name', extension as Extension, {0}, bytes as 'Size (Bytes)', filepath as 'File Path' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_google_photo")
            {
                query = String.Format("SELECT parentpath as 'Parent Path', album_name as 'Album Name', filename as 'File Name', extension as Extension, bytes as 'Size (Bytes)', {0}, latitude as Latitude, longitude as Longitude, latitude_span as 'Latitude Span', longitude_span as 'Longitude Span', exif_latitude as 'Exif Latitude', exif_longitude as 'Exif Longitude', exif_latitude_span as 'Exif Latitude Span', exif_longitude_span as 'Exif Longitude Span', filepath as 'File Path' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_chrome")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', used_device as 'Used Device' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_map")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', keyword_latitude as 'Keyword Latitude', keyword_longitude as 'Keyword Longitude', latitude as Latitude, longitude as Longitude, geodata_description as 'Geodata Desc', used_device as 'Used Device' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_gmail")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_google_analytics")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', used_device as 'Used Device' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_location_history")
            {
                query = String.Format("SELECT {0}, latitude as Latitude, longitude as Longitude, altitude as Altitude, accuracy as Accuracy FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_youtube")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', channel_name as 'Channel Name', channel_url as 'Channel URL' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_video_search")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', used_device as 'Used Device' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_my_activity_voice_audio")
            {
                query = String.Format("SELECT {0}, service as Service, type as Type, keyword as Keyword, keyword_url as 'Keyword URL', filepath as 'File Path', used_device as 'Used Device' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }
            else if (t_name == "parse_semantic_location_history")
            {
                query = String.Format("SELECT type as Type, {0}, slatitude as 'Start Latitude', slongitude as 'Start Longitude', elatitude as 'End Latitude', elongitude as 'End Longitude', place_name as 'Place Name', place_addr as 'Place Address',duration as Duration, distance as Distance, transportation as Transportation, confidence as Confidence, device_tag as 'Device Tag' FROM {1};", Datetime_Builder(t_name, 1), t_name);
            }

            return query;
        }

        private void Load_DB()
        {
            OpenDatabase();

            string query = "SELECT * FROM sqlite_master WHERE type IN ('table', 'view') AND name NOT LIKE 'sqlite_%'";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            // Read SQLite Data Table List & Schema
            while (rdr.Read())
            {
                string t_name = rdr[rdr.GetName(1)].ToString();

                DataTable t_data = new DataTable();
                query = Query_Builder(t_name);
                SQLiteCommand _cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader _rdr = _cmd.ExecuteReader();

                t_data.Load(_rdr);
                tables.Add(t_name, t_data);
            }

            CloseDatabase();
        }
    }
}
