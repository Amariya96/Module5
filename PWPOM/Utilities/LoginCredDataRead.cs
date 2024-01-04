using ExcelDataReader;
using PWPOM.TestDataClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWPOM.Utilities
{
    public class LoginCredDataRead
    {
        public static List<EAText> ReadLoginCredData(string excelFilePath, string? sheetName)
        {
            List<EAText> excelDataList = new List<EAText>();
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true,
                            }
                        });
                        var dataTable = result.Tables[sheetName];
                        if (dataTable != null)
                        {

                            foreach (DataRow row in dataTable.Rows)
                            {
                                EAText excelData = new EAText
                                {
                                    UserName = GetValueOrDefault(row, "un"),
                                    Password = GetValueOrDefault(row, "pwd")

                                };
                                excelDataList.Add(excelData);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Sheet '{sheetName}' not found in the excel file");
                        }
                    }
                }
            return excelDataList;
        }
        static string? GetValueOrDefault(DataRow row, string columnName)
        {
            Console.WriteLine(row + "  " + columnName);
            return row.Table.Columns.Contains(columnName) ? row[columnName]?.ToString() : null;
        }
    }
}
