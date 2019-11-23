using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mytheme.Dal.Dto;
using Mytheme.Templating;
using Mytheme.Templating.TemplateTypes;
using TemplatingTests.Mocks;

namespace TemplatingTests
{
    [TestClass]
    public class TemplateTests
    {
        

        [TestMethod]
        public void ValidTemplateTest()
        {
            var validTemplate =
                @"Test Template

Name: [tbl:{gender} {race} First Names] [tbl:Last Names]

Race: [var:{""name"":""race"",""display"":true,""value"":""tbl:race""}]
Gender: [var:{""name"":""gender"",""display"":true,""value"":""lst:male, female""}]

STR: [die:3d6]   DEX: [die:3d6] CON: [die:3d6]
INT: [die:3d6]   WIS: [die:3d6] CHA: [die:3d6]

[tmp:Test Template]

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse eu nulla vitae orci consequat pharetra id in dui. Nulla euismod pretium commodo. Fusce ultrices elit porta purus mattis efficitur. Pellentesque sed luctus metus. Quisque luctus sagittis magna eget porta. Suspendisse eu molestie lectus, in pellentesque lacus. Sed interdum feugiat tellus in vestibulum. Suspendisse erat risus, molestie ac quam vitae, euismod pellentesque urna. Integer vel elit eros. Praesent venenatis a justo sed dapibus. Aenean eget tortor cursus, tristique ipsum vitae, vehicula erat.


In sed ante vitae diam sodales rhoncus vel eu libero. Nam hendrerit ipsum ac magna consectetur accumsan. Phasellus ut efficitur arcu. Sed vestibulum at libero non lacinia. Fusce semper iaculis metus, et ullamcorper ante auctor sit amet. Nulla quis feugiat purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam eget enim vel mi dignissim sollicitudin. Cras non aliquet lectus. Integer id tortor eu ante dapibus blandit et non odio. Phasellus sed posuere libero. Quisque tincidunt, ligula sit amet pharetra dignissim, dui orci dictum nisi, id blandit tellus odio at eros. Maecenas eget rutrum turpis. Ut finibus leo a felis vehicula consectetur. Sed hendrerit justo sit amet erat cursus, quis suscipit diam consectetur. Vestibulum at accumsan dui, vel varius tellus.";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = 1,
                Name = "Valid Template",
                Category = "Test",
                Description = "Description here",
                Enabled = true,
                TemplateBody = validTemplate
            };

            var result = validator.ValidateTemplate(template).Result;

            var outTemplate = result.Template;
            var errors = result.ValidationErrors;

            Assert.AreEqual(0,errors.Count,  $"Validation errors count > 0 : {errors.Count}");
            Assert.AreEqual(9,outTemplate.Fields.Count, $"Fields count != 9 : {outTemplate.Fields.Count}");
            Assert.AreEqual(2, outTemplate.TemplateVariables.Count, $"Variable count != 2 : {outTemplate.TemplateVariables.Count}");
        }

        [TestMethod]
        public void InvalidTemplateTest()
        {
            var invalidTemplate =
                @"Test Template

Name: [tbl:{gender} {race} First Names] [tbl:Missing Table]

Race: [var:{""name"":""race"",""display"":true,""value"":""tbl:race""}]
Gender: [var:{""name"":""gender"",""display"":true,""value"":""lst:male, female}]

STR: [die:3d6]   DEX: [die:3d6] CON: [die:3dt]
INT: [die:3d6]   WIS: [die:3d6] CHA: [die:3d6]

[tmp:Missing Template]

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse eu nulla vitae orci consequat pharetra id in dui. Nulla euismod pretium commodo. Fusce ultrices elit porta purus mattis efficitur. Pellentesque sed luctus metus. Quisque luctus sagittis magna eget porta. Suspendisse eu molestie lectus, in pellentesque lacus. Sed interdum feugiat tellus in vestibulum. Suspendisse erat risus, molestie ac quam vitae, euismod pellentesque urna. Integer vel elit eros. Praesent venenatis a justo sed dapibus. Aenean eget tortor cursus, tristique ipsum vitae, vehicula erat.


In sed ante vitae diam sodales rhoncus vel eu libero. Nam hendrerit ipsum ac magna consectetur accumsan. Phasellus ut efficitur arcu. Sed vestibulum at libero non lacinia. Fusce semper iaculis metus, et ullamcorper ante auctor sit amet. Nulla quis feugiat purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam eget enim vel mi dignissim sollicitudin. Cras non aliquet lectus. Integer id tortor eu ante dapibus blandit et non odio. Phasellus sed posuere libero. Quisque tincidunt, ligula sit amet pharetra dignissim, dui orci dictum nisi, id blandit tellus odio at eros. Maecenas eget rutrum turpis. Ut finibus leo a felis vehicula consectetur. Sed hendrerit justo sit amet erat cursus, quis suscipit diam consectetur. Vestibulum at accumsan dui, vel varius tellus.";




            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = 1,
                Name = "Invalid Template",
                Category = "Test",
                Description = "Description here",
                Enabled = true,
                TemplateBody = invalidTemplate
            };

            var result = validator.ValidateTemplate(template).Result;

            var outTemplate = result.Template;
            var errors = result.ValidationErrors;

            Assert.AreEqual(errors.Count, 4, $"Validation errors count: {errors.Count}");
            Assert.AreEqual(ValidationError.TableDoesNotExist, errors["[tbl:Missing Table]"]);
            Assert.AreEqual(ValidationError.TemplateDoesNotExist, errors["[tmp:Missing Template]"]);
            Assert.AreEqual(ValidationError.InvalidTag, errors["[die:3dt]"]);
            Assert.AreEqual(ValidationError.InvalidTag, errors[@"[var:{""name"":""gender"",""display"":true,""value"":""lst:male, female}]"]);
            Assert.AreEqual(10,outTemplate.Fields.Count, $"Fields count != 10 : {outTemplate.Fields.Count}");
            Assert.AreEqual(1, outTemplate.TemplateVariables.Count, $"Variable count != 1 : {outTemplate.TemplateVariables.Count}");
        }

    }
}
