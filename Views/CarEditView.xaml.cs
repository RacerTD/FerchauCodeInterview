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
    /// Interaction logic for CarEditView.xaml
    /// </summary>
    public partial class CarEditView : Window
    {
        private CarTableViewModel _tableViewModel;

        public CarEditView(CarTableViewModel carTable, Car car = null)
        {
            InitializeComponent();

            _tableViewModel = carTable;
            CarEditViewModel editModel = new CarEditViewModel(this, car);
            editModel.CarListChanged += OnCarListChanged;
            DataContext = editModel;
        }

        private void OnCarListChanged()
        {
            _tableViewModel.UpdateTable();
        }
    }
}
