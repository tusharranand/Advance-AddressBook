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
                Console.WriteLine("2 to Update details of a contact that already exists");
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
                        program.UpdateDetails();
                        break;
                    default:
                        break;
                }
            } while (option!=0);
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
    }
}