using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mytheme.Data.Dto
{
    public abstract class DtoObject
    {

        public bool TryValidate(out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(this, context, results,validateAllProperties: true);
        }
    }
}
