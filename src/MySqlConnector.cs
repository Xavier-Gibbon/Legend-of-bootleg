using System;
using System.Collections.Generic;
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
			username = "root";
			password = "Jaws7925";
			string connectionString = "SERVER=" + server + ";DATABASE=" + database + ";UID=" + username + ";PASSWORD=" + password + ";";

			connection = new MySqlConnection (connectionString);
		}

		public bool OpenConnection ()
		{
			try {
				connection.Open ();
				return true;
			} catch (MySqlException ex) {
				Console.WriteLine ("There was an error opening the connection. Exception: {0}", ex.Message);

				return false;
			}
		}

		public bool CloseConnection ()
		{
			try {
				connection.Close ();
				return true;
			} catch (MySqlException ex) {
				Console.WriteLine ("There was an error closing the connection. Exception: {0}", ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Executes a nonquery. Use this for statements that dont return a value
		/// </summary>
		/// <param name="command">The command.</param>
		public void NonQuery (string command)
		{
			MySqlCommand cmd = new MySqlCommand (command, connection);

			cmd.ExecuteNonQuery ();
		}

		/// <summary>
		/// Executes a select command. Use only for select statements
		/// </summary>
		/// <param name="command">Command.</param>
		public List<List<string>> Select (string command)
		{
			List<List<string>> result = new List<List<string>>();

			MySqlCommand cmd = new MySqlCommand (command, connection);
			MySqlDataReader dataReader = cmd.ExecuteReader ();

			for (int i = 0; i < dataReader.FieldCount; i++) {
				result.Add (new List<string> ());
			}

			while (dataReader.Read ()) {
				for (int i = 0; i < dataReader.FieldCount; i++) {
					result [i].Add (dataReader [i].ToString ());
				}
			}

			dataReader.Close ();



			return result;
		}
	}
}
