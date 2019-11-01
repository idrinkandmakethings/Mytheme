using System.Collections.Generic;
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

            var (error, result) = validator.SetRandomNumberField(field);

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

            var (error, result) = validator.SetRandomNumberField(field);

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

            var (error, result) = validator.SetRandomTableField(field).Result;

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

            var (error, result) = validator.SetRandomTableField(field).Result;

            var json = result.TemplateJson;

            Assert.IsTrue(error == ValidationError.TableDoesNotExist, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json == "Missing Table", $"Table value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateExistsTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[TMP:Test Template]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Template
            };

            var (error, result) = validator.SetTemplateField(field).Result;

            var json = result.TemplateJson;

            Assert.IsTrue(error == ValidationError.None, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json == "Test Template", $"Template value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateNotExistTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tmp:Missing Template]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Template
            };

            var (error, result) = validator.SetTemplateField(field).Result;

            var json = result.TemplateJson;

            Assert.IsTrue(error == ValidationError.TemplateDoesNotExist, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json == "Missing Template", $"Table value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateDieSingleTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[Die:d8]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.DieRoll
            };

            var (error, result) = validator.SetDieRollField(field);

            var json = JsonConvert.DeserializeObject<TemplateDie>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual(8, json.DieSize, $"Die size is not what is expected: {json.DieSize}");
            Assert.AreEqual(1, json.DieCount, $"Die count is not what is expected: {json.DieCount}");
            Assert.AreEqual(0, json.Modifier, $"Die mod is not what is expected: {json.Modifier}");
        }

        [TestMethod]
        public void SetTemplateDieSingleWithModPositiveTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[Die:d10+4]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.DieRoll
            };

            var (error, result) = validator.SetDieRollField(field);

            var json = JsonConvert.DeserializeObject<TemplateDie>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual(10, json.DieSize, $"Die size is not what is expected: {json.DieSize}");
            Assert.AreEqual(1, json.DieCount, $"Die count is not what is expected: {json.DieCount}");
            Assert.AreEqual(4, json.Modifier, $"Die mod is not what is expected: {json.Modifier}");
        }

        [TestMethod]
        public void SetTemplateDieSingleWithModNegativeTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[Die:d16-7]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.DieRoll
            };

            var (error, result) = validator.SetDieRollField(field);

            var json = JsonConvert.DeserializeObject<TemplateDie>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual(16, json.DieSize, $"Die size is not what is expected: {json.DieSize}");
            Assert.AreEqual(1, json.DieCount, $"Die count is not what is expected: {json.DieCount}");
            Assert.AreEqual(-7, json.Modifier, $"Die mod is not what is expected: {json.Modifier}");
        }

        [TestMethod]
        public void SetTemplateDieMultipleTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[Die:23d8]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.DieRoll
            };

            var (error, result) = validator.SetDieRollField(field);

            var json = JsonConvert.DeserializeObject<TemplateDie>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual(8, json.DieSize, $"Die size is not what is expected: {json.DieSize}");
            Assert.AreEqual(23, json.DieCount, $"Die count is not what is expected: {json.DieCount}");
            Assert.AreEqual(0, json.Modifier, $"Die mod is not what is expected: {json.Modifier}");
        }

        [TestMethod]
        public void SetTemplateDieMultipleWithModTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[die:4d6+2]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.DieRoll
            };

            var (error, result) = validator.SetDieRollField(field);

            var json = JsonConvert.DeserializeObject<TemplateDie>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual(6, json.DieSize, $"Die size is not what is expected: {json.DieSize}");
            Assert.AreEqual(4, json.DieCount, $"Die count is not what is expected: {json.DieCount}");
            Assert.AreEqual(2, json.Modifier, $"Die mod is not what is expected: {json.Modifier}");
        }

        [TestMethod]
        public void SetTemplateListTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[lst:alfa, bravo, charlie]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.List
            };

            var (error, result) = validator.SetListField(field);

            var json = JsonConvert.DeserializeObject<List<string>>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json.Contains("alfa"), $"List does not contain alfa");
            Assert.IsTrue(json.Contains("bravo"), $"List does not contain bravo");
            Assert.IsTrue(json.Contains("charlie"), $"List does not contain charlie");
        }

        [TestMethod]
        public void SetTemplateVarListTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = @"[var:{""name"":""gender"",""display"":true,""value"":""[lst:male, female]""}]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Variable
            };

            var (error, result) = validator.SetVariableField(field);

            var json = JsonConvert.DeserializeObject<TemplateVar>(result.TemplateJson);

            var list = (TemplateLst)json.Value;
            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json.Display, $"Display is incorrect: {json.Display}");
            Assert.AreEqual("gender", json.Name, $"Name is incorrect: {json.Name}");
            Assert.AreEqual(TemplateFieldType.List, json.Type, $"Type is not list");
            Assert.IsTrue(list.Values.Contains("male"), $"List does not contain male");
            Assert.IsTrue(list.Values.Contains("female"), $"List does not contain female");
        }

        [TestMethod]
        public void SetTemplateVarInvalidTest()
        {

        }
    }
}
