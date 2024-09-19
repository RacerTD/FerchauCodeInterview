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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarRental.Views
{
    /// <summary>
    /// Interaction logic for CarTableView.xaml
    /// </summary>
    public partial class CarTableView : Page
    {
        public CarTableView()
        {
            InitializeComponent();
            this.DataContext = new CarTableViewModel();
        }

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			CarTableViewModel? viewModel = DataContext as CarTableViewModel;
			if (viewModel != null && viewModel.EditCarCommand.CanExecute(null))
			{
				viewModel.EditCarCommand.Execute(null);
			}
		}
	}
}
