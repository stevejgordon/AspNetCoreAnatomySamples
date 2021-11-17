using AspNetCoreAnatomySamples.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AspNetCoreAnatomySamples.Customisation.ModelBinding
{
    public class DateRangeBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            _ = bindingContext ?? throw new ArgumentNullException(nameof(bindingContext));

            // the parameter names we are expecting
            const string startDateModelName = "startDate";
            const string endDateModelName = "endDate";

            // see if the value providers can supply the parameters
            var startDateValueProviderResult = bindingContext.ValueProvider.GetValue(startDateModelName);
            var endDateValueProviderResult = bindingContext.ValueProvider.GetValue(endDateModelName);

            // if we are missing one or both of the parameter we can't bind the date range
            if (startDateValueProviderResult == ValueProviderResult.None || 
                endDateValueProviderResult == ValueProviderResult.None)
            {
                bindingContext.ModelState.TryAddModelError("dateRange", "A valid start and end date are required.");
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            // set the values on the ModelState
            bindingContext.ModelState.SetModelValue(startDateModelName, startDateValueProviderResult);
            bindingContext.ModelState.SetModelValue(endDateModelName, endDateValueProviderResult);

            // Get the actual value from the binding results
            var startDateValue = startDateValueProviderResult.FirstValue;
            var endDateValue = endDateValueProviderResult.FirstValue;

            var validStartDate = DateTime.TryParse(startDateValue, out var startDate);
            var validEndDate = DateTime.TryParse(endDateValue, out var endDate);

            // Try to parse the values to dates, if this fails, we can't use them
            if (validStartDate && validEndDate)
            {
                try
                {
                    // create the new DateRange instance using the date values
                    var dateRange = new DateRange(startDate.Date, endDate.Date);

                    // set the binding result to success, with the bound DateRange
                    bindingContext.Result = ModelBindingResult.Success(dateRange);

                    return Task.CompletedTask;
                }
                catch (ArgumentException e)
                {
                    // if we have dates but the end is prior to the start, DateRange throws.
                    // we catch that here and set an appropriate model error.
                    bindingContext.ModelState.TryAddModelError(e.ParamName, e.Message);
                    bindingContext.Result = ModelBindingResult.Failed();
                }
            }

            // update model state with failures if either of the supplied properties can't be parsed as a
            // valid date

            if (!validStartDate)
                bindingContext.ModelState.TryAddModelError(startDateModelName, "A start date must be provided.");

            if (!validEndDate)
                bindingContext.ModelState.TryAddModelError(endDateModelName, "An end date must be provided.");

            bindingContext.Result = ModelBindingResult.Failed();

            return Task.CompletedTask;
        }
    }

    public class DateRangeBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            // if the required model type is DateRange, we return the DateRangeBinder
            // if we return null, other providers are checked.

            return context.Metadata.ModelType == typeof(DateRange) 
                ? new BinderTypeModelBinder(typeof(DateRangeBinder)) : null;
        }
    }
}

