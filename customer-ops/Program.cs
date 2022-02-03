using Npgsql;

class Program
{
    #region DatabaseConnection
    private static string Host = "localhost";
    private static string User = "postgres";
    private static string DatabaseName = "<your_db_name>";
    private static string Password = "<your_db_password";
    #endregion
    #region CustomersTable
    private static string CustomerTable = "Customers";
    private static string CustomerColumn1 = "customerId";
    private static string CustomerColumn2 = "firstName";
    private static string CustomerColumn3 = "lastName";
    private static string CustomerColumn4 = "phoneNumber";
    #endregion

    public static async Task Main(String[] args)
    {
        var provider = $"Host={Host};Username={User};Password={Password};Database={DatabaseName}";

        // Create a connection
        await using var connection = new NpgsqlConnection(provider);
        Console.Out.WriteLine("Opening a connection to the database");
        connection.Open();

        // Check if the table exists
        await using (var command = new NpgsqlCommand($"DROP TABLE IF EXISTS {CustomerTable}", connection))
        {
            command.ExecuteNonQuery();
            Console.Out.WriteLine("Finished dropping table (if existed)");

        }

        // Create a table
        await using (var command = new NpgsqlCommand($"CREATE TABLE {CustomerTable}(id serial PRIMARY KEY, {CustomerColumn1} VARCHAR(50), {CustomerColumn2} VARCHAR(50), {CustomerColumn3} VARCHAR(50), {CustomerColumn4} VARCHAR(50))", connection))
        {
            command.ExecuteNonQuery();
            Console.Out.WriteLine("Finished creating table");
        }

        await AddCustomerData(connection, "1", "Emre", "Kılıç", "123456789");
        await AddCustomerData(connection, "2", "George", "Piker", "123785789");
        await AddCustomerData(connection, "3", "Ash", "Livington", "785456789");
        await ReadCustomerData(connection,"");
        await UpdateCustomerData(connection, "1", "Emre", "Quelich", "48954846546");
        await ReadCustomerData(connection, "");
        await RemoveCustomerData(connection, "2");
        await ReadCustomerData(connection, "");

    }


    public static async Task AddCustomerData(NpgsqlConnection conn, string c_id, string c_firstName, string c_lastName, string c_phoneNumber)
    {
        await using (var cmd = new NpgsqlCommand($"INSERT INTO {CustomerTable} ({CustomerColumn1}, {CustomerColumn2}, {CustomerColumn3}, {CustomerColumn4}) VALUES (@x1, @x2, @x3, @x4)", conn))
        {
            cmd.Parameters.AddWithValue("x1", c_id);
            cmd.Parameters.AddWithValue("x2", c_firstName);
            cmd.Parameters.AddWithValue("x3", c_lastName);
            cmd.Parameters.AddWithValue("x4", c_phoneNumber);
            cmd.ExecuteNonQuery();
            Console.Out.WriteLine("Successfully added the customer");
        };
    }

    public static async Task ReadCustomerData(NpgsqlConnection conn, string col)
    {
        var cmdCol = "*";
        if(col != "" && col != null)
        {
            cmdCol = col;
        }

        await using var cmd = new NpgsqlCommand($"SELECT {cmdCol} FROM {CustomerTable}", conn);


        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"{reader.GetString(1)}, {reader.GetString(2)}, {reader.GetString(3)}, {reader.GetString(4)}");
            }
        }
        
    }

    public static async Task UpdateCustomerData(NpgsqlConnection conn, string c_id, string c_firstName, string c_lastName, string c_phoneNumber)
    {
        var setFirstName = "";
        var setLastName = "";
        var setPhoneNumber = "";
        
        // Load the values if not empty
        if(c_firstName != "")
        {
            setFirstName = c_firstName;
        }
        if (c_lastName != "")
        {
            setLastName = c_lastName;
        }
        if (c_phoneNumber != "")
        {
            setPhoneNumber = c_phoneNumber;
        }

        await using (var cmd = new NpgsqlCommand($"UPDATE {CustomerTable} SET {CustomerColumn2} = @q2, {CustomerColumn3} = @q3, {CustomerColumn4} = @q4   WHERE {CustomerColumn1} = @q1", conn))
        {
            cmd.Parameters.AddWithValue("q1", c_id);
            cmd.Parameters.AddWithValue("q2", setFirstName);
            cmd.Parameters.AddWithValue("q3", setLastName);
            cmd.Parameters.AddWithValue("q4", setPhoneNumber);  
            cmd.ExecuteNonQuery();
            Console.Out.WriteLine("Successfully updated the customer");
        }
    }

    public static async Task RemoveCustomerData(NpgsqlConnection conn, string c_id)
    {
        
        await using (var command = new NpgsqlCommand($"DELETE FROM {CustomerTable} WHERE {CustomerColumn1} = @n", conn))
        {
            command.Parameters.AddWithValue("n", c_id);
            command.ExecuteNonQuery();
            Console.Out.WriteLine("Successfully removed the customer");
        }
    }

}