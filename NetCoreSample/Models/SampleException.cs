using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreSample.Models
{
    public class SampleException : Exception
    {

        List<ValidationResultModel> validationResultModel;
        public SampleException(ModelStateDictionary modelState) {
            validationResultModel = new List<ValidationResultModel>();
            validationResultModel = modelState.Keys
            .SelectMany(key => modelState[key].Errors.Select(x => new ValidationResultModel()
            {
                ErrorKey = key,
                ErrorMessage = x.ErrorMessage,
            })).ToList();
        }


        public class ValidationResultModel
        {
            public string ErrorKey { get; set; }

            public string ErrorMessage { get; set; }
        }
    }
}
