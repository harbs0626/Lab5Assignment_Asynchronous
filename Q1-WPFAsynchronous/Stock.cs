using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/// <summary>
/// ** Student Name     : Harbin Ramo
/// ** Student Number   : 301046044
/// ** Lab Assignment   : #5 - Asynchronous Programming
/// ** Date (MM/dd/yyy) : 03/26/2020
/// </summary>
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

        public static void GetData(int stockId, string[] _data)
        {
            
            ListStocks.Add(
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
            _data = _data.Replace("\"", "");
            return _data;
        }

    }
}
