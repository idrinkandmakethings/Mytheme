using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Data.Dto;
using Mytheme.Services.Interfaces;
using Mytheme.Templating.TemplateTypes;
using Mytheme.Utility;
using Newtonsoft.Json;
using Serilog;

namespace Mytheme.Templating
{
    public class TemplateRenderer
    {
        private IRandomTableService tableService;
        private ITemplateService templateService;
        private Random rng;

        public TemplateRenderer(IRandomTableService tableService, ITemplateService templateService)
        {
            this.tableService = tableService;
            this.templateService = templateService;
            rng = new Random();
        }

        public async Task<string> RenderTemplateToMarkDown(Template template)
        {
            // generate var fields first
            var generatedVars = new Dictionary<string, string>();
            var templateBody = template.TemplateBody;
             
            foreach (var key in template.TemplateVariables.Keys)
            {
                var field = template.TemplateVariables[key];
                var json = JsonConvert.DeserializeObject<TemplateVar>(field.TemplateJson);

                var subField = JsonConvert.DeserializeObject<TemplateField>(json.TemplateObjectJson);
                
                var result = await RenderTemplateField(subField, generatedVars);
                
                generatedVars[field.VariableName] = result;

                if (json.Display)
                {
                    templateBody = templateBody.ReplaceFirst(field.Value, result);
                }
                else
                {
                    templateBody = templateBody.ReplaceFirst(field.Value, "");
                }
            }

            foreach (var field in template.Fields)
            {
                var result = await RenderTemplateField(field, generatedVars);
                templateBody = templateBody.ReplaceFirst(field.Value, result);
            }

            return templateBody;
        }

        private async Task<string> RenderTemplateField(TemplateField field, Dictionary<string, string> generatedVars)
        {
            switch (field.FieldType)
            {
                case TemplateFieldType.DieRoll:
                    return RollTheDice(field);
                case TemplateFieldType.List:
                    return SelectFromList(field);
                case TemplateFieldType.RandomNumber:
                    return RandomNumberGen(field);
                case TemplateFieldType.RandomTable:
                    return await TableResult(field, generatedVars);
                case TemplateFieldType.Template:
                    return await TemplateRecurse(field, generatedVars);
                default:
                    return "Template Render Error!";
            }
        }



        private string RollTheDice(TemplateField field)
        {
            try
            {
                var result = 0;


                var die = JsonConvert.DeserializeObject<TemplateDie>(field.TemplateJson);
                for (int i = 0; i < die.DieCount; i++)
                {
                    var next = rng.Next(1, die.DieSize + 1);
                    result += next;
                }

                result += die.Modifier;

                return result.ToString();
            }
            catch (Exception e)
            {
                Log.Error($"Exception calling RollTheDice(TemplateField field), ex:{e.Message}");
                Log.Debug(e.StackTrace);
                return "(Die roll error!)";
            }
        }

        private string SelectFromList(TemplateField field)
        {
            try
            {
                var lst = JsonConvert.DeserializeObject<TemplateLst>(field.TemplateJson);

                if (lst.Values.Count == 0)
                {
                    return "(List Empty!)";
                }

                return lst.Values[rng.Next(lst.Values.Count)];
            }
            catch (Exception e)
            {
                Log.Error($"Exception calling SelectFromList(TemplateField field), ex:{e.Message}");
                Log.Debug(e.StackTrace);
                return "(List error!)";
            }
        }

        private async Task<string> TableResult(TemplateField field, Dictionary<string, string> generatedVars)
        {
            try
            {
                var tbl = JsonConvert.DeserializeObject<TemplateTbl>(field.TemplateJson);
                var tableName = tbl.TableName;

                foreach (var variable in tbl.Variables)
                {
                    if (!generatedVars.ContainsKey(variable))
                    {
                        return "(Variable for table name missing!)";
                    }
                    tableName = tableName.Replace($"{{{variable}}}", generatedVars[variable]);
                }

                var exists = await tableService.TableExists(tableName);

                if (!exists.Result)
                {
                    return $"({tableName} table doesn't exist!)";
                }

                var tableVals = await tableService.GetRandomTableByName(tableName);

                var valsToSelectFrom = new List<string>();

                foreach (var entry in tableVals.Result.Entries)
                {
                    if (entry.UpperBound == entry.LowerBound)
                    {
                        valsToSelectFrom.Add(entry.Entry);
                    }
                    else
                    {
                        for (var i = entry.LowerBound; i <  entry.UpperBound; i++)
                        {
                            valsToSelectFrom.Add(entry.Entry);
                        }
                    }
                }

                return valsToSelectFrom[rng.Next(valsToSelectFrom.Count)];

            }
            catch (Exception e)
            {
                Log.Error($"Exception calling TableResult(TemplateField field), ex:{e.Message}");
                Log.Debug(e.StackTrace);
                return "(Table error!)";
            }
        }

        private string RandomNumberGen(TemplateField field)
        {
            try
            {
                var trng = JsonConvert.DeserializeObject<TemplateRng>(field.TemplateJson);

                return rng.Next(trng.LowerBound, trng.UpperBound + 1).ToString();
            }
            catch (Exception e)
            {
                Log.Error($"Exception calling RandomNumberGen(TemplateField field), ex:{e.Message}");
                Log.Debug(e.StackTrace);
                return "(RNG error!)";
            }
        }

        private async Task<string> TemplateRecurse(TemplateField field, Dictionary<string, string> generatedVars)
        {
            try
            {
                var tmp = JsonConvert.DeserializeObject<TemplateTmp>(field.TemplateJson);
                var templateName = tmp.TemplateName;

                foreach (var variable in tmp.Variables)
                {
                    if (!generatedVars.ContainsKey(variable))
                    {
                        return "(Variable for template name missing!)";
                    }
                    templateName = templateName.Replace($"{{{variable}}}", generatedVars[variable]);
                }

                var exists = await templateService.TemplateExists(templateName);

                if (!exists.Result)
                {
                    return $"({templateName} template doesn't exist!)";
                }

                var template = await templateService.GetTemplateByName(templateName);

                var result = await RenderTemplateToMarkDown(template.Result);

                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Exception calling TemplateRecurse(TemplateField field), ex:{e.Message}");
                Log.Debug(e.StackTrace);
                return "(Template error!)";
            }
        }
    }
}
