using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CalendarPopup.Utils;
using Binding = System.Windows.Data.Binding;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using Orientation = System.Windows.Controls.Orientation;
using TextBox = System.Windows.Controls.TextBox;


namespace CalendarPopup;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private NotifyIcon notifyIcon;
    private MainViewModel viewModel;
    private bool isCalendarVisible = false;
    private DateTime? hiddenAt = null;
    
    public MainWindow()
    {
        InitializeComponent();
        InitializeNotifyIcon();
        this.Hide(); // Hide the window initially

        viewModel = new MainViewModel();

        var vStack = new Grid();
        vStack.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Minimum fill (Auto)
        vStack.RowDefinitions.Add(new RowDefinition
            { Height = new GridLength(1, GridUnitType.Star) }); // Maximum fill (*)

        var headerGrid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#21221c"))
        };
        headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        headerGrid.DataContext = viewModel;

        var binding = new Binding("TodayDateString")
        {
            Mode = BindingMode.TwoWay
        };

        var button1 = new TextBlock
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = new SolidColorBrush(Colors.Azure),
            Padding = new Thickness(12)
        };
        button1.SetBinding(TextBlock.TextProperty, binding);
        Grid.SetColumn(button1, 0);
        headerGrid.Children.Add(button1);

        var calendarArea = CalendarDetailGrid();
        Grid.SetRow(headerGrid, 0);
        vStack.Children.Add(headerGrid);
        Grid.SetRow(calendarArea, 1);
        vStack.Children.Add(calendarArea);

        this.Content = vStack;
    }

    public Grid CalendarDetailGrid()
    {
        var grid = new Grid
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#272927")),
            DataContext = viewModel
        };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(64) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(26) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        var binding = new Binding("SelectedMonth")
        {
            Mode = BindingMode.TwoWay,
            Converter = new DateTimeToMonthConverter()
        };
        var monthText = new TextBlock
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = new SolidColorBrush(Colors.Azure),
            Padding = new Thickness(12),
            VerticalAlignment = VerticalAlignment.Center,
            FontWeight = FontWeights.Bold,
        };
        monthText.SetBinding(TextBlock.TextProperty, binding);
        Grid.SetColumnSpan(monthText, 6);
        Grid.SetColumn(monthText, 0);
        Grid.SetRow(monthText, 0);
        grid.Children.Add(monthText);


        var upButton = CustomButtonTest("\u2191", () => { viewModel.GoBackOneMonth(); });
        Grid.SetColumn(upButton, 6);
        grid.Children.Add(upButton);

        var downButton = CustomButtonTest("\u2193", () => { viewModel.GoForwardOneMonth(); });
        Grid.SetColumn(downButton, 7);
        grid.Children.Add(downButton);


        var montag = new TextBlock
        {
            Text = "Mo",
            Foreground = new SolidColorBrush(Colors.Azure),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(montag, 2);
        Grid.SetColumn(montag, 1);
        grid.Children.Add(montag);


        var dienstag = new TextBlock
        {
            Text = "Di",
            Foreground = new SolidColorBrush(Colors.Azure),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(dienstag, 2);
        Grid.SetColumn(dienstag, 2);
        grid.Children.Add(dienstag);


        var mittwoch = new TextBlock
        {
            Text = "Mi",
            Foreground = new SolidColorBrush(Colors.Azure),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(mittwoch, 2);
        Grid.SetColumn(mittwoch, 3);
        grid.Children.Add(mittwoch);

        var donnerstag = new TextBlock
        {
            Text = "Do",
            Foreground = new SolidColorBrush(Colors.Azure),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(donnerstag, 2);
        Grid.SetColumn(donnerstag, 4);
        grid.Children.Add(donnerstag);

        var freitag = new TextBlock
        {
            Text = "Fr",
            Foreground = new SolidColorBrush(Colors.Azure),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(freitag, 2);
        Grid.SetColumn(freitag, 5);
        grid.Children.Add(freitag);

        var samstag = new TextBlock
        {
            Text = "Sa",
            Foreground = new SolidColorBrush(Colors.Azure),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(samstag, 2);
        Grid.SetColumn(samstag, 6);
        grid.Children.Add(samstag);

        var sonntag = new TextBlock
        {
            Text = "So",
            Foreground = new SolidColorBrush(Colors.Azure),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(sonntag, 2);
        Grid.SetColumn(sonntag, 7);
        grid.Children.Add(sonntag);

        RenderDays(grid, viewModel.SelectedMonth);

        // Subscribe to PropertyChanged event
        if (viewModel is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(viewModel.SelectedMonth))
                {
                    RenderDays(grid, viewModel.SelectedMonth);
                }
            };
        }

        return grid;
    }

    private void RenderDays(Grid grid, DateTime targetMonth)
    {
        var startDate = GetFirstMondayOrLastInPreviousMonth(targetMonth.Year, targetMonth.Month);
        
        Console.WriteLine("First monday is " + startDate);

        // Use LINQ to find elements at the specified position
        var elementsToRemove = grid.Children
            .OfType<UIElement>()
            .Where(child => (Grid.GetRow(child) >= 3 && Grid.GetRow(child) <= 3 + 6) && Grid.GetColumn(child) >= 0 &&
                            Grid.GetColumn(child) <= 7)
            .ToList(); // Convert to a list to avoid modifying the collection while iterating

        // Remove each element
        foreach (var element in elementsToRemove)
        {
            grid.Children.Remove(element);
        }

        for (int i = 0; i < 6; i++)
        {
            var kw = GetCalendarWeek(startDate);
            var kwBlock = new TextBlock
            {
                Text = "KW " + kw.ToString(),
                Foreground = new SolidColorBrush(targetMonth.Month == startDate.Month ? Colors.Azure : Colors.Gray),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetRow(kwBlock, 3 + i);
            Grid.SetColumn(kwBlock, 0);
            grid.Children.Add(kwBlock);


            for (int j = 0; j < 7; j++)
            {
                var color = targetMonth.Month == startDate.Month ? Colors.Azure : Colors.Gray;

                if (startDate == DateTime.Today)
                {
                    color = Colors.LightCoral;
                }

                var textBlock = new TextBlock
                {
                    Text = startDate.Day.ToString(),
                    Foreground = new SolidColorBrush(color),
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                Grid.SetRow(textBlock, 3 + i);
                Grid.SetColumn(textBlock, 1 + j);
                grid.Children.Add(textBlock);
                
                startDate = startDate.AddDays(1);
            }
        }
    }


    private void InitializeNotifyIcon()
    {
        notifyIcon = new NotifyIcon
        {
            Icon = new System.Drawing.Icon("calendar-icon_34471.ico"), // Use your .ico file here
            Text = "Calendar App",
            Visible = true
        };

        // Create a context menu for the tray icon
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Show Calendar", null, ShowCalendar);
        contextMenu.Items.Add("Exit", null, ExitApplication);

        notifyIcon.ContextMenuStrip = contextMenu;

        notifyIcon.Click += (s, e) =>
        {
            if (isCalendarVisible)
            {
                Console.WriteLine("Hiding calendar");
                isCalendarVisible = false;
                Hide();
            }
            else
            {
                if (hiddenAt == null || DateTime.Now.Subtract(hiddenAt.Value).TotalMilliseconds > 500)
                {
                    Console.WriteLine("Showing calendar");
                    isCalendarVisible = true;
                    ShowCalendar(s, e);
                }
            }
        };
    }

    private Border CustomButtonTest(string text, Action action)
    {
        // Create a Border to hold the TextBlock
        var border = new Border
        {
            Background = Brushes.Transparent,
            CornerRadius = new CornerRadius(5),
            Padding = new Thickness(10),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        // Create a TextBlock
        var textBlock = new TextBlock
        {
            Text = text,
            FontSize = 16,
            Foreground = Brushes.White,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        // Add the TextBlock to the Border
        border.Child = textBlock;

        // Add Hover Events
        border.MouseEnter += (s, e) => { border.Background = new SolidColorBrush(Colors.DarkGray); };
        border.MouseLeave += (s, e) => { border.Background = Brushes.Transparent; };

        // Add Click Event
        border.MouseDown += (s, e) => { action(); };

        // Add the Border to the Window
        return border;
    }

    private void ShowCalendar(object sender, EventArgs e)
    {
        viewModel.SelectedMonth = DateTime.Now;
        viewModel.TodayDateString = DateTime.Now.ToString("dddd, dd. MMMM", new CultureInfo("de-DE"));
        
        this.Show();
        this.Activate();
        this.Focus();
        this.WindowState = WindowState.Normal;

        PositionWindowBottomRight();
        CreateCalendar();
    }

    private void ExitApplication(object sender, EventArgs e)
    {
        notifyIcon.Visible = false;
        notifyIcon.Dispose();
        System.Windows.Application.Current.Shutdown();
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true; // Prevent the window from actually closing
        this.Hide(); // Just hide the window
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        Console.WriteLine("Window deactivated");
        hiddenAt = DateTime.Now;
        isCalendarVisible = false;
        
        // Close the window when it loses focus
        this.Close();
    }

    private void PositionWindowBottomRight()
    {
        // Get screen dimensions
        double screenWidth = SystemParameters.WorkArea.Width; // Excludes taskbar
        double screenHeight = SystemParameters.WorkArea.Height;

        // Get window dimensions
        double windowWidth = this.Width;
        double windowHeight = this.Height;

        // Calculate position for bottom-right corner
        this.Left = screenWidth - windowWidth; // X-axis position
        this.Top = screenHeight - windowHeight; // Y-axis position
    }

    private void CreateCalendar()
    {
        // Clear previous content (optional for multiple runs)
        CalendarGrid.Children.Clear();
        CalendarGrid.RowDefinitions.Clear();

        // Add the first row for headers (Monday to Friday)
        CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        string[] weekdays = { "KW", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

        for (int i = 0; i < weekdays.Length; i++)
        {
            var header = new TextBlock
            {
                Text = weekdays[i],
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5)
            };
            Grid.SetColumn(header, i);
            Grid.SetRow(header, 0); // First row for headers
            CalendarGrid.Children.Add(header);
        }

        // Get the current month and year
        DateTime now = DateTime.Now;
        int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

        // Start from the second row for days
        int currentRow = 1;
        CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });


        for (int day = 1; day <= daysInMonth; day++)
        {
            DateTime currentDate = new DateTime(now.Year, now.Month, day);

            // Calculate the day of the week (Monday = 1, ..., Sunday = 7)
            int dayOfWeek = (int)currentDate.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Adjust for Sunday (make it 7)

            // Ignore Saturday and Sunday
            if (dayOfWeek > 5) continue;

            // Add the CW only once per row (week)
            if (dayOfWeek == 1) // Start of a new week
            {
                // Add the CW number to the first column
                var cwText = new TextBlock
                {
                    Text = GetCalendarWeek(new DateTime(now.Year, now.Month, day)).ToString(),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5)
                };
                Grid.SetColumn(cwText, 0); // First column for CW
                Grid.SetRow(cwText, currentRow);
                CalendarGrid.Children.Add(cwText);
            }


            // Create a TextBlock for the day
            var dayText = new TextBlock
            {
                Text = day.ToString(),
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5)
            };

            // Calculate the row and column
            int column = dayOfWeek;
            Grid.SetColumn(dayText, column);
            Grid.SetRow(dayText, currentRow);

            CalendarGrid.Children.Add(dayText);

            // Add a new row when Monday starts a new week
            if (dayOfWeek == 5) // If it's Friday, move to a new row
            {
                currentRow++;
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
        }
    }


    private int GetCalendarWeek(DateTime date)
    {
        return ISOWeek.GetWeekOfYear(date);
    }

    static DateTime GetFirstMondayOrLastInPreviousMonth(int year, int month)
    {
        // First day of the specified month
        var firstDayOfMonth = new DateTime(year, month, 1);

        // If the 1st is Monday, return it
        if (firstDayOfMonth.DayOfWeek == DayOfWeek.Monday)
            return firstDayOfMonth;

        // Otherwise, find the last Monday of the previous month
        
        while (true)
        {
            firstDayOfMonth = firstDayOfMonth.AddDays(-1);
            if (firstDayOfMonth.DayOfWeek == DayOfWeek.Monday)
                break;
            
        }

        return firstDayOfMonth;
    }
}