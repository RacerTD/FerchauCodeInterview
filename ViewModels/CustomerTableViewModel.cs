using CarRental.DataBase;
using CarRental.Libs;
using CarRental.Models;
using CarRental.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarRental.ViewModels
{
    public class CustomerTableViewModel : INotifyPropertyChanged
    {
		// Fields
		public ObservableCollection<Customer> Customers { get; set; }
		public ObservableCollection<Customer> FilteredCustomers { get; set; }

		// Properties
		private string _searchText;
		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				OnPropertyChanged(nameof(SearchText));
				FilterCustomers();
			}
		}

		private Customer _selectedCustomer;
		public Customer SelectedCustomer
		{
			get => _selectedCustomer;
			set
			{
				_selectedCustomer = value;
				OnPropertyChanged(nameof(SelectedCustomer));
			}
		}

		// Commands
		public ICommand AddCustomerCommand { get; set; }
		public ICommand EditCustomerCommand { get; set; }
		public ICommand RefreshTableCommand { get; set; }

		// Actions
		public event PropertyChangedEventHandler PropertyChanged;

		// Constructor
		public CustomerTableViewModel()
		{
			Customers = new ObservableCollection<Customer>();
			FilteredCustomers = new ObservableCollection<Customer>(Customers);

			AddCustomerCommand = new RelayCommand(AddCustomer);
			EditCustomerCommand = new RelayCommand(EditCustomer, CanEditOrDeleteCustomer);
			RefreshTableCommand = new RelayCommand(UpdateTable);

			UpdateTable();
		}

		// Methods
		public void UpdateTable()
		{
			DatabaseContext context = new DatabaseContext();
			Customers = new ObservableCollection<Customer>(context.GetAllCustomers());
			FilterCustomers();
		}

		private void UpdateTable(object parameters)
		{
			UpdateTable();
		}

		private void FilterCustomers()
		{
			if (string.IsNullOrWhiteSpace(SearchText))
			{
				FilteredCustomers = new ObservableCollection<Customer>(Customers);
			}
			else
			{
				FilteredCustomers = new ObservableCollection<Customer>(
					Customers.Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
				);
			}

			OnPropertyChanged(nameof(FilteredCustomers));
		}

		private void AddCustomer(object parameter)
		{
			CustomerEditView editWindow = new CustomerEditView(this);
			editWindow.ShowDialog();
		}

		private void EditCustomer(object parameter)
		{
			if (SelectedCustomer != null)
			{
				CustomerEditView editWindow = new CustomerEditView(this, SelectedCustomer);
				editWindow.ShowDialog();
			}
		}

		private bool CanEditOrDeleteCustomer(object parameter)
		{
			return SelectedCustomer != null;
		}


		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
