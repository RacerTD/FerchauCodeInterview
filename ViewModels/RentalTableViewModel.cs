using CarRental.DataBase;
using CarRental.Libs;
using CarRental.Models;
using CarRental.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarRental.ViewModels
{
    public class RentalTableViewModel : INotifyPropertyChanged
    {
        // Fields
        public ObservableCollection<FullRental> Rentals { get; set; }
        public ObservableCollection<FullRental> FilteredRentals { get; set; }

        // Properties
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
				FilterRentals();
            }
        }

        private FullRental _selectedRental;
        public FullRental SelectedRental
        {
            get => _selectedRental;
            set
            {
                _selectedRental = value;
                OnPropertyChanged(nameof(SelectedRental));
            }
        }

        // Commands
        public ICommand AddRentalCommand { get; set; }
        public ICommand EditRentalCommand { get; set; }
        public ICommand RefreshTableCommand { get; set; }

        // Constructor
        public RentalTableViewModel()
        {
            Rentals = new ObservableCollection<FullRental>();
            FilteredRentals = new ObservableCollection<FullRental>();

            AddRentalCommand = new RelayCommand(AddRental);
            EditRentalCommand = new RelayCommand(EditRental, CanEditOrDeleteRental);
            RefreshTableCommand = new RelayCommand(UpdateTable);

            UpdateTable();
        }

        // Methods
        public void UpdateTable()
        {
            DatabaseContext context = new DatabaseContext();

            List<Rental> rentals = context.GetAllRentals();
            List<FullRental> fullRentals = new List<FullRental>();

            foreach (Rental rental in rentals)
            {
                FullRental full = new FullRental(rental);
                if (full.IsComplete)
                {
                    fullRentals.Add(full);
                }
            }

            Rentals = new ObservableCollection<FullRental>(fullRentals);

            FilterRentals();
        }

        private void UpdateTable(object parameters)
        {
            UpdateTable();
        }

        private void FilterRentals()
        {
			if (string.IsNullOrWhiteSpace(SearchText))
			{
				FilteredRentals = new ObservableCollection<FullRental>(Rentals);
			}
			else
			{
				FilteredRentals = new ObservableCollection<FullRental>(
					Rentals.Where(c => c.Car.MakeModel.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || c.Customer.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
				);
			}

			OnPropertyChanged(nameof(FilteredRentals));
		}

        private void AddRental(object parameter)
        {
            RentalEditView editWindow = new RentalEditView(this);
            editWindow.ShowDialog();
        }

		private void EditRental(object parameter)
		{
			if (SelectedRental != null)
			{
                RentalEditView editWindow = new RentalEditView(this, SelectedRental.Rental);
                editWindow.ShowDialog();
            }
		}

		private bool CanEditOrDeleteRental(object parameter)
		{
			return SelectedRental != null;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
