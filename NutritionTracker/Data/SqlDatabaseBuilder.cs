using System;
using Microsoft.Data.Sqlite;
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
        command.CommandText = "INSERT OR IGNORE INTO Nutrients (Id, Name,NameAlternate, Unit) " +
                               "VALUES (1, 'Calories',NULL, 'kcal'), " +
                               "(2, 'Calories',null, 'g'), " +
                               "(3, 'Protein',null, 'g'), " +
                               "(4, 'Carbs',null, 'g'), " +
                               "(5, 'Fat',null, 'g'), " +
                               "(6, 'Fibre',null, 'g'), " +
                               //Fat-soluble
                               "(7, 'Vitamin A',null, 'mcg'), " +
                               "(8, 'Vitamin D',null, 'mcg'), " +
                               "(9, 'Vitamin K',null, 'mcg'), " +
                               "(10, 'Vitamin E',null, 'mcg'), " +
                               //water-soluble
                               "(11, 'Vitamin C',null, 'mcg')";
        
        
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
