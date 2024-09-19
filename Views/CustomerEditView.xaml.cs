using CarRental.Models;
using CarRental.ViewModels;
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
using System.Windows.Shapes;

namespace CarRental.Views
{
    /// <summary>
    /// Interaction logic for CustomerEditView.xaml
    /// </summary>
    public partial class CustomerEditView : Window
    {
        private CustomerTableViewModel _tableViewModel;

        public CustomerEditView(CustomerTableViewModel customerTable, Customer customer = null)
        {
            InitializeComponent();

            _tableViewModel = customerTable;
            CustomerEditViewModel editModel = new CustomerEditViewModel(this, customer);
            editModel.CustomerListChanged += OnCustomerListChanged;
            DataContext = editModel;
        }

		private void OnCustomerListChanged()
		{
            _tableViewModel.UpdateTable();
		}
	}
}
