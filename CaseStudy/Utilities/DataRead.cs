using CaseStudy.TestDataClasses;
using ExcelDataReader;
using System.Data;
using System.Text;

namespace CaseStudy.Utilities
{
    public class DataRead
    {
        public static List<AmazonText> AmazonReadData(string excelFilePath, string? sheetName)
        {
            List<AmazonText> excelDataList = new List<AmazonText>();
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
                                AmazonText excelData = new AmazonText
                                {
                                    SearchText = GetValueOrDefault(row, "searchtext")

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
