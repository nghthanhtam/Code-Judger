﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Judge3T
{
    /// <summary>
    /// Interaction logic for UserControl_HuongDan.xaml
    /// </summary>
    public partial class UserControl_HuongDan : UserControl
    {
        public UserControl_HuongDan()
        {
            InitializeComponent();
        }

        MainWindow MainWin;

        public UserControl_HuongDan(MainWindow x)
        {
            InitializeComponent();
            MainWin = x;
            Win.Width = Double.NaN;
            Win.Height = Double.NaN;

        }

    }
}
