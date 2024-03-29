﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWPOM.PWTests.Pages
{
    internal class LoginPage
    {
        private IPage _page; //single browser tab
        private ILocator _lnkLogin;
        private ILocator _inpUserName;
        private ILocator _inpPassword;
        private ILocator _btnLogin;
        private ILocator _lnkWelMess;

        public LoginPage(IPage page)
        {
            _page = page;
            _lnkLogin = _page.Locator(selector:"text=Login");
            _inpUserName = _page.Locator(selector: "#UserName");
            _inpPassword = _page.Locator(selector: "#Password");
            _btnLogin = _page.Locator(selector: "input", new PageLocatorOptions { HasTextString = "Log in" });
            _lnkWelMess = _page.Locator(selector: "text='Hello admin!'");
        }

        public async Task ClickLoginLink()
        {
            await _lnkLogin.ClickAsync();
        }
        public async Task Login(string username, string password)
        {
            await _inpUserName.FillAsync(username);
            await _inpPassword.FillAsync(password);
            await _btnLogin.ClickAsync();
        }
        public async Task<bool> CheckWelMess()
        {
           bool IsWelMessVisible = await _lnkWelMess.IsVisibleAsync();
           return IsWelMessVisible;
        }
    }
}
