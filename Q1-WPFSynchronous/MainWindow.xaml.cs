using System;
using System.Collections.Generic;
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
/// <summary>
/// ** Student Name     : Harbin Ramo
/// ** Student Number   : 301046044
/// ** Lab Assignment   : #5 - Synchronous Programming
/// ** Date (MM/dd/yyy) : 03/26/2020
/// </summary>
namespace Q1_WPFSynchronous
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

        // ** Begin calculate for factorial number
        private void CalculateFactorialButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string _enteredValue = string.Empty;
                _enteredValue = this.NumberTextBox.Text;

                this.FactorialNumber(int.Parse(_enteredValue));

                MessageBox.Show("Synchronous process complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while processing request.");
                return;
            }

        }

        // ** Factorial number method
        private void FactorialNumber(int _fN)
        {
            string _result = string.Empty;
            _result = _result + $"> Entered number is: {_fN}" + Environment.NewLine + Environment.NewLine;
            _result = _result + $"> Process begins at: {DateTime.Now}" + Environment.NewLine + Environment.NewLine;

            int _temp = 0;
            _temp = _fN;

            for(int i = _fN - 1; i >= 1; i--)
            {
                _temp = _temp * i;
            }

            _result = _result + $"> Process completed at: {DateTime.Now}" + Environment.NewLine + Environment.NewLine;
            _result = _result + $"> Number result       : {_temp}";

            this.ResultTextBox.Text = _result;
        }
    }
}
