﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gtForensics
{
    /// <summary>
    /// _MBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class _MBox : Window
    {
        public _MBox(string msg)
        {
            InitializeComponent();

            msgBox.Text = msg;
        }

        private void OK_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
