using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NETTEST2.Models.Offers;
using NETTEST2.Models.Cars;

namespace NETTEST2.Models.Context
{
	public class UserDataContext : DbContext
	{
		public DbSet<Offer> Offers { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<CarDetails> CarsDetails { get; set; }
		public DbSet<ImageModel> ImageModels { get; set; }
	}
}