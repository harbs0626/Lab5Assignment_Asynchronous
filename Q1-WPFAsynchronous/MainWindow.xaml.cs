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

        public StreamReader _streamReader;

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this._streamReader = new StreamReader(this.SelectedFileTextBox.Text);

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                
                worker.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred while processing request");
                return;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var result = from s in Stock.ListStocks
                         where s.Symbol == this.SearchTextBox.Text
                         orderby s.Date descending
                         select s;

            MessageBox.Show($"{result.ToList().Count()} record(s) found.");
            
            this.RecordsTextBox.Text = $"{result.ToList().Count()}";
            this.StockDataGrid.ItemsSource = result.ToList();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int counter = 1;
            
            try
            {
                while (true)
                {
                    string _readLine = this._streamReader.ReadLine();
                    if (_readLine != null)
                    {
                        if (!_readLine.Contains("Symbol"))
                        {
                            // ** Source: https://forums.asp.net/t/1247607.aspx?Reading+CSV+with+comma+placed+within+double+quotes+
                            Regex _parseString = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                            if (!_readLine.Contains("("))
                            {
                                Stock.GetData(counter++, _parseString.Split(_readLine));

                                (sender as BackgroundWorker).ReportProgress(counter);
                                Thread.Sleep(100);
                            }

                            // ** For Test Only
                            //Console.WriteLine(_readLine);

                            //_isRead = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                e.Result = "Completed";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //_isRead = false;
            }

            //bool _isRead = Stock.IsFileRead(this.SelectedFileTextBox.Text);
            //if (_isRead)
            //{
            //    int _recordCount = Stock.ListStocks.Count();
            //    MessageBox.Show($"{_recordCount} record(s) have been uploaded!");

            //    this.RecordsTextBox.Text = $"{_recordCount}";

            //    var result = from s in Stock.ListStocks
            //                 orderby s.Date descending
            //                 select s;

            //    this.StockDataGrid.ItemsSource = result.ToList();
            //}

            //for (int i = 0; i < 100; i++)
            //{
            //    (sender as BackgroundWorker).ReportProgress(i);
            //    Thread.Sleep(100);
            //}
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage >= this.ProcessProgressBar.Maximum)
            {
                this.ProcessProgressBar.Maximum = e.ProgressPercentage;
            }

            this.ProcessProgressBar.Value = e.ProgressPercentage;

            
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ToString() == "Completed")
                {
                    int _recordCount = Stock.ListStocks.Count();
                    MessageBox.Show($"{_recordCount} record(s) have been uploaded!");

                    this.RecordsTextBox.Text = $"{_recordCount}";

                    var result = from s in Stock.ListStocks
                                 orderby s.Date descending
                                 select s;

                    this.StockDataGrid.ItemsSource = result.ToList();

                    this._streamReader.Close();
                    this._streamReader.Dispose();
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, 
            SelectionChangedEventArgs e)
        {

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedFileTextBox.Text = "-----";
            this.UploadButton.IsEnabled = false;
            this.SearchTextBox.Text = "-----";

            MessageBox.Show("Form cleared!");

            var result = from s in Stock.ListStocks
                         orderby s.Date descending
                         select s;

            this.RecordsTextBox.Text = $"{result.ToList().Count()}";
            this.StockDataGrid.ItemsSource = result.ToList();
        }
    }
}
