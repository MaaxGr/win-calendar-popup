using System.Globalization;
using System.Windows.Data;

namespace CalendarPopup.Utils;

public class DateTimeToMonthConverter : IValueConverter
{
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.ToString("MMMM yyyy", new CultureInfo("de-DE")); // Full month name
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (DateTime.TryParse(value as string, out DateTime dateTime))
        {
            return dateTime;
        }
        return DateTime.MinValue;
    }
    
}