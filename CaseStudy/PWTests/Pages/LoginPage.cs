using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CaseStudy.PWTests.Pages
{
    internal class LoginPage
    {
        private IPage _page;
        private ILocator SearchInput => _page.Locator(selector: "#twotabsearchtextbox");
        private ILocator SearchButton => _page.Locator(selector: "#nav-search-submit-button");
        public LoginPage(IPage page) => _page = page;
        public async Task Search(string searchtext)
        {
            await SearchInput.ClickAsync();
            await SearchInput.FillAsync(searchtext);
            await SearchButton.ClickAsync();
        }

    }
}
