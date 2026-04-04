using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarPopup;

public class MainViewModel : INotifyPropertyChanged
{
    
    private string _todayDateString = "";
    private DateTime _selectedMonth = DateTime.Now;
    
    public string TodayDateString
    {
        get => _todayDateString;
        set
        {
            _todayDateString = value;
            Console.WriteLine(_todayDateString);
            OnPropertyChanged();
        }
    }
    
    public DateTime SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            _selectedMonth = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsCurrentMonth));
        }
    }

    public bool IsCurrentMonth
    {
        get
        {
            var now = DateTime.Now;
            return _selectedMonth.Month == now.Month && _selectedMonth.Year == now.Year;
        }
    }

    public void GoBackOneMonth()
    {
        SelectedMonth = SelectedMonth.AddMonths(-1);
    }
    
    public void GoForwardOneMonth()
    {
        SelectedMonth = SelectedMonth.AddMonths(1);
    }

    public void ResetToToday()
    {
        SelectedMonth = DateTime.Now;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}