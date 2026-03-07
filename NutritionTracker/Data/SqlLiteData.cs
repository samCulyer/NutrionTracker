using Avalonia.OpenGL;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NutritionTracker.Data;

public sealed class SqlLiteData
{
    public string ConnectionString { get; }

    public SqlLiteData(DataSource source) 
    {
        string documentsPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string folderPath =
            Path.Combine(documentsPath, "NutritionTrackerData");

        string dbName = source == DataSource.Test
            ? "TestDatabase.db"
            : "Database.db";

        string dbPath = Path.Combine(folderPath, "Database.db");
        ConnectionString = $"Data Source={dbPath};Cache=Shared";

        Directory.CreateDirectory(folderPath);
        CreateDatabase();
    }

    private void CreateDatabase() 
    {
        using SqliteConnection connection = new(ConnectionString);
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
        await using SqliteConnection connection = new(ConnectionString);
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
