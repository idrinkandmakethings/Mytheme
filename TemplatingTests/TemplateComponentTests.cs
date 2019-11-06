using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mytheme.Dal.Dto;
using Mytheme.Templating.TemplateTypes;
using Mytheme.Templating;
using Newtonsoft.Json;
using TemplatingTests.Mocks;

namespace TemplatingTests
{
    [TestClass]
    public class TemplateComponentTests
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

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual(3, json.LowerBound, $"Lower bound was incorrect: {json.LowerBound}");
            Assert.AreEqual(18, json.UpperBound, $"Upper bound was incorrect: {json.UpperBound}");
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

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual(-18, json.LowerBound, $"Lower bound was incorrect: {json.LowerBound}");
            Assert.AreEqual(-3, json.UpperBound, $"Upper bound was incorrect: {json.UpperBound}");
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

            var json = JsonConvert.DeserializeObject<TemplateTbl>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None,error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("Test Table",json.TableName, $"Table value was incorrect: {json}");
            Assert.AreEqual(0,json.Variables.Count, $"There should be no variables set");
        }

        [TestMethod]
        public void SetRandomTableExistsTrailingWhiteSpaceTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tbl: Test Table ]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomTable
            };

            var (error, result) = validator.SetRandomTableField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTbl>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("Test Table", json.TableName, $"Table value was incorrect: {json}");
            Assert.AreEqual(0, json.Variables.Count, $"There should be no variables set");
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

            var json = JsonConvert.DeserializeObject<TemplateTbl>(result.TemplateJson);

            Assert.AreEqual(ValidationError.TableDoesNotExist, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("Missing Table", json.TableName, $"Table value was incorrect: {json}");
            Assert.AreEqual(0, json.Variables.Count, $"There should be no variables set");
        }


        [TestMethod]
        public void SetRandomTableExistsWithVariablesTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tbl:{Var 1} Test {Var 2} Table]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomTable
            };

            var (error, result) = validator.SetRandomTableField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTbl>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("{Var 1} Test {Var 2} Table", json.TableName, $"Table value was incorrect: {json}");
            Assert.IsTrue(json.Variables.Contains("Var 1"), "Var 1 is not in variable list");
            Assert.IsTrue(json.Variables.Contains("Var 2"), "Var 2 is not in variable list");
        }

        [TestMethod]
        public void SetRandomTableExistsWithBadVariablesTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tbl:{Var 1 Test {Var 2} Table]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomTable
            };

            var (error, result) = validator.SetRandomTableField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTbl>(result.TemplateJson);

            Assert.AreEqual(ValidationError.InvalidTag, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("{Var 1 Test {Var 2} Table", json.TableName, $"Table value was incorrect: {json}");
        }

        [TestMethod]
        public void SetRandomTableExistsWithBadVariablesNestedTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tbl:{Var 1 Test {Var 2}} Table]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.RandomTable
            };

            var (error, result) = validator.SetRandomTableField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTbl>(result.TemplateJson);

            Assert.AreEqual(ValidationError.InvalidTag, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("{Var 1 Test {Var 2}} Table", json.TableName, $"Table value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateTmpExistsTest()
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

            var json = JsonConvert.DeserializeObject<TemplateTmp>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("Test Template", json.TemplateName, $"Template value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateTmpExistsTrialingWhiteSpaceTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[TMP: Test Template ]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Template
            };

            var (error, result) = validator.SetTemplateField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTmp>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("Test Template", json.TemplateName, $"Template value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateTmpNotExistTest()
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

            var json = JsonConvert.DeserializeObject<TemplateTmp>(result.TemplateJson);

            Assert.AreEqual(ValidationError.TemplateDoesNotExist, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("Missing Template", json.TemplateName, $"Table value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateTmpWithVariablesTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tmp:{Var 1} Test {Var 2} Template]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Template
            };

            var (error, result) = validator.SetTemplateField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTmp>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("{Var 1} Test {Var 2} Template", json.TemplateName, $"Table value was incorrect: {json}");
            Assert.IsTrue(json.Variables.Contains("Var 1"), "Var 1 is not in variable list");
            Assert.IsTrue(json.Variables.Contains("Var 2"), "Var 2 is not in variable list");
        }

        [TestMethod]
        public void SetTemplateTmpWithBadVariablesTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tmp:{Var 1 Test {Var 2} Template]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Template
            };

            var (error, result) = validator.SetTemplateField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTmp>(result.TemplateJson);

            Assert.AreEqual(ValidationError.InvalidTag, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("{Var 1 Test {Var 2} Template", json.TemplateName, $"Table value was incorrect: {json}");
        }

        [TestMethod]
        public void SetTemplateTmpWithBadVariablesNestedTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = "[tmp:{Var 1 Test {Var 2}} Template]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Template
            };

            var (error, result) = validator.SetTemplateField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateTmp>(result.TemplateJson);

            Assert.AreEqual(ValidationError.InvalidTag, error, $"Validation error: {error.ToString()}");
            Assert.AreEqual("{Var 1 Test {Var 2}} Template", json.TemplateName, $"Table value was incorrect: {json}");
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

            var json = JsonConvert.DeserializeObject<TemplateLst>(result.TemplateJson);

            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json.Values.Contains("alfa"), $"List does not contain alfa");
            Assert.IsTrue(json.Values.Contains("bravo"), $"List does not contain bravo");
            Assert.IsTrue(json.Values.Contains("charlie"), $"List does not contain charlie");
        }

        [TestMethod]
        public void SetTemplateVarListTest()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = @"[var:{""name"":""gender"",""display"":true,""value"":""lst:male, female""}]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Variable
            };

            var (error, result) = validator.SetVariableField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateVar>(result.TemplateJson);

            var listField = JsonConvert.DeserializeObject<TemplateField>(json.TemplateObjectJson);

            var list = JsonConvert.DeserializeObject<TemplateLst>(listField.TemplateJson);

            Assert.AreEqual(field.VariableName, "gender", $"Variable name field is incorrect: {field.VariableName}");
            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json.Display, $"Display is incorrect: {json.Display}");
            Assert.AreEqual("gender", json.Name, $"Name is incorrect: {json.Name}");
            Assert.AreEqual(TemplateFieldType.List, json.TemplateObjectType, $"Type is not list");
            Assert.IsTrue(list.Values.Contains("male"), $"List does not contain male");
            Assert.IsTrue(list.Values.Contains("female"), $"List does not contain female");
        }

        [TestMethod]
        public void SetTemplateVarTableTest()
        {
            

        var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var testVal = @"[var:{ ""name"":""race"",""display"":true,""value"":""tbl:race""}]";

            var field = new TemplateField
            {
                Order = 1,
                Value = testVal,
                FieldType = TemplateFieldType.Variable
            };

            var (error, result) = validator.SetVariableField(field).Result;

            var json = JsonConvert.DeserializeObject<TemplateVar>(result.TemplateJson);

            var tblField = JsonConvert.DeserializeObject<TemplateField>(json.TemplateObjectJson);

            var tbl = JsonConvert.DeserializeObject<TemplateTbl>(tblField.TemplateJson);

            Assert.AreEqual(field.VariableName, "race", $"Variable name field is incorrect: {field.VariableName}");
            Assert.AreEqual(ValidationError.None, error, $"Validation error: {error.ToString()}");
            Assert.IsTrue(json.Display, $"Display is incorrect: {json.Display}");
            Assert.AreEqual("race", json.Name, $"Name is incorrect: {json.Name}");
            Assert.AreEqual(TemplateFieldType.RandomTable, json.TemplateObjectType, $"Type is not table");
            Assert.AreEqual("race",tbl.TableName, $"Table name is incorrect: {tbl.TableName}");
            Assert.AreEqual(0, tbl.Variables.Count, $"Table Vars != 0 : {tbl.Variables.Count}");

        }
    }
}
