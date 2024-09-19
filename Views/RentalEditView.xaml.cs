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
    /// Interaction logic for RentalEditView.xaml
    /// </summary>
    public partial class RentalEditView : Window
    {
        private RentalTableViewModel _tableViewModel;

        public RentalEditView(RentalTableViewModel tableViewModel, Rental rental = null)
        {
            InitializeComponent();
            _tableViewModel = tableViewModel;
            RentalEditViewModel editModel= new RentalEditViewModel(this, rental);
            editModel.RentalListChanged += OnRentalListChanged;
            DataContext = editModel;
        }

        private void OnRentalListChanged()
        {
            _tableViewModel.UpdateTable();
        }

	}
}
