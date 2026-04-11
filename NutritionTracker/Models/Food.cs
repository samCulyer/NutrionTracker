using System;
using System.Collections.Generic;
using System.Text;

namespace NutritionTracker.Models;

public class NutrientModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AltName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
}
public class FoodModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string ExternalId { get; set; } = string.Empty;
    public int SourceId { get; set; }
    public string SourceName { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<NutrientModel> Nutrients { get; set; } = new();
}
