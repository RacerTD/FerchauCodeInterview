using CarRental.DataBase;
using CarRental.Libs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarRental.ViewModels
{
    public class CustomerEditViewModel
    {
		// Fields
		private Window _window;

		// Properties
		private Customer _customer;
		public Customer Customer
		{
			get => _customer;
			set
			{
				_customer = value;
				OnPropertyChanged(nameof(Customer));
			}
		}

		private bool _isNewCustomer;
		public bool IsNewCustomer
		{
			get => _isNewCustomer;
			set
			{
				_isNewCustomer = value;
				OnPropertyChanged(nameof(IsNewCustomer));
			}
		}

		// Commands
		public ICommand SaveCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand CancelCommand { get; set; }

		// Actions
		public event Action CustomerListChanged;

		// Constructor
		public CustomerEditViewModel(Window window, Customer customer = null)
		{
			_window = window;

			if (customer == null)
			{
				Customer = new Customer();
				IsNewCustomer = true;
			}
			else
			{
				Customer = customer;
				IsNewCustomer = false;
			}

			SaveCommand = new RelayCommand(Save, CanSave);
			DeleteCommand = new RelayCommand(Delete);
			CancelCommand = new RelayCommand(Cancel);
		}

		// Methods
		private void Save(object parameter)
		{
			DatabaseContext context = new DatabaseContext();
			
			if (IsNewCustomer)
			{
				context.AddCustomer(Customer);
			}
			else
			{
				context.UpdateCustomer(Customer);
			}

			CustomerListChanged?.Invoke();

			CloseWindow();
		}

		private bool CanSave(object parameter)
		{
			return !string.IsNullOrEmpty(Customer.Name) && !string.IsNullOrEmpty(Customer.Contact);
		}

		private void Delete(object parameter)
		{
			if (!IsNewCustomer)
			{
				DatabaseContext context = new DatabaseContext();
				context.RemoveCustomer(Customer.CustomerId);
				CustomerListChanged?.Invoke();
			}

			CloseWindow();
		}

		private void Cancel(object parameter)
		{
			CloseWindow();
		}

		private void CloseWindow()
		{
			_window?.Close();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
