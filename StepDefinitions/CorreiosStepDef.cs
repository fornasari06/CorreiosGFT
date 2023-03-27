using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using TechTalk.SpecFlow.CommonModels;

namespace Correios.StepDefinitions
{
    [Binding]
    public sealed class CorreiosStepDef
    {

        private IWebDriver driver;

        [BeforeScenario]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.correios.com.br/");
        }


        [Given(@"Eu procuro pelo CEP (.*)")]
        public void GivenEuProcuroPeloCEP(string cep)
        {
            var cepInput = driver.FindElement(By.Id("relaxation"));
            cepInput.SendKeys(cep);
            Thread.Sleep(1000);
            cepInput.SendKeys(Keys.Enter);
        }

        [When(@"Confirmo que o CEP não Existe")]
        public void WhenConfirmoQueOCEPNaoExiste()
        {
            
            var currentWindow = driver.CurrentWindowHandle;
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            var h6Element = driver.FindElement(By.TagName("h6"));
            Assert.AreEqual("Dados não encontrado", h6Element.Text);;


        }

        [Then(@"Eu volto para tela incial")]
        public void ThenEuVoltoParaTelaIncial()
        {
            driver.Navigate().Back();
        }

        [When(@"Eu confirmo que o resultado é ""([^""]*)""")]
        public void WhenEuConfirmoQueOResultadoE(string endereco)
        {

            var currentWindow = driver.CurrentWindowHandle;
            driver.SwitchTo().Window(driver.WindowHandles.Last());


            var resultado = driver.FindElement(By.XPath("//*[@id=\"resultado-DNEC\"]/tbody/tr/td[1]"));
            Assert.AreEqual("Rua Quinze de Novembro - lado ímpar", resultado.Text); 

        }

        [Given(@"Eu procuro pelo código de rastreamento ""([^""]*)""")]
        public void GivenEuProcuroPeloCodigoDeRastreamento(string codigo)
        {
            var codigoInput = driver.FindElement(By.Id("objetos"));
            codigoInput.SendKeys(codigo);
            codigoInput.SendKeys(Keys.Enter);
            Thread.Sleep(5000);


        }

        [Then(@"Eu confirmo que o código não está correto")]
        public void ThenEuConfirmoQueOCodigoNaoEstaCorreto()
        {
            var result = driver.FindElement(By.CssSelector(".resp-erro"));
            if (result.Text != "O(s) código(s) informado(s) não está(ão) correto(s).")
                throw new Exception("Código correto");
        }

        [AfterStep]
        public void TakeScreenshotAfterStep()
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var stepText = ScenarioContext.Current.StepContext.StepInfo.Text;
            var invalidChars = Path.GetInvalidFileNameChars();
            var fileName = string.Join("_", stepText.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            var screenshotPath = Path.Combine("Screenshots", $"screenshot-{fileName}.png");
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

        }


        [AfterScenario]
        public void TearDown()
        {

            
            driver.Quit();
        }


    }
}