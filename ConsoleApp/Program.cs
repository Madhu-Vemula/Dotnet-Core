
using Microsoft.Data.SqlClient;

namespace ConsoleApp
{
    public class Program
    {
        static void CreateEmployeesTable(SqlConnection connection)
        {
            string sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
            BEGIN
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
                BEGIN
                    CREATE TABLE Roles (
                        RoleID INT PRIMARY KEY,
                        RoleName NVARCHAR(50) NOT NULL
                    );
                    INSERT INTO Roles VALUES (1, 'Admin'), (2, 'User');
                END

                CREATE TABLE Employees (
                    EmployeeID INT PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(100) UNIQUE NOT NULL,
                    RoleID INT NOT NULL FOREIGN KEY REFERENCES Roles(RoleID),
                    ManagerID INT NULL FOREIGN KEY REFERENCES Employees(EmployeeID),
                    Password NVARCHAR(100) NOT NULL
                );
                SELECT 'Tables created successfully' AS Result;
            END
            ELSE
            BEGIN
                SELECT 'Tables already exist' AS Result;
            END";

            using (var cmd = new SqlCommand(sql, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["Result"]);
                    }
                }
            }
        }

        public static void InsertRecord()
        {
            SqlConnection? con = null;
            try
            {
                con = new SqlConnection("Server=.\\SQLEXPRESS02;Database=Demo;Integrated Security=True;TrustServerCertificate=True;");
                con.Open();

                //         // 1. First create the Employees table if it doesn't exist
                //         string createTableSql = @"
                // IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employees')
                // BEGIN
                //     CREATE TABLE Employees (
                //         EmployeeID INT PRIMARY KEY,
                //         Name NVARCHAR(100) NOT NULL,
                //         Email NVARCHAR(100) NOT NULL
                //     );
                //     PRINT 'Employees table created successfully.';
                // END";

                //         new SqlCommand(createTableSql, con).ExecuteNonQuery();

                // 2. Now insert the record
                string insertSql = @"
        INSERT INTO Employees (EmployeeID, Name, Email)
        VALUES (@Id, @Name, @Email)";

                SqlCommand cm = new SqlCommand(insertSql, con);
                cm.Parameters.AddWithValue("@Id", 2);
                cm.Parameters.AddWithValue("@Name", "Madhu Vemula");
                cm.Parameters.AddWithValue("@Email", "madhu.vemula@pal.tech");

                cm.ExecuteNonQuery();
                Console.WriteLine("Record Inserted Successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                con?.Close();
            }
        }
        public static void DisplayData()
        {
            SqlConnection? con = null;
            try
            {

                con = new SqlConnection("Server=.\\SQLEXPRESS02;Database=Demo;Integrated Security=True;TrustServerCertificate=True;");

                SqlCommand cm = new SqlCommand("Select * from employees", con);

                con.Open();

                SqlDataReader sdr = cm.ExecuteReader();

                while (sdr.Read())
                {
                    Console.WriteLine(sdr["EmployeeID"] + " " + sdr["name"] + " " + sdr["email"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("OOPs, something went wrong." + e);
            }
            // Closing the connection  
            finally
            {
                con?.Close();
            }
        }
        public static void Main(string[] args)
        {
            var obj = new Program();

            TestConnection();
            // InsertRecord(); 
            DisplayData();
            Console.WriteLine("Hello World!");
        }

        static void TestConnection()
        {
            string connectionString = "Server=.\\SQLEXPRESS02;Database=master;Integrated Security=True;TrustServerCertificate=True;";

            try
            {
                // // 1. Ensure database exists
                // using (var connection = new SqlConnection(connectionString))
                // {
                //     connection.Open();
                //     var cmd = new SqlCommand(
                //         "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Demo') " +
                //         "CREATE DATABASE Demo;", connection);
                //     cmd.ExecuteNonQuery();
                //     Console.WriteLine("Verified/Created Demo database");
                // }

                // 2. Now connect to Demo database
                connectionString = "Server=.\\SQLEXPRESS02;Database=Demo;Integrated Security=True;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("✅ Successfully connected to Demo database!");
                    Console.WriteLine($"Server: {connection.DataSource}");
                    Console.WriteLine($"Version: {connection.ServerVersion}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
