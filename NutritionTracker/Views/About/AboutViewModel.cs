using System.Collections.Generic;
using NutritionTracker.Data;

namespace NutritionTracker.Views.About;

public class AboutViewModel : BaseViewModel
{
    public List<string> References { get; }= new  List<string>
    {
        "https://nutritionsource.hsph.harvard.edu/"
    };
    public AboutViewModel()
    {
        
    }
}