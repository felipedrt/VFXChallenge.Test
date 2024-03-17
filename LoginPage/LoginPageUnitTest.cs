using Moq;
using VFXChallenge.Components.Pages.LoginPage;
using VFXChallenge.Components.Pages.LoginPage.Services;
using VFXChallenge.Components.Pages.LoginPage.Models;
using Bunit.TestDoubles;
using Bunit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Forms;

namespace VFXChallenge.Test.LoginPage
{
    [TestFixture]
    public class LoginPageUnitTest
    {
        //private Login login;
        //private FakeNavigationManager _navigationMock;
        //private Mock<LoginService> _loginServiceMock;

        private Bunit.TestContext ctx;
        private FakeNavigationManager navigationMock;
        private Mock<LoginService> loginServiceMock;


        [SetUp]
        public void Setup()
        {
            //Bunit.TestContext testContext = new Bunit.TestContext();
            //_loginServiceMock = new Mock<LoginService>();

            //login = new Login
            //{
            //    Navigation = _navigationMock,
            //    LoginService = _loginServiceMock.Object
            //};
            ctx = new Bunit.TestContext();
            navigationMock = new FakeNavigationManager(ctx);
            loginServiceMock = new Mock<LoginService>();

            ctx.Services.AddSingleton(loginServiceMock.Object);
            ctx.Services.AddSingleton<NavigationManager>(navigationMock);
        }

        [Test]
        public void ShouldSubmitForm()
        {
            var component = ctx.RenderComponent<Login>();

            var clientIdInput = component.Find("#clientId");
            clientIdInput.Change("123");

            var userIdInput = component.Find("#userId");
            userIdInput.Change("456");

            var passwordInput = component.Find("#password");
            passwordInput.Change("12345");

            var submitButton = component.Find("input[type='submit']");
            submitButton.Click();

            Assert.That(loginServiceMock?.Object?.LoginPageModel?.ClientId, Is.EqualTo("123"));
            Assert.That(loginServiceMock?.Object?.LoginPageModel?.UserId, Is.EqualTo("456"));
            Assert.That(loginServiceMock?.Object?.LoginPageModel?.Password, Is.EqualTo("12345"));
            Assert.That(navigationMock.Uri, Is.EqualTo("http://localhost/forexPrices"));
        }

        [Test]
        public void ShouldNotSubmitForm()
        {
            var component = ctx.RenderComponent<Login>();

            var clientIdInput = component.Find("#clientId");
            clientIdInput.Change("");

            var userIdInput = component.Find("#userId");
            userIdInput.Change("");

            var passwordInput = component.Find("#password");
            passwordInput.Change("");

            var submitButton = component.Find("input[type='submit']");
            submitButton.Click();

            var clientMsgError = component.Find("#clientIdMsgError");
            var userIdMsgError = component.Find("#userIdMsgError");
            var passwordMsgError = component.Find("#passwordMsgError");

            Assert.IsNotEmpty(clientMsgError.InnerHtml);
            Assert.IsNotEmpty(userIdMsgError.InnerHtml);
            Assert.IsNotEmpty(passwordMsgError.InnerHtml);
        }

        [TearDown]
        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}