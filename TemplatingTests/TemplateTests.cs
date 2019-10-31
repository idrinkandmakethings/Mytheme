using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mytheme.Dal.Dto;
using Mytheme.Templating.TemplateTypes;
using Mytheme.Templating;
using Newtonsoft.Json;
using TemplatingTests.Mocks;

namespace TemplatingTests
{
    [TestClass]
    public class TemplateTests
    {
        [TestMethod]
        public void SetRandomNumberTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[rng:3:18]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomNumber
            };

            var regex = validator.validationRegexs[field.FieldType];
            
            var match = regex.Match(testVal);
            
            var (error, result) = validator.SetRandomNumberField(field, match);

            var json = JsonConvert.DeserializeObject<TemplateRng>(result.TemplateJson); 

            Assert.IsTrue(error == ValidationError.None, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json.LowerBound == 3, $"Lower bound was incorrect: {json.LowerBound}");
            Assert.IsTrue(json.UpperBound == 18, $"Upper bound was incorrect: {json.UpperBound}");
        }

        [TestMethod]
        public void SetRandomNumberNegativeTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[rng:-3:-18]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomNumber
            };

            var regex = validator.validationRegexs[field.FieldType];

            var match = regex.Match(testVal);

            var (error, result) = validator.SetRandomNumberField(field, match);

            var json = JsonConvert.DeserializeObject<TemplateRng>(result.TemplateJson);

            Assert.IsTrue(error == ValidationError.None, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json.LowerBound == -18, $"Lower bound was incorrect: {json.LowerBound}");
            Assert.IsTrue(json.UpperBound == -3, $"Upper bound was incorrect: {json.UpperBound}");
        }

        [TestMethod]
        public void SetRandomTableExistsTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tbl:Test Table]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomTable
            };

            var regex = validator.validationRegexs[field.FieldType];

            var match = regex.Match(testVal);

            var (error, result) = validator.SetRandomTableField(field, match).Result;

            var json = result.TemplateJson;

            Assert.IsTrue(error == ValidationError.None, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json == "Test Table", $"Table value was incorrect: {json}");
        }


        [TestMethod]
        public void SetRandomTableDoesNotExistTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tbl:Missing Table]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomTable
            };

            var regex = validator.validationRegexs[field.FieldType];

            var match = regex.Match(testVal);

            var (error, result) = validator.SetRandomTableField(field, match).Result;

            var json = result.TemplateJson;

            Assert.IsTrue(error == ValidationError.TableDoesNotExist, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json == "Missing Table", $"Table value was incorrect: {json}");
        }
    }
}
