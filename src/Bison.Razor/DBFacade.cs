namespace Bison;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

public class DBFacade
{
    private readonly string connectionString;

    private const int PageSize = 32;

    public DBFacade(string dbPath)
    {
        File.Create(dbPath).Dispose();
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

    public List<ObservationViewModel> getObservations(int page)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT o.observation_id, u.username, o.text, o.pub_date
                                FROM observation o
                                JOIN user u ON o.author_id = u.user_id
                                ORDER BY o.pub_date DESC
                                LIMIT @pageSize OFFSET @offset";
        command.Parameters.AddWithValue("@pageSize", PageSize);
        command.Parameters.AddWithValue("@offset", (Math.Max(page, 1) - 1) * PageSize);

        using var reader = command.ExecuteReader();
        var result = new List<ObservationViewModel>();
        while (reader.Read())
        {
            long observationId = reader.GetInt64(0);
            string author = reader.GetString(1);
            string message = reader.GetString(2);
            string timestamp = reader.GetInt64(3).ToString();
            result.Add(new ObservationViewModel(observationId, author, message, timestamp));
        }
        connection.Close();
        return result;
    }

    public List<ObservationViewModel> getObservations(string author, int page)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT o.observation_id, u.username, o.text, o.pub_date
                                FROM observation o
                                JOIN user u ON o.author_id = u.user_id
                                WHERE u.username = @author
                                ORDER BY o.pub_date DESC
                                LIMIT @pageSize OFFSET @offset";
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("@pageSize", PageSize);
        command.Parameters.AddWithValue("@offset", (Math.Max(page, 1) - 1) * PageSize);
        
        using var reader = command.ExecuteReader();
        var result = new List<ObservationViewModel>();
        while (reader.Read())
        {
            long observationId = reader.GetInt64(0);
            string username = reader.GetString(1);
            string message = reader.GetString(2);
            string timestamp = reader.GetInt64(3).ToString();
            result.Add(new ObservationViewModel(observationId, username, message, timestamp));
        }
        connection.Close();
        return result;
    }

    public ObservationViewModel? getObservationFromId(long? id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT o.observation_id, u.username, o.text, o.pub_date  
                                FROM observation o 
                                JOIN user u ON o.author_id = u.user_id
                                WHERE o.observation_id = @id";
        command.Parameters.AddWithValue("@id", id);


        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;
        long observationId = reader.GetInt64(0);
        string author = reader.GetString(1);
        string message = reader.GetString(2);
        string timestamp = reader.GetInt64(3).ToString();
        return new ObservationViewModel(observationId, author, message, timestamp);
    }

    // 
    public List<CommentViewModel> GetCommentViewModels(long? observation_Id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT c.comment_id, c.observation_id, u.username, c.text, c.pub_date
                                FROM comment c
                                JOIN user u ON c.author_id = u.user_id
                                WHERE c.observation_id = @observation_Id
                                ORDER BY c.pub_date DESC";

        command.Parameters.AddWithValue("@observation_Id", observation_Id);
        

        using var reader = command.ExecuteReader();
        var result = new List<CommentViewModel>();
        while (reader.Read())
        {
            long comment_id = reader.GetInt64(0);
            long observationId = reader.GetInt64(1);
            string author = reader.GetString(2);
            string message = reader.GetString(3);
            string timestamp = reader.GetInt64(4).ToString();
            result.Add(new CommentViewModel(comment_id, observationId, author, message, timestamp));
        }

        connection.Close();
        return result;
    }

    public List<ProposalViewModel> GetProposalViewModels(long? observation_Id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT p.proposal_id, p.observation_id, u.username, p.taxonId, p.pub_date
                                FROM proposal p
                                JOIN user u ON p.author_id = u.user_id
                                WHERE p.observation_id = @observation_Id
                                ORDER BY p.pub_date DESC";

        command.Parameters.AddWithValue("@observation_Id", observation_Id);


        using var reader = command.ExecuteReader();
        var result = new List<ProposalViewModel>();
        while (reader.Read())
        {
            long proposal_Id = reader.GetInt64(0);
            long observationId = reader.GetInt64(1);
            string author = reader.GetString(2);
            string taxonId = reader.GetString(3);
            string pub_data = reader.GetInt64(4).ToString();
            result.Add(new ProposalViewModel(proposal_Id,observationId,author,taxonId,pub_data));
        }

        connection.Close();
        return result;
    }
    
    
    
}