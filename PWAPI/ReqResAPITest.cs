using Microsoft.Playwright;
using System.Text.Json;

namespace PWAPI
{
    public class ReqResAPITest
    {
        IAPIRequestContext reqContext;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            reqContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = "https://reqres.in/api/"
            });
        }

        [Test]
        public async Task GetAllUsers()
        {
            var getres = await reqContext.GetAsync(url: "users?page=2");
            await Console.Out.WriteLineAsync("Res : \n " + getres.ToString());
            await Console.Out.WriteLineAsync("Code : \n " + getres.Status);
            await Console.Out.WriteLineAsync("Text : \n " + getres.StatusText);

            Assert.That(getres.Status.Equals(200));
            Assert.That(getres, Is.Not.Null);

            JsonElement resBody = (JsonElement)await getres.JsonAsync();
            await Console.Out.WriteLineAsync("Res Body : \n " + resBody.ToString());          
        }
        [Test]
        [TestCase(2)]
        public async Task GetSingleUser(int uid)
        {
            var getres = await reqContext.GetAsync(url: "users" + uid);
            await Console.Out.WriteLineAsync("Res : \n " + getres.ToString());
            await Console.Out.WriteLineAsync("Code : \n " + getres.Status);
            await Console.Out.WriteLineAsync("Text : \n " + getres.StatusText);

            Assert.That(getres.Status.Equals(200));
            Assert.That(getres, Is.Not.Null);

            JsonElement resBody = (JsonElement)await getres.JsonAsync();
            await Console.Out.WriteLineAsync("Res Body : \n " + resBody.ToString());
        }
        [Test]
        [TestCase(23)]
        public async Task GetSingleUserNotFound(int uid)
        {
            var getres = await reqContext.GetAsync(url: "users/" + uid);
            await Console.Out.WriteLineAsync("Res : \n " + getres.ToString());
            await Console.Out.WriteLineAsync("Code : \n " + getres.Status);
            await Console.Out.WriteLineAsync("Text : \n " + getres.StatusText);

            Assert.That(getres.Status.Equals(404));
            Assert.That(getres, Is.Not.Null);

            JsonElement resBody = (JsonElement)await getres.JsonAsync();
            await Console.Out.WriteLineAsync("Res Body : \n " + resBody.ToString());

            Assert.That(resBody.ToString, Is.EqualTo("{}"));
        }

        [Test]
        [TestCase("John", "Engineer")]
        public async Task PostUser(string nm, string jb)
        {
            var postData = new { name = nm, job = jb };
            var jsonData= System.Text.Json.JsonSerializer.Serialize(postData);

            var postres = await reqContext.PostAsync(url: "users", new APIRequestContextOptions { Data = jsonData});
            await Console.Out.WriteLineAsync("Res : \n " + postres.ToString());
            await Console.Out.WriteLineAsync("Code : \n " + postres.Status);
            await Console.Out.WriteLineAsync("Text : \n " + postres.StatusText);

            Assert.That(postres.Status.Equals(201));
            Assert.That(postres, Is.Not.Null);
        }
        [Test]
        [TestCase(2,"John", "Engineer")]
        public async Task PutUser(int uid, string nm, string jb)
        {
            var putData = new { name = nm, job = jb };
            var jsonData = System.Text.Json.JsonSerializer.Serialize(putData);

            var putres = await reqContext.PutAsync(url: "users/" + uid, new APIRequestContextOptions { Data = jsonData });
            await Console.Out.WriteLineAsync("Res : \n " + putres.ToString());
            await Console.Out.WriteLineAsync("Code : \n " + putres.Status);
            await Console.Out.WriteLineAsync("Text : \n " + putres.StatusText);

            Assert.That(putres.Status.Equals(200));
            Assert.That(putres, Is.Not.Null);
        }
        [Test]
        [TestCase(2)]
        public async Task DeleteUser(int uid)
        {
            var deleteres = await reqContext.DeleteAsync(url: "users" + uid);
            await Console.Out.WriteLineAsync("Res : \n " + deleteres.ToString());
            await Console.Out.WriteLineAsync("Code : \n " + deleteres.Status);
            await Console.Out.WriteLineAsync("Text : \n " + deleteres.StatusText);

            Assert.That(deleteres.Status.Equals(204));
            Assert.That(deleteres, Is.Not.Null);
        }
    }
}