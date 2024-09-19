using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace CarRental.DataBase
{
	public class DatabaseContext
	{
		private readonly string _connectionString;

		public DatabaseContext()
		{
			//Console.WriteLine("DatabaseContext: New DB Context");

			string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string dbFilePath = Path.Combine(exePath, "CarRental.db");
			_connectionString = $"Data Source={dbFilePath};Version=3;";

			if (!File.Exists(dbFilePath))
			{
				Console.WriteLine("DatabaseContext: Creating DB at: " + dbFilePath);
				SQLiteConnection.CreateFile(dbFilePath);
				CreateTables();
			}
		}

		private void CreateTables()
		{
			Console.WriteLine("DatabaseContext: Creating Tables");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();

			string createCustomerTableQuery = @"
            CREATE TABLE IF NOT EXISTS Customer (
                CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Contact TEXT
            );";

			string createCarTableQuery = @"
            CREATE TABLE IF NOT EXISTS Car (
                CarId INTEGER PRIMARY KEY AUTOINCREMENT,
                Make TEXT NOT NULL,
                Model TEXT NOT NULL,
                Year INTEGER NOT NULL,
                KilometersDriven REAL NOT NULL
            );";

			string createRentalTableQuery = @"
            CREATE TABLE IF NOT EXISTS Rental (
                RentalId INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerId INTEGER NOT NULL,
                CarId INTEGER NOT NULL,
                StartDate TEXT NOT NULL,
                EndDate TEXT NOT NULL,
				KilometersRented REAL NOT NULL,
                FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId),
                FOREIGN KEY (CarId) REFERENCES Car(CarId)
            );";

			using SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = createCustomerTableQuery;
			command.ExecuteNonQuery();

			command.CommandText = createCarTableQuery;
			command.ExecuteNonQuery();

			command.CommandText = createRentalTableQuery;
			command.ExecuteNonQuery();
		}

		#region Customer

		public void AddCustomer(Customer customer)
		{
			Console.WriteLine($"DatabaseContext: Creating customer {customer.Name}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string insertCustomerQuery = @"
                    INSERT INTO Customer (Name, Contact)
                    VALUES (@Name, @Contact);";

			using SQLiteCommand command = new SQLiteCommand(insertCustomerQuery, connection);
			command.Parameters.AddWithValue("@Name", customer.Name);
			command.Parameters.AddWithValue("@Contact", customer.Contact);
			command.ExecuteNonQuery();
		}

		public void UpdateCustomer(Customer customer)
		{
			Console.WriteLine($"DatabaseContext: Updating customer {customer.CustomerId}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string updateCustomerQuery = @"
                    UPDATE Customer
                    SET Name = @Name, Contact = @Contact
                    WHERE CustomerId = @CustomerId;";

			using SQLiteCommand command = new SQLiteCommand(updateCustomerQuery, connection);
			command.Parameters.AddWithValue("@Name", customer.Name);
			command.Parameters.AddWithValue("@Contact", customer.Contact);
			command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
			command.ExecuteNonQuery();
		}

		public void RemoveCustomer(int customerId)
		{
			List<Rental> customerRentals = GetRentalsByCustomerId(customerId);
			Console.WriteLine($"DatabaseContext: Deleting Customer {customerId}, need to remove {customerRentals.Count} rentals");
			if (customerRentals != null)
			{
				foreach (Rental rental in customerRentals)
				{
					RemoveRental(rental.RentalId);
				}
			}

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string deleteCustomerQuery = @"
                    DELETE FROM Customer WHERE CustomerId = @CustomerId;";

			using SQLiteCommand command = new SQLiteCommand(deleteCustomerQuery, connection);
			command.Parameters.AddWithValue("@CustomerId", customerId);
			command.ExecuteNonQuery();
		}

		public List<Customer> GetAllCustomers()
		{
			List<Customer> customers = new List<Customer>();

			using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Open();

				string sql = "SELECT * FROM Customer";
				using SQLiteCommand command = new SQLiteCommand(sql, connection);
				using SQLiteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					var customer = new Customer
					{
						CustomerId = Convert.ToInt32(reader["CustomerId"]),
						Name = reader["Name"].ToString(),
						Contact = reader["Contact"].ToString()
					};
					customers.Add(customer);
				}
			}

			Console.WriteLine($"DatabaseContext: Retrieving all {customers.Count} customers");

			return customers;
		}

		public Customer GetCustomer(int customerId)
		{
			Console.WriteLine($"DatabaseContext: Retrieving cusomter {customerId}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string selectCustomerQuery = @"
            SELECT * FROM Customer
            WHERE CustomerId = @CustomerId;";

			using SQLiteCommand command = new SQLiteCommand(selectCustomerQuery, connection);
			command.Parameters.AddWithValue("@CustomerId", customerId);

			using SQLiteDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				return new Customer
				{
					CustomerId = Convert.ToInt32(reader["CustomerId"]),
					Name = reader["Name"].ToString(),
					Contact = reader["Contact"].ToString()
				};
			}
			else
			{
				return null;
			}
		}

		#endregion Customer

		#region Car

		public void AddCar(Car car)
		{
			Console.WriteLine($"DatabaseContext: Creating car {car.MakeModel}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string insertCarQuery = @"
                    INSERT INTO Car (Make, Model, Year, KilometersDriven)
                    VALUES (@Make, @Model, @Year, @KilometersDriven);";

			using SQLiteCommand command = new SQLiteCommand(insertCarQuery, connection);
			command.Parameters.AddWithValue("@Make", car.Make);
			command.Parameters.AddWithValue("@Model", car.Model);
			command.Parameters.AddWithValue("@Year", car.Year);
			command.Parameters.AddWithValue("@KilometersDriven", car.KilometersDriven);
			command.ExecuteNonQuery();
		}

		public void UpdateCar(Car car)
		{
			Console.WriteLine($"DatabaseContext: Updating car {car.CarId}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string updateCarQuery = @"
                    UPDATE Car
                    SET Make = @Make, Model = @Model, Year = @Year, KilometersDriven = @KilometersDriven
                    WHERE CarId = @CarId;";

			using SQLiteCommand command = new SQLiteCommand(updateCarQuery, connection);
			command.Parameters.AddWithValue("@Make", car.Make);
			command.Parameters.AddWithValue("@Model", car.Model);
			command.Parameters.AddWithValue("@Year", car.Year);
			command.Parameters.AddWithValue("@KilometersDriven", car.KilometersDriven);
			command.Parameters.AddWithValue("@CarId", car.CarId);
			command.ExecuteNonQuery();
		}

		public void RemoveCar(int carId)
		{
			List<Rental> carRentals = GetRentalsByCarId(carId);
			Console.WriteLine($"Deleting Car {carId}, need to remove {carRentals.Count} rentals");
			if (carRentals != null)
			{
				foreach (Rental rental in carRentals)
				{
					RemoveRental(rental.RentalId);
				}
			}

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string deleteCarQuery = @"
                    DELETE FROM Car WHERE CarId = @CarId;";

			using SQLiteCommand command = new SQLiteCommand(deleteCarQuery, connection);
			command.Parameters.AddWithValue("@CarId", carId);
			command.ExecuteNonQuery();
		}

		public List<Car> GetAllCars()
		{
			List<Car> cars = new List<Car>();

			using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Open();

				string sql = "SELECT * FROM Car";
				using SQLiteCommand command = new SQLiteCommand(sql, connection);
				using SQLiteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					Car car = new Car
					{
						CarId = Convert.ToInt32(reader["CarId"]),
						Make = reader["Make"].ToString(),
						Model = reader["Model"].ToString(),
						Year = Convert.ToInt32(reader["Year"]),
						KilometersDriven = Convert.ToDouble(reader["KilometersDriven"])
					};
					cars.Add(car);
				}
			}

			Console.WriteLine($"DatabaseContext: Retrieving all {cars.Count} cars");

			return cars;
		}

		public Car GetCar(int carId)
		{
			Console.WriteLine($"DatabaseContext: Retrieving car {carId}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string selectCarQuery = @"
            SELECT * FROM Car
            WHERE CarId = @CarId;";

			using SQLiteCommand command = new SQLiteCommand(selectCarQuery, connection);
			command.Parameters.AddWithValue("@CarId", carId);

			using SQLiteDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				return new Car
				{
					CarId = Convert.ToInt32(reader["CarId"]),
					Make = reader["Make"].ToString(),
					Model = reader["Model"].ToString(),
					Year = Convert.ToInt32(reader["Year"]),
					KilometersDriven = Convert.ToDouble(reader["KilometersDriven"])
				};
			}
			else
			{
				return null;
			}
		}

		#endregion Car

		#region Rental

		public void AddRental(Rental rental)
		{
			Console.WriteLine($"DatabaseContext: Creating rental for customer: {rental.CustomerId} and car: {rental.CarId}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();

			string insertRentalQuery = @"
            INSERT INTO Rental (CustomerId, CarId, StartDate, EndDate, KilometersRented)
            VALUES (@CustomerId, @CarId, @StartDate, @EndDate, @KilometersRented);";

			using SQLiteCommand command = new SQLiteCommand(insertRentalQuery, connection);
			command.Parameters.AddWithValue("@CustomerId", rental.CustomerId);
			command.Parameters.AddWithValue("@CarId", rental.CarId);
			command.Parameters.AddWithValue("@StartDate", rental.StartDate.ToString("yyyy-MM-dd"));
			command.Parameters.AddWithValue("@EndDate", rental.EndDate.ToString("yyyy-MM-dd"));
			command.Parameters.AddWithValue("@KilometersRented", rental.KilometersRented);


			command.ExecuteNonQuery();
		}

		public void UpdateRental(Rental rental)
		{
			Console.WriteLine($"DatabaseContext: Updating rental {rental.CarId}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string updateRentalQuery = @"
                    UPDATE Rental
                    SET CustomerId = @CustomerId, CarId = @CarId, StartDate = @StartDate, EndDate = @EndDate, KilometersRented = @KilometersRented
                    WHERE RentalId = @RentalId;";

			using SQLiteCommand command = new SQLiteCommand(updateRentalQuery, connection);
			command.Parameters.AddWithValue("@CustomerId", rental.CustomerId);
			command.Parameters.AddWithValue("@CarId", rental.CarId);
			command.Parameters.AddWithValue("@StartDate", rental.StartDate.ToString("yyyy-MM-dd"));
			command.Parameters.AddWithValue("@EndDate", rental.EndDate.ToString("yyyy-MM-dd"));
			command.Parameters.AddWithValue("@KilometersRented", rental.KilometersRented);
			command.Parameters.AddWithValue("@RentalId", rental.RentalId);
			command.ExecuteNonQuery();
		}

		public void RemoveRental(int rentalId)
		{
			Console.WriteLine($"DatabaseContext: Removing rental {rentalId}");

			using SQLiteConnection connection = new SQLiteConnection(_connectionString);
			connection.Open();
			string deleteRentalQuery = @"
                    DELETE FROM Rental WHERE RentalId = @RentalId;";

			using SQLiteCommand command = new SQLiteCommand(deleteRentalQuery, connection);
			command.Parameters.AddWithValue("@RentalId", rentalId);
			command.ExecuteNonQuery();
		}

		public List<Rental> GetAllRentals()
		{
			List<Rental> rentals = new List<Rental>();

			using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Open();

				string sql = "SELECT * FROM Rental";
				using SQLiteCommand command = new SQLiteCommand(sql, connection);
				using SQLiteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					Rental rental = new Rental
					{
						RentalId = Convert.ToInt32(reader["RentalId"]),
						CustomerId = Convert.ToInt32(reader["CustomerId"]),
						CarId = Convert.ToInt32(reader["CarId"]),
						StartDate = DateTime.Parse(reader["StartDate"].ToString()),
						EndDate = DateTime.Parse(reader["EndDate"].ToString()),
						KilometersRented = Convert.ToDouble(reader["KilometersRented"])
					};
					rentals.Add(rental);
				}
			}

			Console.WriteLine($"DatabaseContext: Retrieving all {rentals.Count} rentals");

			return rentals;
		}

		public List<Rental> GetRentalsByCustomerId(int customerId)
		{
			List<Rental> rentals = new List<Rental>();

			using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Open();

				string selectRentalsQuery = @"
            SELECT * FROM Rental
            WHERE CustomerId = @CustomerId;";

				using SQLiteCommand command = new SQLiteCommand(selectRentalsQuery, connection);
				command.Parameters.AddWithValue("@CustomerId", customerId);

				using SQLiteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					Rental rental = new Rental
					{
						RentalId = Convert.ToInt32(reader["RentalId"]),
						CustomerId = Convert.ToInt32(reader["CustomerId"]),
						CarId = Convert.ToInt32(reader["CarId"]),
						StartDate = DateTime.Parse(reader["StartDate"].ToString()),
						EndDate = DateTime.Parse(reader["EndDate"].ToString()),
						KilometersRented = Convert.ToDouble(reader["KilometersRented"])
					};
					rentals.Add(rental);
				}
			}

			Console.WriteLine($"DatabaseContext: Retrieving all {rentals.Count} rentals for customer {customerId}");

			return rentals;
		}

		public List<Rental> GetRentalsByCarId(int carId)
		{
			List<Rental> rentals = new List<Rental>();

			using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Open();

				string selectRentalsQuery = @"
            SELECT * FROM Rental
            WHERE CarId = @CarId;";

				using SQLiteCommand command = new SQLiteCommand(selectRentalsQuery, connection);
				command.Parameters.AddWithValue("@CarId", carId);

				using SQLiteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					Rental rental = new Rental
					{
						RentalId = Convert.ToInt32(reader["RentalId"]),
						CustomerId = Convert.ToInt32(reader["CustomerId"]),
						CarId = Convert.ToInt32(reader["CarId"]),
						StartDate = DateTime.Parse(reader["StartDate"].ToString()),
						EndDate = DateTime.Parse(reader["EndDate"].ToString()),
						KilometersRented = Convert.ToDouble(reader["KilometersRented"])
					};
					rentals.Add(rental);
				}
			}

			Console.WriteLine($"DatabaseContext: Retrieving all {rentals.Count} rentals for car {carId}");

			return rentals;
		}

		#endregion Rental
	}
}
