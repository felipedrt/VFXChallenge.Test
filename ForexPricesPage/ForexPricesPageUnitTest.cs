using Moq;
using VFXChallenge.Components.Pages.ForexPricesPage;
using VFXChallenge.Components.Pages.LoginPage.Services;
using VFXChallenge.Components.Pages.LoginPage.Models;
using Bunit.TestDoubles;
using Bunit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Forms;
using VFXChallenge.Components.Pages.ForexPricesPage.Services;
using System.ComponentModel;

namespace VFXChallenge.Test.LoginPage
{
    [TestFixture]
    public class ForexPricesPageUnitTest
    {
        private Bunit.TestContext ctx;
        private Mock<LoginService> loginServiceMock;


        [SetUp]
        public void Setup()
        {
            ctx = new Bunit.TestContext();
            loginServiceMock = new Mock<LoginService>();

            loginServiceMock.Object.LoginPageModel = new LoginPageModel();
            ctx.Services.AddSingleton(loginServiceMock.Object);
            ctx.Services.AddScoped<ForexPriceService>();
        }

        [Test]
        public async Task ShouldCallGetDataWithSuccess()
        {
            var component = ctx.RenderComponent<ForexPrices>();
            await component.InvokeAsync(() => component.Instance.GetData());
            Assert.False(component.Instance.AlphaVantageResponse.HasError);
            Assert.IsEmpty(component.Instance.AlphaVantageResponse.MsgError);
            Assert.NotNull(component.Instance.AlphaVantageResponse.Data);
            Assert.IsNotEmpty(component.Instance.lastRefreshed.ToString());
        }

        [Test]
        public async Task ShouldCallGetDataWithError()
        {
            var component = ctx.RenderComponent<ForexPrices>();
            component.Instance.fromCurrency = "";
            component.Instance.toCurrency = "";

            await component.InvokeAsync(() => component.Instance.GetData());
            Assert.True(component.Instance.AlphaVantageResponse.HasError);
            Assert.IsNotEmpty(component.Instance.AlphaVantageResponse.MsgError);
            Assert.That(component.Instance.AlphaVantageResponse.Data.TimeSeries, Is.EqualTo(null));
            Assert.IsNotEmpty(component.Instance.lastRefreshed.ToString());
        }

        [Test]
        public async Task ShouldSetFromCurrencyValue()
        {
            var component = ctx.RenderComponent<ForexPrices>();
            ChangeEventArgs e = new ChangeEventArgs()
            {
                Value = "EUR"
            };
            await component.InvokeAsync(() => component.Instance.FromCurrencyValueChanged(e));

            Assert.False(component.Instance.AlphaVantageResponse.HasError);
            Assert.IsEmpty(component.Instance.AlphaVantageResponse.MsgError);
            Assert.NotNull(component.Instance.AlphaVantageResponse.Data);
            Assert.IsNotEmpty(component.Instance.lastRefreshed.ToString());
            Assert.That(component.Instance.fromCurrency, Is.EqualTo("EUR"));
        }

        [Test]
        public async Task ShouldSetToCurrencyValue()
        {
            var component = ctx.RenderComponent<ForexPrices>();
            ChangeEventArgs e = new ChangeEventArgs()
            {
                Value = "GBP"
            };
            await component.InvokeAsync(() => component.Instance.ToCurrencyValueChanged(e));

            Assert.False(component.Instance.AlphaVantageResponse.HasError);
            Assert.IsEmpty(component.Instance.AlphaVantageResponse.MsgError);
            Assert.NotNull(component.Instance.AlphaVantageResponse.Data);
            Assert.IsNotEmpty(component.Instance.lastRefreshed.ToString());
            Assert.That(component.Instance.fromCurrency, Is.EqualTo("GBP"));
        }

        [TearDown]
        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}