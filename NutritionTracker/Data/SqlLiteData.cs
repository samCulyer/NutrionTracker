using Avalonia.OpenGL;
using Microsoft.Data.Sqlite;
using NutritionTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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


    public async Task<List<NutrientModel>> GetNutrients() 
    {
        List<NutrientModel> nutrients = [];
        await using SqliteConnection connection = new(ConnectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM Nutrients";

        await using var reader = await command.ExecuteReaderAsync();
        if (reader.HasRows) 
        {
            while (reader.Read()) 
            { 
                NutrientModel nutrient = new NutrientModel() 
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    AltName = reader.GetString(2),
                    Unit = reader.GetString(3)
                };
                nutrients.Add(nutrient);
            }
        }

        return nutrients;
    }
    public async Task<List<FoodModel>> GetFoodWithSelectedNutrients(List<NutrientModel> nutrients) 
    {
        List<FoodModel> foods = [];
        await using SqliteConnection connection = new(ConnectionString);
        await connection.OpenAsync();

        var sb = new StringBuilder();
        sb.Append(@"SELECT 
                    Food_View.Id,
                    Food_View.Name,
                    Food_View.Description,
                    Food_View.ExternalId,
                    Food_View.SourceId,
                    Food_View.SourceName,
                    Food_View.CategoryId,
                    Food_View.CategoryName");
        foreach (var nutrient in nutrients) 
        {
            
        }

        sb.Append(@"FROM Food_View 
                    LEFT JOIN Food_Nutrients ON Food_view.Id = Food_Nutrients.FoodId
                    LEFT JOIN Nutrients ON Nutrients.Id = Food_Nutrients.NutrientId");

        await using var command = connection.CreateCommand();
        command.CommandText = sb.ToString();

        await using var reader = await command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                FoodModel food = new FoodModel() 
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    ExternalId = reader.GetString(3),
                    SourceId = reader.GetInt32(4),
                    SourceName = reader.GetString(5),
                    CategoryId = reader.GetInt32(6),
                    CategoryName = reader.GetString(7)
                };

                //NutrientModel nutrient = new NutrientModel()
                //{
                //    Id = reader.GetInt32(0),
                //    Name = reader.GetString(1),
                //    AltName = reader.GetString(2),
                //    Unit = reader.GetString(3)
                //};
                //nutrients.Add(nutrient);
            }
        }

        return foods;
    }
}
