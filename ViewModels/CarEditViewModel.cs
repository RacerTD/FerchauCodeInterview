using CarRental.DataBase;
using CarRental.Libs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CarRental.ViewModels
{
    public class CarEditViewModel : INotifyPropertyChanged
    {
		// Fields
		private Window _window;

		// Properties
		private Car _car;
		public Car Car
		{
			get => _car;
			set
			{
				_car = value;
				OnPropertyChanged(nameof(Car));
			}
		}

		private bool _isNewCar;
		public bool IsNewCar
		{
			get => _isNewCar;
			set
			{
				_isNewCar = value;
				OnPropertyChanged(nameof(IsNewCar));
			}
		}

		// Commands
		public ICommand SaveCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand CancelCommand { get; set; }

		// Actions
		public event Action CarListChanged;

		// Constructor
		public CarEditViewModel(Window window, Car car = null)
		{
			_window = window;

			if (car == null)
			{
				Car = new Car();
				IsNewCar = true;
			}
			else
			{
				Car = car;
				IsNewCar = false;
			}

			SaveCommand = new RelayCommand(Save, CanSave);
			DeleteCommand = new RelayCommand(Delete);
			CancelCommand = new RelayCommand(Cancel);
		}

		// Methods
		private void Save(object parameter)
		{
			DatabaseContext context = new DatabaseContext();

			if (IsNewCar)
			{
				context.AddCar(Car);
			}
			else
			{
				context.UpdateCar(Car);
			}

			CarListChanged?.Invoke();

			CloseWindow();
		}

		private bool CanSave(object parameter)
		{
			return !string.IsNullOrEmpty(Car.Make) && !string.IsNullOrEmpty(Car.Model);
		}

		private void Delete(object parameter)
		{
			if (!IsNewCar)
			{
				DatabaseContext context = new DatabaseContext();
				context.RemoveCar(Car.CarId);
				CarListChanged?.Invoke();
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
