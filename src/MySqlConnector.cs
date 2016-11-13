using System;
using MySql.Data.MySqlClient;
namespace MyGame
{
	public class MySqlConnector
	{
		private MySqlConnection connection;
		private string server;
		private string database;
		private string username;
		private string password;

		public MySqlConnector ()
		{
			server = "localhost";
			database = "TLoBDatabase";
			username = "game";
			password = "password";
			string connectionString = "SERVER=" + server + ";DATABASE=" + database + ";UID=" + username + ";PASSWORD=" + password + ";";

			connection = new MySqlConnection (connectionString);
		}
	}
}
