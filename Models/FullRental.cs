using CarRental.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class FullRental
    {
        public Rental Rental { get; set; }
        public Customer Customer { get; set; }
        public Car Car { get; set; }

        public bool IsComplete => Rental != null && Customer != null && Car != null;

        public FullRental(Rental rental) 
        {
            Rental = rental;

            DatabaseContext context = new DatabaseContext();
            Customer = context.GetCustomer(Rental.CustomerId);
			Car = context.GetCar(Rental.CarId);

            if (Customer == null)
            {
                Console.WriteLine($"Rental {Rental.RentalId} is missing its Customer: {Rental.CustomerId}");
            }

            if (Car == null)
            {
                Console.WriteLine($"Rental {Rental.RentalId} is missing its Car: {Rental.CarId}");
            }
        }

		public FullRental(Rental rental, Customer customer, Car car)
		{
            Rental = rental;
            Customer = customer;
            Car = car;
		}
	}
}
