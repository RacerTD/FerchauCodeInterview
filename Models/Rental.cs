using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Models
{
	public class Rental
	{
		public int RentalId { get; set; }
		public int CustomerId { get; set; }
		public int CarId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public double KilometersRented { get; set; }
	}
}
