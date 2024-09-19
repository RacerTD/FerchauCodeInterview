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
    public class CarTableViewModel : INotifyPropertyChanged
    {
		// Fields
		public ObservableCollection<Car> Cars { get; set; }
		public ObservableCollection<Car> FilteredCars { get; set; }

		// Properties
		private string _searchText;
		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				OnPropertyChanged(nameof(SearchText));
				FilterCars();
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
		public ICommand AddCarCommand { get; set; }
		public ICommand EditCarCommand { get; set; }

		// Constructor
		public CarTableViewModel()
		{
			Cars = new ObservableCollection<Car>();
			FilteredCars = new ObservableCollection<Car>(Cars);

			AddCarCommand = new RelayCommand(AddCar);
			EditCarCommand = new RelayCommand(EditCar, CanEditOrDeleteCar);

			UpdateTable();
		}

		// Methods
		public void UpdateTable()
		{
			DatabaseContext context = new DatabaseContext();
			Cars = new ObservableCollection<Car>(context.GetAllCars());
			FilterCars();
		}

		private void UpdateTable(object parameters)
		{
			UpdateTable();
		}

		private void FilterCars()
		{
			if (string.IsNullOrWhiteSpace(SearchText))
			{
				FilteredCars = new ObservableCollection<Car>(Cars);
			}
			else
			{
				FilteredCars = new ObservableCollection<Car>(
					Cars.Where(c => c.Make.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || c.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
				);
			}

			OnPropertyChanged(nameof(FilteredCars));
		}

		private void AddCar(object parameter)
		{
			var editWindow = new CarEditView(this);
			editWindow.ShowDialog();
		}

		private void EditCar(object parameter)
		{
			if (SelectedCar != null)
			{
				var editWindow = new CarEditView(this, SelectedCar);
				editWindow.ShowDialog();
			}
		}

		private bool CanEditOrDeleteCar(object parameter)
		{
			return SelectedCar != null;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
