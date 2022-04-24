using System;
using System.Data;
using System.Data.SqlClient;

namespace Adv_AddressBook
{
    public class Program
    {
        static string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Addressbook;Integrated Security=SSPI";
        static SqlConnection connection = new SqlConnection(ConnectionStr);
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Advance Addressbook Program");
            Contacts contacts = new Contacts();
            connection.Open();
            connection.Close();
        }
    }
}