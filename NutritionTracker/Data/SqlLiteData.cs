using Avalonia.OpenGL;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NutritionTracker.Data;

public sealed class SqlLiteData
{

    private static string documentsPath =
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private static string folderPath =
        Path.Combine(documentsPath, "NutritionTrackerData");
    private static string dbPath = Path.Combine(folderPath, "NTDatabase.db");
    private static string connectionstring = $"Data Source={dbPath};Cache=Shared";

    private static readonly SqlLiteData _instance = new();
    public static SqlLiteData Instance => _instance;
    private SqlLiteData() 
    {
        Directory.CreateDirectory(folderPath);
        CreateDatabase();
    }

    private void CreateDatabase() 
    {
        //error use sqllite instead of sqllite.core? 
        //install Nuget Package Microsoft.Data.Sqlite(not Microsoft.Data.Sqlite.Core). (my version is 2.2.2)

        //    and use SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        //connection = new SqliteConnection("Data Source = Sample.db");

        //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        //connection.Open();
        //but I advise use nuget package System.Data.SQLite instead Microsoft.Data.Sqlite
        using SqliteConnection connection = new(connectionstring);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Ingredients " +
            "(Name VARCHAR(20), Weight DOUBLE)";
        command.ExecuteNonQuery();

        command.CommandText = "CREATE TABLE IF NOT EXISTS TestIngredients " +
            "(Name VARCHAR(20), Weight DOUBLE)";
        command.ExecuteNonQuery();
    }

    public async Task PopulateTestTable() 
    {
        await using SqliteConnection connection = new(connectionstring);
        await connection.OpenAsync();

        string name = "mince";
        double weight = 100;

        await using var command = connection.CreateCommand();

        command.CommandText = "INSERT INTO TestIngredients (Name, Weight) " +
            "VALUES (@name, @weight)";
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@weight", weight);
        await command.ExecuteNonQueryAsync();
    }
}
