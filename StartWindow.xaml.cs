using Microsoft.Win32;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace gtForensics
{
    /// <summary>
    /// StartWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();

            // Set Combobox
            SetUTCCombo();
        }

        private void SetUTCCombo()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Timezone", typeof(string));

            dt.Rows.Add("-12:00");
            dt.Rows.Add("-11:00");
            dt.Rows.Add("-10:00");
            dt.Rows.Add("-09:30");
            dt.Rows.Add("-09:00");
            dt.Rows.Add("-08:00");
            dt.Rows.Add("-07:00");
            dt.Rows.Add("-06:00");
            dt.Rows.Add("-05:00");
            dt.Rows.Add("-04:00");
            dt.Rows.Add("-03:30");
            dt.Rows.Add("-03:00");
            dt.Rows.Add("-02:00");
            dt.Rows.Add("-01:00");
            dt.Rows.Add("+00:00");
            dt.Rows.Add("+01:00");
            dt.Rows.Add("+02:00");
            dt.Rows.Add("+03:00");
            dt.Rows.Add("+03:30");
            dt.Rows.Add("+04:00");
            dt.Rows.Add("+04:30");
            dt.Rows.Add("+05:00");
            dt.Rows.Add("+05:30");
            dt.Rows.Add("+05:45");
            dt.Rows.Add("+06:00");
            dt.Rows.Add("+07:00");
            dt.Rows.Add("+08:00");
            dt.Rows.Add("+08:45");
            dt.Rows.Add("+09:00");
            dt.Rows.Add("+09:30");
            dt.Rows.Add("+10:00");
            dt.Rows.Add("+10:30");
            dt.Rows.Add("+11:00");
            dt.Rows.Add("+12:00");
            dt.Rows.Add("+12:45");
            dt.Rows.Add("+13:00");
            dt.Rows.Add("+14:30");

            TimezoneList.ItemsSource = dt.DefaultView;
            TimezoneList.DisplayMemberPath = "Timezone";
            TimezoneList.SelectedValuePath = "Timezone";
            TimezoneList.SelectedIndex = 14;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "SQLite Files (*.db;*.sqlite)|*.db;*.sqlite";
            ofd.ShowDialog();

            if (ofd.FileName.Length > 0)
            {
                dbpath.Text = ofd.FileName;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            /*
             * 입력 예외처리
             * 
             * dbpath
             * apikey
             * 
             */

            if (string.IsNullOrEmpty(dbpath.Text) && string.IsNullOrEmpty(apikey.Text))
            {
                _MBox mbox = new _MBox("Please enter the Database Path and API Key!");
                mbox.Show();
            }
            else if (string.IsNullOrEmpty(dbpath.Text))
            {
                _MBox mbox = new _MBox("Please enter the Database Path!");
                mbox.Show();
            }
            else if(string.IsNullOrEmpty(apikey.Text))
            {
                _MBox mbox = new _MBox("Please enter the API Key!");
                mbox.Show();
            }
            else
            {
                MainWindow mw = new MainWindow(dbpath.Text, TimezoneList.Text, apikey.Text);
                mw.Show();
                this.Close();
            }
        }
    }
}
