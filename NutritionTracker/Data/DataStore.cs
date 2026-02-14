namespace NutritionTracker.Data;

public class DataStore : NotifyChanged
{
    private int _fontSize = 20;
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
}