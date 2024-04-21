using ExecutionLens.API.DOMAIN.Models;
using Nest;

namespace ExecutionLens.API.PERSISTENCE.Extensions;

public static class ElasticExtensions
{
    public static SearchDescriptor<T> BetweenDates<T>(this SearchDescriptor<T> searchDescriptor, DateTime? dateStart, DateTime? dateEnd) where T : MethodLog
    {
        return searchDescriptor.Query(q => q
        .DateRange(r => r
            .Field(f => f.EntryTime)
            .GreaterThanOrEquals(dateStart)
            .LessThanOrEquals(dateEnd)
            ));
    }
    public static SearchDescriptor<T> WithEndpoints<T>(this SearchDescriptor<T> searchDescriptor, string[]? endpoints) where T : MethodLog
    {
        if(endpoints is null || endpoints.Length == 0)
            return searchDescriptor;

        throw new NotImplementedException();
    }
    public static SearchDescriptor<T> WithControllers<T>(this SearchDescriptor<T> searchDescriptor, string[]? controllers) where T : MethodLog
    {
        if (controllers is null || controllers.Length == 0)
            return searchDescriptor;

        throw new NotImplementedException();
    }
}
