using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace CyberVisionTests
{
    public class DuplicateProductTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _page = await _browser.NewPageAsync();
            // LogIn do aplikacji :
            await _page.GotoAsync("https://demo.cybervision.raion.eu/");
            await _page.FillAsync("input[data-test-id='user-name-input']", "alona.adamska");
            await _page.FillAsync("input[data-test-id='password-input']", "Mwz7HdDIx2Eh67D");
            await _page.ClickAsync("button[data-test-id='login-button']");
        }

        [Test]
        public async Task AddDuplicateProductShouldShowWarning()
        {   
            var expectedMessage = "Produkt o podanej nazwie już istnieje.  Czy na pewno chcesz dodać produkt o takiej samej nazwie?";

            await _page.ClickAsync("text=Produkcja");
            await _page.ClickAsync("text=Produkty");
            await _page.ClickAsync("#add-product-button");
            await _page.FillAsync("#edited-product-name", "Noga");
            await _page.FillAsync("#edited-product-code", "1");
            await _page.ClickAsync("text=Zapisz");
            
            

            var partOne = await _page.TextContentAsync(".v-dialog--active .v-card__title span:first-child");
            var partTwo = await _page.TextContentAsync(".v-dialog--active .v-card__title span:last-child");
            var fullMessage = (partOne + " " + partTwo).Trim();
            Assert.AreEqual(expectedMessage, fullMessage, "The dialog should contain the expected message.");


        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
        }
    }
}