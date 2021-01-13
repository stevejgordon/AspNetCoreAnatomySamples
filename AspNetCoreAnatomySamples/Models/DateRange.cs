using System;
using AspNetCoreAnatomySamples.Customisation.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAnatomySamples.Models
{
    // todo: IEquatable, IComparable etc.

    [ModelBinder(BinderType = typeof(DateRangeBinder))]
    public readonly struct DateRange
    {
        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("The end date cannot be before the start date.", nameof(endDate));

            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public override string ToString() => $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}";
    }
}
