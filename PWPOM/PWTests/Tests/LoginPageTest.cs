using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using PWPOM.PWTests.Pages;
using PWPOM.TestDataClasses;
using PWPOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWPOM.PWTests.Tests
{
    [TestFixture]
    public class LoginPageTest : PageTest
    {
        Dictionary<string, string>? Properties;
        string? currdir;
        private void ReadConfigSettings()
        {
            Properties = new Dictionary<string, string>();
            currdir = Directory.GetParent(@"../../../")?.FullName;

            string fileName = currdir + "/ConfigSettings/config.properties";
            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && line.Contains("="))
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
            await Page.GotoAsync(Properties["baseUrl"]);
            Console.WriteLine("Page Loaded");
        }

        [Test]
        public async Task LoginTest()
        {
            // LoginPage lp = new(Page);
            NewLoginPage lp = new(Page);

            string? excelFilePath = currdir + "/TestData/EAData.xlsx";
            string? sheetName = "LoginData";

            List<EAText> excelDataList = LoginCredDataRead.ReadLoginCredData(excelFilePath, sheetName);

            foreach (var excelData in excelDataList)
            {
                string? username = excelData.UserName;
                string? password = excelData.Password;

                await lp.ClickLoginLink();
                await lp.Login(username, password);

                await Page.ScreenshotAsync(new() { 
                    Path = currdir + "/Screenshots/ss.png", FullPage=true,
                });

                Assert.IsTrue(await lp.CheckWelMess());
            }
        }
    }
}
