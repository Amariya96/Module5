using CaseStudy.PWTests.Pages;
using CaseStudy.TestDataClasses;
using CaseStudy.Utilities;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy.PWTests.Tests
{
    [TestFixture]
    public class LoginPageTest : PageTest
    {
        Dictionary<string, string> Properties;
        string? currdir;
        private void ReadConfigSettings()
        {
            Properties = new Dictionary<string, string>();
            currdir = Directory.GetParent(@"../../../")?.FullName;

            string fileName = currdir + "/configsettings/config.properties";
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && line.Contains('='))
                {
                    string[] parts = line.Split('=');
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    Properties[key] = value;
                }
            }
        }

        [SetUp]
        public async Task Setup()
        {
            ReadConfigSettings();
            Console.WriteLine("Opened Browser");
            await Page.GotoAsync(Properties["baseUrl"], new Microsoft.Playwright.PageGotoOptions
            { WaitUntil = Microsoft.Playwright.WaitUntilState.DOMContentLoaded });
            Console.WriteLine("Amazon home page loaded");
        }

        [Test]
        public async Task SearchProductTest()
        {
            LoginPage loginPage = new LoginPage(Page);

            string? excelFilePath = currdir + "/Test Data/AmazonData.xlsx";
            string? sheetName = "SearchData";

            List<AmazonText> excelDataList = DataRead.AmazonReadData(excelFilePath, sheetName);

            foreach (var excelData in excelDataList)
            {

                string? searchtext = excelData.SearchText;
                loginPage.Search(searchtext);

                await Console.Out.WriteLineAsync("Clicked on Search Input Box");
                await Console.Out.WriteLineAsync($"Typed searchtext: {searchtext}");


                await Console.Out.WriteLineAsync(Page.Url);


            }
        }
    }
    }
