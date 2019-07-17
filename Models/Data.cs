using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Google.Apis.Sheets.v4;

namespace HumidityAndTempApp.Models
{
    public class Data : Credentials
    {
      
        private static readonly string SpreadSheetId = "1aH-XZ_ft9BLteExz6Cdu6kNBl3l6EcZnagvFn4Tu30I";
        private static readonly string sheet = "DHT11SensorData";
        public readonly string timeRange = "A2:A";
        public string valueRange { get; set; }
        public int lastValue { get; set; }


        public static List<int> GetColumnValues(string dataRange)
        {
            GetCredentials();
            List<int> colValues = new List<int>();

            string range = $"{sheet}!{dataRange}";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadSheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                try
                {


                    foreach (var row in values)
                    {
                        int temp;
                        bool isInt = Int32.TryParse(row[0].ToString(), out temp);

                        if (isInt)
                        {
                            colValues.Add(temp);
                        }

                    }
                }

                catch (Exception ex)
                {
                    string filePath = "/~/Errors/errors.txt";

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("-----------------------------------------------------------------------------");
                        writer.WriteLine("Date : " + DateTime.Now.ToString());
                        writer.WriteLine();

                        while (ex != null)
                        {
                            writer.WriteLine(ex.GetType().FullName);
                            writer.WriteLine("Message : " + ex.Message);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);

                            ex = ex.InnerException;
                        }
                    }
                }

             
            }

            return colValues;
        }

        public static List<DateTime> GetColumnValues(string dataRange, bool a) //for date type
        {
            GetCredentials();
            List<DateTime> colValues = new List<DateTime>();

            string range = $"{sheet}!{dataRange}";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadSheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                try
                {


                    foreach (var row in values)
                    {
                        DateTime temp;
                        bool isInt = DateTime.TryParse(row[0].ToString(), out temp);

                        if (isInt)
                        {
                            colValues.Add(temp);
                        }

                    }
                }

                catch (Exception ex)
                {
                    string filePath = "/~/Errors/errors.txt";

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("-----------------------------------------------------------------------------");
                        writer.WriteLine("Date : " + DateTime.Now.ToString());
                        writer.WriteLine();

                        while (ex != null)
                        {
                            writer.WriteLine(ex.GetType().FullName);
                            writer.WriteLine("Message : " + ex.Message);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);

                            ex = ex.InnerException;
                        }
                    }
                }


            }

            return colValues;
        }

        public static double GetOneValue(string one)
        {
            string range = $"{sheet}!{one}";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadSheetId, range);

            var response = request.Execute();
            var values = response.Values;
            double result; 

            if (values != null && values.Count > 0)
            {
                foreach(var row in values)
                {
                    result = double.Parse(row[0].ToString());
                    return result;
                }
                
                
            }

            return 0;
                

            
        }

        public static int GetLastFromColumn(List<int> l)
        {
            return l[l.Count - 1];
        }

        public static DateTime GetLastFromColumn(List<DateTime> l) //for date type
        {
            return l[l.Count - 1];
        }
    }
}