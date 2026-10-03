namespace Bison;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

public class DBFacade
{
    private readonly string connectionString;

    public DBFacade(string dbPath)
    {
        connectionString = $"Data source={dbPath}";
        if (new FileInfo(dbPath).Length == 0)
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            RunEmbeddedScript(connection, "schema.sql");
            RunEmbeddedScript(connection, "dump.sql");
            connection.Close();
        }
    }

    private static void RunEmbeddedScript(SqliteConnection connection, string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);

        var command = connection.CreateCommand();
        command.CommandText = reader.ReadToEnd();
        command.ExecuteNonQuery();
    }

    public List<ObservationViewModel> getObservations()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT u.username, o.text, o.pub_date
                                FROM observation o
                                JOIN user u ON o.author_id = u.user_id
                                ORDER BY o.pub_date DESC";

        using var reader = command.ExecuteReader();
        var result = new List<ObservationViewModel>();
        while (reader.Read())
        {
            string author = reader.GetString(0);
            string message = reader.GetString(1);
            string timestamp = reader.GetInt64(2).ToString();
            result.Add(new ObservationViewModel(author, message, timestamp));
        }
        connection.Close();
        return result;
    }

    public List<ObservationViewModel> getObservations(string author)
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT u.username, o.text, o.pub_date
                                FROM observation o
                                JOIN user u ON o.author_id = u.user_id
                                WHERE u.username = @author
                                ORDER BY o.pub_date DESC";
        command.Parameters.AddWithValue("@author", author);
        
        using var reader = command.ExecuteReader();
        var result = new List<ObservationViewModel>();
        while (reader.Read())
        {
            string username = reader.GetString(0);
            string message = reader.GetString(1);
            string timestamp = reader.GetInt64(2).ToString();
            result.Add(new ObservationViewModel(author, message, timestamp));
        }
        connection.Close();
        return result;
    }
    
    
}