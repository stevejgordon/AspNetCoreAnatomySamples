using System;
using System.Collections.Generic;
using AspNetCoreAnatomySamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreAnatomySamples.Customisation.ActionFilter
{
    public class TwoYearDateRangeFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do we have a bound dateRange?
            if (context.ActionArguments.TryGetValue("dateRange", out var dateRangeArg) && dateRangeArg is DateRange dateRange)
            {
                // If so, is the range less than 2 years?
                if (dateRange.EndDate > dateRange.StartDate.AddYears(2))
                {
                    // If not, return a bad request with problem details formatted content.
                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        {"dateRange", new [] { "The date range (between the 'startDate' and 'endDate') must span less than " +
                                               "two years." }}
                    }));
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }  
}
