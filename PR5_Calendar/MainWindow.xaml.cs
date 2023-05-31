using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Interfaces;
using MessageBox = System.Windows.MessageBox;

namespace PR5_Calendar
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : UiWindow
	{
		public static int selectedmonth;
		public static int firstdayofmonth;
		public List<DateRow> Dates { get; set; }
		public static DateTime CalTime = DateTime.Now;
		public MainWindow()
		{
			InitializeComponent();
			fill();
		}

		private void PreviousMonth_Click(object sender, RoutedEventArgs e)
		{
			
			CalTime = CalTime.AddMonths(-1);
			fill();
		}

		private void NextMonth_Click(object sender, RoutedEventArgs e)
		{
			CalTime = CalTime.AddMonths(1);
			fill();
		}

		private void fill()
		{
			Dates = new List<DateRow>();

			int daysInMonth = DateTime.DaysInMonth(MainWindow.CalTime.Year, MainWindow.CalTime.Month);
			DateTime firstOfMonth = new DateTime(MainWindow.CalTime.Year, MainWindow.CalTime.Month, 1);

			MonthTxb.Text = MainWindow.CalTime.ToString("MMMM") + " " + MainWindow.CalTime.ToString("yyyy");

			int dayOfWeek = (int)firstOfMonth.DayOfWeek;

			DateRow currentRow = new DateRow();
			currentRow.Sunday = " ";
			currentRow.Monday = " ";
			currentRow.Tuesday = " ";
			currentRow.Wednesday = " ";
			currentRow.Thursday = " ";
			currentRow.Friday = " ";
			currentRow.Saturday = " ";

			if (firstOfMonth.DayOfWeek != DayOfWeek.Sunday)
			{
				switch (firstOfMonth.DayOfWeek)
				{

					case DayOfWeek.Saturday:
						currentRow = new DateRow();
						currentRow.Saturday = "1";
						break;
					case DayOfWeek.Friday:
						currentRow = new DateRow();
						currentRow.Friday = "1";
						currentRow.Saturday = "2";
						break;
					case DayOfWeek.Thursday:
						currentRow = new DateRow();
						currentRow.Thursday= "1";
						currentRow.Friday = "2";
						currentRow.Saturday = "3";
						break;
					case DayOfWeek.Wednesday:
						currentRow = new DateRow();
						currentRow.Wednesday = "1";
						currentRow.Thursday= "2";
						currentRow.Friday = "3";
						currentRow.Saturday = "4";
						break;
					case DayOfWeek.Tuesday:
						currentRow = new DateRow();
						currentRow.Tuesday = "1";
						currentRow.Wednesday = "2";
						currentRow.Thursday= "3";
						currentRow.Friday = "4";
						currentRow.Saturday = "5";
						break;
					case DayOfWeek.Monday:
						currentRow = new DateRow();
						currentRow.Monday = "1";
						currentRow.Tuesday = "2";
						currentRow.Wednesday = "3";
						currentRow.Thursday= "4";
						currentRow.Friday = "5";
						currentRow.Saturday = "6";
						break;
					case DayOfWeek.Sunday:
						break;
				}
				Dates.Add(currentRow);
				currentRow = new DateRow();
			}




			for (int i = 1; i <= daysInMonth; i++)
			{
				switch (dayOfWeek)
				{
					case 0:
						if (i != 1) currentRow = new DateRow();
						Dates.Add(currentRow);
						currentRow.Sunday = i.ToString();
						break;
					case 1:
						currentRow.Monday = i.ToString();
						break;
					case 2:
						currentRow.Tuesday = i.ToString();
						break;
					case 3:
						currentRow.Wednesday = i.ToString();
						break;
					case 4:
						currentRow.Thursday = i.ToString();
						break;
					case 5:
						currentRow.Friday = i.ToString();
						break;
					case 6:
						currentRow.Saturday = i.ToString();
						break;
				}
				dayOfWeek = (dayOfWeek + 1) % 7;
			}

			DataGrid.ItemsSource = Dates;
			DataGrid.Items.Refresh();
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			foreach (DataGridCellInfo cellInfo in DataGrid.SelectedCells)
			{
				// Get the row and column index from the cell info
				int columnIndex = cellInfo.Column.DisplayIndex;
				int rowIndex = DataGrid.Items.IndexOf(cellInfo.Item);

				// Get the TextBlock control from the cell's content
				TextBlock textBlock = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;

				// Get the text from the TextBlock and display it in a MessageBox
				if (textBlock != null)
				{
					string cellText = textBlock.Text;
					MessageBox.Show(string.Format("Row {0}, Column {1}: {2}", rowIndex, columnIndex, cellText));
				}
			}
		}
	}
	public class DateRow
	{
		public string Sunday { get; set; }
		public string Monday { get; set; }
		public string Tuesday { get; set; }
		public string Wednesday { get; set; }
		public string Thursday { get; set; }
		public string Friday { get; set; }
		public string Saturday { get; set; }
	}

	public class DivideByConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double width)
			{
				MainWindow mnw = new MainWindow();
				

				if (mnw.DataGrid.Items.Count > 5) return (width - 100) / 6;
				else
				{
					return (width - 100) / 5;
				}
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
