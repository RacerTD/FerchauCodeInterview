using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Models
{
	public class Customer
	{
		public int CustomerId { get; set; }
		public string Name { get; set; }
		public string Contact { get; set; }

		public override string ToString()
		{
			return $"Customer ID: {CustomerId}, Name: {Name}, Contact: {Contact}";
		}
	}
}
