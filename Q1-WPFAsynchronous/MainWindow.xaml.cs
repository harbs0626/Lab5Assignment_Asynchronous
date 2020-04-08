using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using Microsoft.Win32;
/// <summary>
/// ** Student Name     : Harbin Ramo
/// ** Student Number   : 301046044
/// ** Lab Assignment   : #5 - Asynchronous Programming
/// ** Date (MM/dd/yyy) : 03/26/2020
/// ** Additional Notes for Regular Expressions (Regex):
/// **      https://forums.asp.net/t/1247607.aspx?Reading+CSV+with+comma+placed+within+double+quotes+
/// </summary>
namespace Q1_WPFAsynchronous
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ** Browse for file to be uploaded
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.InitialDirectory = this.GetFileDirectory();
            Nullable<bool> _openResult = _openFileDialog.ShowDialog();
            if (_openResult == true)
            {
                this.SelectedFileTextBox.Text = _openFileDialog.FileName;
                this.UploadButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("No file selected!");
                return;
            }
        }

        // ** Get file directory to retrieve StockData.csv
        private string GetFileDirectory()
        {
            string[] _pathList = Environment.CurrentDirectory.Split(
                System.IO.Path.DirectorySeparatorChar);
            string _currentPath = string.Empty;

            for(int i = 0; i < _pathList.Length - 2; i++)
            {
                _currentPath = _currentPath + _pathList[i] + "\\";
            }

            return _currentPath + "Files\\";
        }

        // ** Begin file uploading and processing
        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string _myFile = this.SelectedFileTextBox.Text;

                Task<Stock> _myTask = Task.Run(() => this.StartAsync(_myFile));

                await Task.WhenAll(_myTask);

                if (_myTask.IsCompleted)
                {
                    int _recordCount = Stock.ListStocks.Count();
                    MessageBox.Show($"{_recordCount} record(s) have been uploaded!");

                    this.RecordsTextBox.Text = $"{_recordCount}";

                    var result = from s in Stock.ListStocks
                                 orderby s.Date descending
                                 select s;

                    this.StockDataGrid.ItemsSource = result.ToList();

                    this.UploadButton.IsEnabled = false;

                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred while processing request");
                return;
            }
        }

        // ** Begin file uploading and processing
        Stock StartAsync(string _filePath)
        {
            var _myResult = new Stock();

            using(StreamReader _streamReader = new StreamReader(_filePath))
            {
                int counter = 1;

                while (true)
                {
                    string _readLine = _streamReader.ReadLine();
                    if (_readLine != null)
                    {
                        if (!_readLine.Contains("Symbol"))
                        {
                            Regex _parseString = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                            if (!_readLine.Contains("("))
                            {
                                Stock.GetData(counter, _parseString.Split(_readLine));
                                IncrementProgress(counter);
                                counter++;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return _myResult;
        }

        // ** Update progressbar value while uploading and processing file
        public void IncrementProgress(int i)
        {
            if (!CheckAccess())
            {
                Dispatcher.Invoke(() => IncrementProgress(i));
            }
            else
            {
                if (i > this.ProcessProgressBar.Maximum)
                {
                    this.ProcessProgressBar.Maximum = i;
                }
                this.ProcessProgressBar.Value += i;
            }
        }

        // ** Search button
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var result = from s in Stock.ListStocks
                         where s.Symbol == this.SearchTextBox.Text ||
                               s.Open.Contains(this.SearchTextBox.Text) ||
                               s.High.Contains(this.SearchTextBox.Text) ||
                               s.Low.Contains(this.SearchTextBox.Text) ||
                               s.Close.Contains(this.SearchTextBox.Text)
                         orderby s.Date descending
                         select s;

            MessageBox.Show($"{result.ToList().Count()} record(s) found.");
            
            this.RecordsTextBox.Text = $"{result.ToList().Count()}";
            this.StockDataGrid.ItemsSource = result.ToList();
        }

        // ** Clear Search button
        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Search cleared!");

            var result = from s in Stock.ListStocks
                         orderby s.Date descending
                         select s;

            this.SearchTextBox.Text = "-----";

            this.RecordsTextBox.Text = $"{result.ToList().Count()}";
            this.StockDataGrid.ItemsSource = result.ToList();
        }

        // ** Reset Form button
        private void ResetForm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Form cleared!");

            this.SelectedFileTextBox.Text = "-----";
            this.UploadButton.IsEnabled = false;
            this.SearchTextBox.Text = "-----";

            Stock.ListStocks.Clear();
            this.ProcessProgressBar.Value = 0;
            this.ProcessProgressBar.Maximum = 100;

            this.RecordsTextBox.Text = $"{0}";
            this.StockDataGrid.ItemsSource = null;
        }
    }
}
