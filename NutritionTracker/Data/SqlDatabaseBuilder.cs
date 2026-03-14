using System;
using Microsoft.Data.Sqlite;
using System.IO;

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
        //diet columns
        command.CommandText = "CREATE TABLE IF NOT EXISTS Food " +
                              "(Id INTEGER PRIMARY KEY, " +
                              "Name TEXT NOT NULL) ";
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
        //milligram 1 thousandth of a gram mg
        //micrograms μg or mcg 1 millionth of a gram 
        //international units ui conversion depends on type of vitamin
        // 1 mg = 1000 mcg
        command.CommandText = "INSERT INTO Ingredients (Id, Name, Unit) " +
                              "VALUES (1, Calories, kcal), " +
                              "(2, Calories, g), " +
                              "(3, Protein, g), " +
                              "(4, Carbs, g), " +
                              "(5, Fat, g), " +
                              "(6, Fibre, g), " +
                              //Fat-soluble
                              "(7, Vitamin A, mgc), " +
                              "(8, Vitamin D, mgc), " +
                              "(9, Vitamin K, mgc), " +
                              "(10, Vitamin E, mgc), " +
                              //water-soluble
                              "(11, Vitamin C, mgc)";
        
        
        command.ExecuteNonQueryAsync();
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
