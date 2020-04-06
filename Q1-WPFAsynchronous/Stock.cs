using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Q1_WPFAsynchronous
{
    public class Stock
    {
        public static List<Stock> ListStocks = new List<Stock>();

        public static int UploadProgress { get; set; }

        public int StockId { get; set; }

        public string Symbol { get; set; }

        public string Date { get; set; }

        public string Open { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public string Close { get; set; }

        //public static bool IsFileRead(string fileName)
        //{
        //    bool _isRead = false;
        //    int counter = 1;

        //    try
        //    {
        //        using (StreamReader _strReader = new StreamReader(fileName))
        //        {
        //            while (true)
        //            {
        //                string _readLine = _strReader.ReadLine();
        //                if (_readLine != null)
        //                {
        //                    if (!_readLine.Contains("Symbol"))
        //                    {
        //                        // ** Source: https://forums.asp.net/t/1247607.aspx?Reading+CSV+with+comma+placed+within+double+quotes+
        //                        Regex _parseString = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        //                        if (!_readLine.Contains("("))
        //                        {
        //                            GetData(counter++, _parseString.Split(_readLine));
        //                            UploadProgress += counter;
        //                        }

        //                        // ** For Test Only
        //                        //Console.WriteLine(_readLine);

        //                        _isRead = true;
        //                    }
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        _isRead = false;
        //    }

        //    return _isRead;
        //}

        public static void GetData(int stockId, string[] _data)
        {
            
            ListStocks.Add(
                //new Stock
                //{
                //    StockId = stockId,
                //    Symbol = _data[0],
                //    Date = Convert.ToDateTime(_data[1]).Date.ToShortDateString(),
                //    Open = Convert.ToDecimal(CleanData(_data[2])),
                //    High = Convert.ToDecimal(CleanData(_data[3])),
                //    Low = Convert.ToDecimal(CleanData(_data[4])),
                //    Close = Convert.ToDecimal(CleanData(_data[5]))
                //},
                new Stock
                {
                    StockId = stockId,
                    Symbol = _data[0],
                    Date = Convert.ToDateTime(_data[1]).Date.ToShortDateString(),
                    Open = CleanData(_data[2]),
                    High = CleanData(_data[3]),
                    Low = CleanData(_data[4]),
                    Close = CleanData(_data[5])
                }
            );
        }

        private static string CleanData(string _data)
        {
            _data = _data.Trim();
            //_data = _data.Replace("$", "");
            _data = _data.Replace("\"", "");
            //if (_data.Contains("("))
            //{
            //    _data = _data.Replace("(", "");
            //    _data = _data.Replace(")", "");
            //    _data = "-" + _data;
            //}
            //return $"{Convert.ToDecimal(_data):C}";
            return _data;
        }

    }
}
