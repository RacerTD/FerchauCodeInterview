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
    /// Interaction logic for RentalTableView.xaml
    /// </summary>
    public partial class RentalTableView : Page
    {
        public RentalTableView()
        {
            InitializeComponent();
			this.DataContext = new RentalTableViewModel();
		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			RentalTableViewModel? viewModel = DataContext as RentalTableViewModel;
			if (viewModel != null && viewModel.EditRentalCommand.CanExecute(null))
			{
				viewModel.EditRentalCommand.Execute(null);
			}
		}
	}
}
