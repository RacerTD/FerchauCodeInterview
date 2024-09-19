using CarRental.DataBase;
using CarRental.Libs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarRental.ViewModels
{
	public class RentalEditViewModel : INotifyPropertyChanged
	{
		// Fields
		private Window _window;

		private ObservableCollection<Rental> _rentals { get; set; }

		// Properties

		private DateTime _startDate;
		public DateTime StartDate
		{
			get => _startDate;
			set
			{
				_startDate = value;
				OnPropertyChanged(nameof(StartDate));
			}
		}

		private DateTime _endDate;
		public DateTime EndDate
		{
			get => _endDate;
			set
			{
				_endDate = value;
				OnPropertyChanged(nameof(EndDate));
			}
		}

		private double _kilometersRented;
		public double KilometersRented
		{
			get => _kilometersRented;
			set
			{
				_kilometersRented = value;
				OnPropertyChanged(nameof(KilometersRented));
			}
		}

		private ObservableCollection<Customer> _customers;
		public ObservableCollection<Customer> Customers
		{
			get => _customers;
			set
			{
				_customers = value;
				OnPropertyChanged(nameof(Customers));
			}
		}

		private ObservableCollection<Car> _cars;
		public ObservableCollection<Car> Cars
		{
			get => _cars;
			set
			{
				_cars = value;
				OnPropertyChanged(nameof(Cars));
			}
		}

		private Rental _rental;
		public Rental Rental
		{
			get => _rental;
			set
			{
				_rental = value;
				OnPropertyChanged(nameof(Rental));
			}
		}

		private bool _isNewRental;
		public bool IsNewRental
		{
			get => _isNewRental;
			set
			{
				_isNewRental = value;
				OnPropertyChanged(nameof(IsNewRental));
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

		private Car _selectedCar;
		public Car SelectedCar
		{
			get => _selectedCar;
			set
			{
				_selectedCar = value;
				OnPropertyChanged(nameof(SelectedCar));
			}
		}

		// Commands
		public ICommand SaveCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand CancelCommand { get; set; }

		// Actions
		public event Action RentalListChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		// Constructor
		public RentalEditViewModel(Window window, Rental rental)
		{
			_window = window;

			DatabaseContext context = new DatabaseContext();

			_rentals = new ObservableCollection<Rental>(context.GetAllRentals());
			Customers = new ObservableCollection<Customer>(context.GetAllCustomers());
			Cars = new ObservableCollection<Car>(context.GetAllCars());

			if (rental == null)
			{
				Rental = new Rental();
				IsNewRental = true;
				StartDate = DateTime.Today;
				EndDate = DateTime.Today;
			}
			else
			{
				Rental = rental;
				IsNewRental = false;
				StartDate = Rental.StartDate;
				EndDate = Rental.EndDate;
				SelectedCar = Cars.First(c => c.CarId == Rental.CarId);
				SelectedCustomer = Customers.First(c => c.CustomerId == Rental.CustomerId);
				KilometersRented = Rental.KilometersRented;
			}

			SaveCommand = new RelayCommand(Save, CanSave);
			DeleteCommand = new RelayCommand(Delete);
			CancelCommand = new RelayCommand(Cancel);
		}

		// Methods
		private void Save(object parameters)
		{
			Rental.StartDate = StartDate;
			Rental.EndDate = EndDate;
			Rental.CarId = SelectedCar.CarId;
			Rental.CustomerId = SelectedCustomer.CustomerId;
			Rental.KilometersRented = KilometersRented;

			DatabaseContext context = new DatabaseContext();

			if (IsNewRental)
			{
				context.AddRental(Rental);
			}
			else
			{
				context.UpdateRental(Rental);
			}

			RentalListChanged?.Invoke();

			CloseWindow();
		}

		private bool CanSave(object parameters)
		{
			if (StartDate > EndDate
				|| SelectedCar == null
				|| SelectedCustomer == null)
			{
				return false;
			}

			return true;
		}

		private void Delete(object parameters)
		{
			if (!IsNewRental)
			{
				DatabaseContext context = new DatabaseContext();
				context.RemoveRental(Rental.RentalId);
				RentalListChanged?.Invoke();
			}

			CloseWindow();
		}

		private void Cancel(object parameters)
		{
			CloseWindow();
		}

		private void CloseWindow()
		{
			_window?.Close();
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
