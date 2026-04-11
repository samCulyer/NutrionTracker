using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics;
using System.IO;

namespace NutritionTracker.Data;

public class SqlDatabaseBuilder
{
    private SqliteConnection? _connection;
    private SqliteTransaction? _transaction;
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
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using SqliteTransaction transaction = connection.BeginTransaction();
        _connection = connection;
        _transaction = transaction;
        try 
        {
            //program
            CreateProgramSettingsTable();
            //user
            CreateUsersTable();
            //food
            CreateFoodCategoryTable();
            CreateDataSourceTable();
            CreateFoodTable();
            CreateNutrientsTable();
            CreateDietTable();
            //linkTables
            CreateUsersFoodTable();
            CreateFoodNutrientsTable();
            CreateDietFoodTable();
            CreateDietUsersTable();
            //views
            CreateFoodView();

            //future
            //Recipes
            //Recipes_Food
            //Recommended Amounts

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Debug.WriteLine(ex);
            Debugger.Break();
        }
        finally
        {
            _connection = null;
            _transaction = null;
        }
    }
    private void ExecuteSql(string query) 
    {
        if (_connection == null) 
        {
            throw new InvalidOperationException("Database not initialized.");
        }
        using SqliteCommand? command = _connection.CreateCommand();
        command.CommandText = query;
        if (_transaction != null) 
        {
            command.Transaction = _transaction;
        }
        command.ExecuteNonQuery();
    }
    private void CreateProgramSettingsTable()
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS ProgramSettings 
                    (Id INTEGER PRIMARY KEY, 
                    Theme INTEGER NOT NULL,
                    IsDBInitialised INTEGER NOT NULL,
                    IsDBFoodInitialised INTEGER NOT NULL,
                    Language TEXT NOT NULL,
                    FontSize INTEGER NOT NULL); ");
    }
    private void CreateUsersTable() 
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Users 
                    (Id INTEGER PRIMARY KEY, 
                    Name TEXT NOT NULL,
                    Gender TEXT NOT NULL,
                    Age INTEGER NOT NULL); ");
    }
    private void CreateUsersFoodTable() 
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Users_Food 
                    (Id INTEGER PRIMARY KEY,
                    UserId INTEGER ,
                    FoodId INTEGER,
                    DateTime TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    FOREIGN KEY(UserId) REFERENCES Users (Id), 
                    FOREIGN KEY(FoodId) REFERENCES Food (Id)); ");
    }
    private void CreateFoodTable()
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Food 
                    (Id INTEGER PRIMARY KEY, 
                    Name TEXT NOT NULL,
                    Description TEXT, 
                    ExternalId TEXT NOT NULL,
                    DataSourceId INTEGER,
                    FoodCategoryId INTEGER,
                    FOREIGN KEY(DataSourceId) REFERENCES DataSource (Id),
                    FOREIGN KEY(FoodCategoryId) REFERENCES FoodCategory (Id)); ");
    }
    private void CreateDataSourceTable() 
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS DataSource 
                    (Id INTEGER PRIMARY KEY, 
                    Name TEXT NOT NULL); ");

        ExecuteSql(@"INSERT OR IGNORE INTO DataSource (Id, Name) VALUES 
                    (1, 'UK'),
                    (2,'US');");
    }
    private void CreateFoodCategoryTable() 
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS FoodCategory 
                    (Id INTEGER PRIMARY KEY, 
                    Name TEXT NOT NULL); ");

        ExecuteSql(@"INSERT OR IGNORE INTO FoodCategory (Id, Name) VALUES 
                    (1, 'Meat'),
                    (2, 'Fish'),
                    (3, 'Dairy'),
                    (4, 'Vegetable'),
                    (5, 'Fruit'),
                    (6, 'Carbohydrate'),        
                    (7, 'NutsSeed'),
                    (8, 'FatsOil'),
                    (9, 'Snack'),
                    (10, 'SoupsSauce'),
                    (11, 'Drink');");
    }
    private void CreateNutrientsTable()
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Nutrients 
                    (Id INTEGER PRIMARY KEY, 
                    Name TEXT NOT NULL, 
                    NameAlternate TEXT, 
                    Unit TEXT); ");
        //milligram 1 thousandth of a gram mg
        //micrograms μg or mcg 1 millionth of a gram 
        //international units ui conversion depends on type of vitamin
        // 1 mg = 1000 mcg
        ExecuteSql(@"INSERT OR IGNORE INTO Nutrients (Id, Name,NameAlternate, Unit) VALUES 
                    (1, 'Calories',null, 'kcal'), 
                    (2, 'Protein',null, 'g'), 
                    (3, 'Carbohydrates',null, 'g'), 
                    (4, 'Fiber',null, 'g'), 
                    (5, 'Fat',null, 'g'), 
                    (6, 'Vitamin A',null, 'mcg'), 
                    (7, 'Vitamin B1','Thiamin', 'mcg'), 
                    (8, 'Vitamin B2','Riboflavin', 'mcg'), 
                    (9, 'Vitamin B3','Niacin', 'mcg'), 
                    (10, 'Vitamin B5','Pantothenic Acid', 'mcg'), 
                    (11, 'Vitamin B6','pyridoxal', 'mcg'),
                    (12, 'Vitamin B7','Biotin', 'mcg'), 
                    (13, 'Vitamin B9','Folic acid', 'mcg'), 
                    (14, 'Vitamin B12',null, 'mcg'), 
                    (15, 'Vitamin C',null, 'mcg'), 
                    (16, 'Choline',null, 'mcg'), 
                    (17, 'Vitamin D','calciferol', 'mcg'), 
                    (18, 'Vitamin E','alpha-tocopherol', 'mcg'), 
                    (19, 'Vitamin K','phylloquinone menadione', 'mcg'), 
                    (20, 'Calcium',null, 'mcg'), 
                    (21, 'Chloride',null, 'mcg'), 
                    (22, 'Chromium',null, 'mcg'), 
                    (23, 'Copper',null, 'mcg'), 
                    (24, 'Fluoride',null, 'mcg'), 
                    (25, 'Iodine',null, 'mcg'),
                    (26, 'Iron',null, 'mcg'), 
                    (27, 'Magnesium',null, 'mcg'), 
                    (28, 'Manganese',null, 'mcg'), 
                    (29, 'Molybdenum',null, 'mcg'), 
                    (30, 'Nickel',null, 'mcg'), 
                    (31, 'Phosphorus',null, 'mcg'), 
                    (32, 'Potassium',null, 'mcg'), 
                    (33, 'Selenium',null, 'mcg'), 
                    (34, 'Sodium',null, 'mcg'), 
                    (35, 'Zinc',null, 'mcg'); ");
    }
    private void CreateFoodNutrientsTable()
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Food_Nutrients 
                    (FoodId INTEGER,
                    NutrientId INTEGER, 
                    AmountPer100g REAL,
                    FOREIGN KEY(FoodId) REFERENCES Food (Id),
                    FOREIGN KEY(NutrientId) REFERENCES Nutrients (Id),
                    PRIMARY KEY (FoodId,NutrientId)
                    ); ");
    }
    private void CreateDietTable()
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Diet 
                    (Id INTEGER PRIMARY KEY, 
                    Name TEXT NOT NULL); ");
    }
    private void CreateDietFoodTable()
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Diet_Food 
                    (DietId INTEGER, 
                    FoodId INTEGER, 
                    FOREIGN KEY(DietId) REFERENCES Diet (Id),
                    FOREIGN KEY(FoodId) REFERENCES Food (Id),
                    PRIMARY KEY (DietId,FoodId)); ");
    }
    private void CreateDietUsersTable()
    {
        ExecuteSql(@"CREATE TABLE IF NOT EXISTS Diet_Users 
                    (DietId INTEGER,
                    UserId INTEGER, 
                    FOREIGN KEY(DietId) REFERENCES Diet (Id),
                    FOREIGN KEY(UserId) REFERENCES Users (Id),
                    PRIMARY KEY (DietId,UserId)); ");
    }

    private void CreateFoodView() 
    {
        ExecuteSql(@"CREATE VIEW IF NOT EXISTS Food_View AS SELECT
                    Food.Id,
                    Food.Name,
                    Food.Description,
                    Food.ExternalId, 
                    Food.DataSourceId AS SourceId,
                    DataSource.Name AS SourceName,
                    Food.FoodCategoryId AS CategoryId,
                    FoodCategory.Name AS CategoryName
                    FROM Food
                    LEFT JOIN DataSource ON DataSource.Id = Food.DataSourceId
                    LEFT JOIN FoodCategory ON FoodCategory.Id = Food.FoodCategoryId
                    ");
    }
    public static void ReadInUKDataBase() 
    { 
        //read excel
        //insert into food,nutrients etc.
    }
    public static void ReadInUSDataBase() 
    {
        //read csv
        //insert into food,nutrients etc.
    }
}
