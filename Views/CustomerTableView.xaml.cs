﻿using CarRental.ViewModels;
using System;
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

namespace CarRental.Views
{
    /// <summary>
    /// Interaction logic for CustomerTableView.xaml
    /// </summary>
    public partial class CustomerTableView : Page
    {
        public CustomerTableView()
        {
            InitializeComponent();
            this.DataContext = new CustomerTableViewModel();
        }

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			CustomerTableViewModel? viewModel = DataContext as CustomerTableViewModel;
			if (viewModel != null && viewModel.EditCustomerCommand.CanExecute(null))
			{
				viewModel.EditCustomerCommand.Execute(null);
			}
		}
	}
}
