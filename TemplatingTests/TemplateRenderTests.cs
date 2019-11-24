using System;
using System.Collections.Generic;
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
12d8=[die:12d8]
2d4+10=[die:2d4+10]
2d10-5=[die:2d10-5]
";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = Guid.Empty.ToString(),
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
            Assert.AreEqual(0, outTemplate.TemplateVariables.Count,
                $"Variable count incorrect : {outTemplate.TemplateVariables.Count}");

            var renderer = new TemplateRenderer(new MockRandomTableService(), new MockTemplateService());

            var render = renderer.RenderTemplateToMarkDown(outTemplate).Result;

            var regex1 = new Regex(@"1d6=(\d+)");
            var regex2 = new Regex(@"12d8=(\d+)");
            var regex3 = new Regex(@"2d4\+10=([\d-]+)");
            var regex4 = new Regex(@"2d10-5=([\d-]+)");

            var match1 = regex1.Match(render);
            var match2 = regex2.Match(render);
            var match3 = regex3.Match(render);
            var match4 = regex4.Match(render);

            int.TryParse(match1.Groups[1].Value, out var val1);
            int.TryParse(match2.Groups[1].Value, out var val2);
            int.TryParse(match3.Groups[1].Value, out var val3);
            int.TryParse(match4.Groups[1].Value, out var val4);

            Assert.IsTrue(match1.Success, "1d6 was not found");
            Assert.IsTrue(1 <= val1 && val1 <= 6, $"d6 roll not valid: {val1.ToString()}");
            Assert.IsTrue(match2.Success, "12d8 was not found");
            Assert.IsTrue(12 <= val2 && val2 <= 96, $"12d8 roll not valid: {val2.ToString()}");
            Assert.IsTrue(match3.Success, "2d4+10 was not found");
            Assert.IsTrue(12 <= val3 && val3 <= 18, $"2d4+10 roll not valid: {val3.ToString()}");
            Assert.IsTrue(match4.Success, "2d10-5 was not found");
            Assert.IsTrue(-3 <= val4 && val4 <= 15, $"2d10-5 roll not valid: {val4.ToString()}");
        }

        [TestMethod]
        public void RandomNumberRenderTest()
        {
            var templateBody =
                @"Random Test

1to6=[rng:1:6]
-6to6=[rng:-6:6]
6to-6=[rng:6:-6]
-10to-2=[rng:-10:-2]
";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = Guid.Empty.ToString(),
                Name = "Random Test",
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
            Assert.AreEqual(0, outTemplate.TemplateVariables.Count,
                $"Variable count incorrect : {outTemplate.TemplateVariables.Count}");

            var renderer = new TemplateRenderer(new MockRandomTableService(), new MockTemplateService());

            var render = renderer.RenderTemplateToMarkDown(outTemplate).Result;

            var regex1 = new Regex(@"1to6=(\d+)");
            var regex2 = new Regex(@"-6to6=([\d-]+)");
            var regex3 = new Regex(@"6to-6=([\d-]+)");
            var regex4 = new Regex(@"-10to-2=([\d-]+)");

            var match1 = regex1.Match(render);
            var match2 = regex2.Match(render);
            var match3 = regex3.Match(render);
            var match4 = regex4.Match(render);

            int.TryParse(match1.Groups[1].Value, out var val1);
            int.TryParse(match2.Groups[1].Value, out var val2);
            int.TryParse(match3.Groups[1].Value, out var val3);
            int.TryParse(match4.Groups[1].Value, out var val4);

            Assert.IsTrue(match1.Success, "1to6 was not found");
            Assert.IsTrue(1 <= val1 && val1 <= 6, "1to6 roll not valid");
            Assert.IsTrue(match2.Success, "-6to6 was not found");
            Assert.IsTrue(-6 <= val2 && val2 <= 6, "-6to6 roll not valid");
            Assert.IsTrue(match3.Success, "6to-6 was not found");
            Assert.IsTrue(-6 <= val3 && val3 <= 6, "6to-6 roll not valid");
            Assert.IsTrue(match4.Success, "-10to-2 was not found");
            Assert.IsTrue(-10 <= val4 && val4 <= -2, "-10to-2 roll not valid");
        }

        [TestMethod]
        public void ListRenderTest()
        {
            var list1 = new List<string> {"item1", "item2", "item3"};
            var list2 = new List<string> {"thing one", "thing two", "thing three"};


            var templateBody =
                @"List Test

list1=[lst:item1,item2, item3 ]
list2=[lst:thing one, thing two, thing three]
";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = Guid.Empty.ToString(),
                Name = "List Test",
                Category = "Test",
                Description = "Description here",
                Enabled = true,
                TemplateBody = templateBody
            };

            var result = validator.ValidateTemplate(template).Result;

            var outTemplate = result.Template;
            var errors = result.ValidationErrors;

            Assert.AreEqual(0, errors.Count, $"Validation errors incorrect : {errors.Count}");
            Assert.AreEqual(2, outTemplate.Fields.Count, $"Fields incorrect : {outTemplate.Fields.Count}");
            Assert.AreEqual(0, outTemplate.TemplateVariables.Count,
                $"Variable count incorrect : {outTemplate.TemplateVariables.Count}");

            var renderer = new TemplateRenderer(new MockRandomTableService(), new MockTemplateService());

            var render = renderer.RenderTemplateToMarkDown(outTemplate).Result;

            var regex1 = new Regex(@"^list1=([\d\w\s]+)[\r]", RegexOptions.Multiline);
            var regex2 = new Regex(@"^list2=([\d\w\s]+)[\r]", RegexOptions.Multiline);

            var match1 = regex1.Match(render);
            var match2 = regex2.Match(render);


            Assert.IsTrue(match1.Success, "list1 was not found");
            Assert.IsTrue(list1.Contains(match1.Groups[1].Value), "list1 not valid");
            Assert.IsTrue(match2.Success, "list2 was not found");
            Assert.IsTrue(list2.Contains(match2.Groups[1].Value), "list2 not valid");
        }

        [TestMethod]
        public void TableRenderTest()
        {
            var list = new List<string>
            {
                "Table entry 1",
                "Table entry 2",
                "Table entry 3",
                "Table entry 4"
            };


            var templateBody =
                @"Table Test

table=[tbl:Test Table ]
";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = Guid.Empty.ToString(),
                Name = "Table Test",
                Category = "Test",
                Description = "Description here",
                Enabled = true,
                TemplateBody = templateBody
            };

            var result = validator.ValidateTemplate(template).Result;

            var outTemplate = result.Template;
            var errors = result.ValidationErrors;

            Assert.AreEqual(0, errors.Count, $"Validation errors incorrect : {errors.Count}");
            Assert.AreEqual(1, outTemplate.Fields.Count, $"Fields incorrect : {outTemplate.Fields.Count}");
            Assert.AreEqual(0, outTemplate.TemplateVariables.Count,
                $"Variable count incorrect : {outTemplate.TemplateVariables.Count}");

            var renderer = new TemplateRenderer(new MockRandomTableService(), new MockTemplateService());

            var render = renderer.RenderTemplateToMarkDown(outTemplate).Result;

            var regex1 = new Regex(@"^table=([\d\w\s]+)[\r]", RegexOptions.Multiline);

            var match1 = regex1.Match(render);

            Assert.IsTrue(match1.Success, "list1 was not found");
            Assert.IsTrue(list.Contains(match1.Groups[1].Value), "list1 not valid");
        }


        [TestMethod]
        public void TemplateRenderTest()
        {
            var table = new List<string>
            {
                "Table entry 1",
                "Table entry 2",
                "Table entry 3",
                "Table entry 4"
            };

            var list = new List<string>
            {
                "thing one",
                "thing two",
                "thing three"
            };

            var templateBody =
                @"Table Test

template=[tmp:Test Template ]

Eof
";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = Guid.Empty.ToString(),
                Name = "Template Test",
                Category = "Test",
                Description = "Description here",
                Enabled = true,
                TemplateBody = templateBody
            };

            var result = validator.ValidateTemplate(template).Result;

            var outTemplate = result.Template;
            var errors = result.ValidationErrors;

            Assert.AreEqual(0, errors.Count, $"Validation errors incorrect : {errors.Count}");
            Assert.AreEqual(1, outTemplate.Fields.Count, $"Fields incorrect : {outTemplate.Fields.Count}");
            Assert.AreEqual(0, outTemplate.TemplateVariables.Count,
                $"Variable count incorrect : {outTemplate.TemplateVariables.Count}");

            var renderer = new TemplateRenderer(new MockRandomTableService(), new MockTemplateService());

            var render = renderer.RenderTemplateToMarkDown(outTemplate).Result;

            var regex1 = new Regex(@"2d4\+10=([\d-]+)");
            var regex2 = new Regex(@"-6to6=([\d-]+)");
            var regex3 = new Regex(@"^list=([\d\w\s]+)[\r]", RegexOptions.Multiline);
            var regex4 = new Regex(@"^table=(Table entry \d)", RegexOptions.Multiline);

            var match1 = regex1.Match(render);
            var match2 = regex2.Match(render);
            var match3 = regex3.Match(render);
            var match4 = regex4.Match(render);

            int.TryParse(match1.Groups[1].Value, out var val1);
            int.TryParse(match2.Groups[1].Value, out var val2);

            Assert.IsTrue(match1.Success, "2d4+10 was not found");
            Assert.IsTrue(12 <= val1 && val1 <= 18, $"2d4+10 roll not valid: {val1.ToString()}");
            Assert.IsTrue(match2.Success, "-6to6 was not found");
            Assert.IsTrue(-6 <= val2 && val2 <= 6, "-6to6 roll not valid");
            Assert.IsTrue(match3.Success, "list was not found");
            Assert.IsTrue(list.Contains(match3.Groups[1].Value), "list not valid");
            Assert.IsTrue(match4.Success, "Table was not found");
            Assert.IsTrue(table.Contains(match4.Groups[1].Value), "Table not valid");
        }



        [TestMethod]
        public void TableListVariableRenderTest()
        {
            var list = new List<string>
            {
                "Table entry 1",
                "Table entry 2",
                "Table entry 3",
                "Table entry 4"
            };


            var templateBody =
                @"Table Test

var1=[var:{ ""name"":""test"",""display"":true,""value"":""lst: Test""}]
var2=[var:{ ""name"":""table"",""display"":false,""value"":""lst:Table""}]

table=[tbl:{test} {table} ]
";


            var validator = new TemplateValidator(new MockRandomTableService(), new MockTemplateService());

            var template = new Template
            {
                Id = Guid.Empty.ToString(),
                Name = "Table Test",
                Category = "Test",
                Description = "Description here",
                Enabled = true,
                TemplateBody = templateBody
            };

            var result = validator.ValidateTemplate(template).Result;

            var outTemplate = result.Template;
            var errors = result.ValidationErrors;

            Assert.AreEqual(0, errors.Count, $"Validation errors incorrect : {errors.Count}");
            Assert.AreEqual(1, outTemplate.Fields.Count, $"Fields incorrect : {outTemplate.Fields.Count}");
            Assert.AreEqual(2, outTemplate.TemplateVariables.Count,
                $"Variable count incorrect : {outTemplate.TemplateVariables.Count}");

            var renderer = new TemplateRenderer(new MockRandomTableService(), new MockTemplateService());

            var render = renderer.RenderTemplateToMarkDown(outTemplate).Result;

            var regex1 = new Regex(@"^var1=([\w]+)[\r]", RegexOptions.Multiline);
            var regex2 = new Regex(@"^var2=[\r]", RegexOptions.Multiline);
            var regex3 = new Regex(@"table=(Table entry \d)");

            var match1 = regex1.Match(render);
            var match2 = regex2.Match(render);
            var match3 = regex3.Match(render);

            Assert.IsTrue(match1.Success, "var1 was not found");
            Assert.AreEqual("Test",match1.Groups[1].Value, "var not valid");

            Assert.IsTrue(match2.Success, "var2 was not found");

            Assert.IsTrue(match3.Success, "table was not found");
            Assert.IsTrue(list.Contains(match3.Groups[1].Value), "list1 not valid");
        }
    }
}


