namespace NutritionTracker.Data;

public class DataStore : NotifyChanged
{
    public SqlLiteData SqlData { get; }
    private int _fontSize = 12;
    public int FontSize
    {
        get => _fontSize;
        set
        {
            if (value != _fontSize)
            {
                _fontSize = value;
                OnPropertyChanged();
            }
        }
    }

    public DataStore() 
    {
        SqlData = SqlLiteData.Instance;
    }
}