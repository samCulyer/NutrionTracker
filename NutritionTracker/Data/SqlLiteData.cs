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

        string dbPath = Path.Combine(folderPath, dbName);
        ConnectionString = $"Data Source={dbPath};Cache=Shared;Foreign Keys=True";

        Directory.CreateDirectory(folderPath);
    }


    public async Task PopulateTestTable() 
    {
        await using SqliteConnection connection = new(ConnectionString);
        await connection.OpenAsync();

        string name = "mince";
        double weight = 100;

        await using var command = connection.CreateCommand();

        command.CommandText = "INSERT INTO Ingredients (Name, Weight) " +
            "VALUES (@name, @weight)";
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@weight", weight);
        await command.ExecuteNonQueryAsync();
    }
}
