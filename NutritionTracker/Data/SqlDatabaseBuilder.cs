using System;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NutritionTracker.Data;

public class SqlDatabaseBuilder
{
    public string ConnectionString { get; }
    public SqlDatabaseBuilder(DataSource source) 
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
    }
    public void CreateDatabase()
    {
        CreateFoodTable();
        CreateNutrientsTable();
        CreateFoodNutrientsTable();
    }
    private void CreateFoodTable()
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Food " +
            "(Id INTEGER PRIMARY KEY, " +
            "Name TEXT NOT NULL, " +
            "Calories REAL, " +
            "Protein REAL," +
            "Carbs REAL," +
            "Fat REAL, " +
            "Fiber REAL)";
        command.ExecuteNonQuery();
    }
    private void CreateNutrientsTable()
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = "CREATE TABLE IF NOT EXISTS Nutrients " +
            "(Id INTEGER PRIMARY KEY, " +
            "Name TEXT NOT NULL, " +
            "Unit TEXT) ";
        command.ExecuteNonQuery();
    }
    private void CreateFoodNutrientsTable()
    {
        using SqliteConnection connection = new(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        //add in composite key
        command.CommandText = "CREATE TABLE IF NOT EXISTS Food_Nutrients " +
            "(FoodId INTEGER REFERENCES Food (Id) , " +
            "NutrientId INTEGER REFERENCES Nutrients (Id), " +
            "AmountPer100g REAL)";
        command.ExecuteNonQuery();
    }
}
