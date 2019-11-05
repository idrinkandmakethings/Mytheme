using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mytheme.Dal.Dto;
using Mytheme.Templating;
using TemplatingTests.Mocks;

namespace TemplatingTests
{
    [TestClass]
    public class TemplateRenderTests
    {

        [TestMethod]
        public void DieRollRenderTest()
        {
            var templateBody =
                @"Die Roll Test
1d6=[die:d6]
12d8=[die:3d8]
2d4+10=[die:2d4+10]
2d10-5=[die:2d10-5]
";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = 1,
                Name = "Die Roll Test",
                Category = "Test",
                Description = "Description here",
                Enabled = true,
                TemplateBody = templateBody
            };

            var result = validator.ValidateTemplate(template).Result;

            var outTemplate = result.Template;
            var errors = result.ValidationErrors;

            Assert.AreEqual(0, errors.Count, $"Validation errors incorrect : {errors.Count}");
            Assert.AreEqual(4, outTemplate.Fields.Count, $"Fields incorrect : {outTemplate.Fields.Count}");
            Assert.AreEqual(0, outTemplate.TemplateVariables.Count, $"Variable count incorrect : {outTemplate.TemplateVariables.Count}");

            var renderer = new TemplateRenderer(new MockRandomTableService(), new MockTemplateService());

            var render = renderer.RenderTemplateToMarkDown(outTemplate).Result;

            var regex1 = new Regex(@"1d6=(\d+)");
            var regex2 = new Regex(@"12d8=(\d+)");
            var regex3 = new Regex(@"2d4\+10=([\d-]+)");
            var regex4 = new Regex(@"2d10-5=([\d-]+)");

            var match1 = regex1.Match(render);
            var match2 = regex1.Match(render);
            var match3 = regex1.Match(render);
            var match4 = regex1.Match(render);

            Assert.IsTrue(match1.Success, "1d6 was not found");
            Assert.IsTrue(match2.Success, "12d8 was not found");
            Assert.IsTrue(match3.Success, "2d4+10 was not found");
            Assert.IsTrue(match4.Success, "2d10-5 was not found");

        }
    }
}
