using Microsoft.AspNetCore.Mvc;
using SiebePaesschesoone.Controllers;
using Xunit;

namespace SiebePaesschesooneTest
{
    public class HomeControllerTest
    {

        //tests if the action "Index" returns the right view
        [Fact]
        public void IndexViewTest()
        {
            var hc = new HomeController();
            var IndexResult = hc.Index() as ViewResult;

            Assert.Equal("Index", IndexResult.ViewName);
        }

        //tests if the action "Submissions" returns the right view
        [Fact]
        public void SubmissionsViewTest()
        {
            var hc = new HomeController();
            var IndexResult = hc.Submissions() as ViewResult;

            Assert.Equal("Submissions", IndexResult.ViewName);
        }

        //tests if the action "ValidationPage" returns the right view
        [Fact]
        public void ValidationPageViewTest()
        {
            var hc = new HomeController();
            var IndexResult = hc.ValidationPage() as ViewResult;

            Assert.Equal("ValidationPage", IndexResult.ViewName);
        }
    }
}

