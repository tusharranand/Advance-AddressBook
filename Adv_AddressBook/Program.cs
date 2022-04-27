using System;
using System.Data;
using System.Data.SqlClient;

namespace Adv_AddressBook
{
    public class Program
    {
        static string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Addressbook;Integrated Security=SSPI";
        static SqlConnection connection = new SqlConnection(ConnectionStr);
        string SPstr = "";

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Advance Addressbook Program");
            Program program = new Program();
            Contacts contact = new Contacts();
            connection.Open();
            int option = 1;
            do
            {
                Console.WriteLine("Choose an option");
                Console.WriteLine("1 to Insert in AddressBook");
                Console.WriteLine("2 to Display all Contacts");
                Console.WriteLine("3 to Update details of a contact that already exists");
                Console.WriteLine("4 to Delete a contact");
                Console.WriteLine("5 to Get contacts by city or state");
                Console.WriteLine("0 to EXIT");
                option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Console.Write("Enter the number of contacts you want to enter: ");
                        int count = Convert.ToInt32(Console.ReadLine());
                        for (int i = 0; i < count; i++)
                        {
                            program.InsertContact();
                        }
                        break;
                    case 2:
                        program.DisplayAllDetails();
                        break;
                    case 3:
                        program.UpdateDetails();
                        break;
                    case 4:
                        program.RemoveContact();
                        break;
                    case 5:
                        List<string> Names = program.ContactsByCityOrState();
                        foreach (string name in Names)
                        {
                            contact = program.GetDetailsForAName(name);
                            program.DisplayDetails(contact);
                        }
                        break;
                    default:
                        break;
                }
            } while (option != 0);
            connection.Close();
        }
        public void InsertContact()
        {
            Contacts contact = new Contacts();
            Console.WriteLine("\nFill in the details,");
            AddDetails(contact);
            InsertContact(contact);
            Console.WriteLine("Contact information for {0} {1} was saved to the database.\n",
                contact.FirstName, contact.LastName);
        }
        public Contacts AddDetails(Contacts contact)
        {
            Console.Write("Enter First Name: ");
            contact.FirstName = Console.ReadLine();
            if (contact.FirstName == "")
            {
                Console.WriteLine("First name can't be empty");
                return contact;
            }
            Console.Write("Enter Last Name: ");
            contact.LastName = Console.ReadLine();
            Console.Write("Enter Address: ");
            contact.Address = Console.ReadLine();
            Console.Write("Enter City: ");
            contact.City = Console.ReadLine();
            Console.Write("Enter State: ");
            contact.State = Console.ReadLine();
            Console.Write("Enter Zip Code: ");
            contact.ZipCode = Console.ReadLine();
            Console.Write("Enter Phone Number: ");
            contact.PhoneNumber = Console.ReadLine();
            Console.Write("Enter Email: ");
            contact.Email = Console.ReadLine();
            DisplayDetails(contact);
            return contact;
        }
        public void InsertContact(Contacts contact)
        {
            SPstr = "dbo.InsertContact";
            SqlCommand cmd = new SqlCommand(SPstr, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            InsertBasicDetails(cmd, contact);
        }
        public void InsertBasicDetails(SqlCommand cmd, Contacts contact)
        {
            cmd.Parameters.AddWithValue("@FirstName", contact.FirstName);
            cmd.Parameters.AddWithValue("@LastName", contact.LastName);
            cmd.Parameters.AddWithValue("@Address", contact.Address);
            cmd.Parameters.AddWithValue("@City", contact.City);
            cmd.Parameters.AddWithValue("@State", contact.State);
            cmd.Parameters.AddWithValue("@ZipCode", contact.ZipCode);
            cmd.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", contact.Email);
            cmd.ExecuteNonQuery();
        }
        public void DisplayAllDetails()
        {
            SPstr = "dbo.DisplayAllDetails";
            SqlCommand cmd = new SqlCommand(SPstr, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Contacts contact = new Contacts();
                    WriteToContactsClass(contact, reader);
                    DisplayDetails(contact);
                }
            }
            reader.Close();

        }
        public Contacts WriteToContactsClass(Contacts contact, SqlDataReader reader)
        {
            contact.FirstName = reader.GetString(0);
            contact.LastName = reader.GetString(1);
            contact.Address = reader.GetString(2);
            contact.City = reader.GetString(3);
            contact.State = reader.GetString(4);
            contact.ZipCode = reader.GetString(5);
            contact.PhoneNumber = reader.GetString(6);
            contact.Email = reader.GetString(7);
            return contact;
        }
        public void DisplayDetails(Contacts contact)
        {
            Console.WriteLine("\n-------------------------------------------------------");
            Console.WriteLine("The details for {0} {1} are:\nAddress: {2}\nCity: {3}\nState: " +
                "{4}\nZip Code: {5}\nPhone Number: {6}\nEmail: {7}", contact.FirstName,
                contact.LastName, contact.Address, contact.City, contact.State, contact.ZipCode,
                contact.PhoneNumber, contact.Email);
            Console.WriteLine("-------------------------------------------------------\n");

        }
        public int ContactExists(string FirstName)
        {
            SPstr = "dbo.ContactExists";
            SqlCommand cmd = new SqlCommand(SPstr, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            var returnValue = cmd.Parameters.Add("@result", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            return (int)returnValue.Value;
        }
        public Contacts GetDetailsForAName(string FirstName)
        {
            Contacts contact = new Contacts();
            SPstr = "dbo.AccessDetailsForFirstName";
            SqlCommand cmd = new SqlCommand(SPstr, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WriteToContactsClass(contact, reader);
                }
            }
            reader.Close();
            return contact;
        }
        public void UpdateDetails()
        {
            Console.Write("\nEnter the First Name: ");
            string FirstName = Console.ReadLine();
            if (ContactExists(FirstName) == 0)
            {
                Console.WriteLine("No such contact Exists.\n");
                return;
            }
            Console.WriteLine("Contact Exists, Enter the rest of details,");
            Contacts contact = new();
            SPstr = "dbo.UpdateDetails";
            SqlCommand cmd = new SqlCommand(SPstr, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OriginalFirstName", FirstName);
            AddDetails(contact);
            InsertBasicDetails(cmd, contact);
        }
        public void RemoveContact()
        {
            Console.Write("\nEnter the First Name: ");
            string FirstName = Console.ReadLine();
            if (ContactExists(FirstName) == 0)
            {
                Console.WriteLine("No such contact Exists.\n");
                return;
            }
            Console.WriteLine("Contact Exists.");
            SPstr = "dbo.RemoveContact";
            SqlCommand cmd = new SqlCommand(SPstr, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Contact with first name, {0} was deleted.\n", FirstName);
        }
        public List<string> ContactsByCityOrState()
        {
            List<string> FirstNames = new List<string>();
            Console.Write("\nSearch for City or State: ");
            string check = Console.ReadLine();
            Console.Write("Enter the name of {0}: ",check);
            string CityOrStateName = Console.ReadLine();
            int control;
            if (check.ToLower() == "city")
                control = 0;
            else if (check.ToLower() == "state")
                control = 1;
            else return null;
            SPstr = "dbo.ContactsByCityOrState";
            SqlCommand cmd = new SqlCommand(SPstr, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@City_State_Name", CityOrStateName);
            cmd.Parameters.AddWithValue("@City_or_State", control);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string FirstName = reader.GetString(0);
                    FirstNames.Add(FirstName);
                }
            }
            reader.Close();
            return FirstNames;
        }
    }
}