using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Models
{
	public class Car
	{
		public int CarId { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int Year { get; set; }
		public double KilometersDriven { get; set; }
		public string MakeModel => Make + " " + Model;

		public override string ToString()
		{
			return $"Car ID: {CarId}, Make: {Make}, Model: {Model}, Year: {Year}, Kilometers Driven: {KilometersDriven}";
		}
	}
}
