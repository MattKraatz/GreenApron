using System;
using SQLite;

namespace GreenApron
{
	public class User
	{
        [PrimaryKey]
		public Guid UserId { get; set; }

		public string Username { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Password { get; set; }

        public string ConfirmPassword { get; set; }

		public DateTime DateCreated { get; set; }
	}
}
