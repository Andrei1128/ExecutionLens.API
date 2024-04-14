using Nest;
using PostMortem.Domain.Models;

namespace PostMortem.Persistance.Extensions;

public static class ElasticExtensions
{
    public static SearchDescriptor<T> BetweenDates<T>(this SearchDescriptor<T> searchDescriptor, DateTime dateStart, DateTime dateEnd) where T : MethodLog
    {
        return searchDescriptor.Query(q => q
            //.DateRange(r => r
            //    .Field(f => f.Entry.Time)
            //    .GreaterThanOrEquals(dateStart)
            //    .LessThanOrEquals(dateEnd)
            //)
        );
    }
    public static SearchDescriptor<T> WithEndpointIfExists<T>(this SearchDescriptor<T> searchDescriptor, string endpointName) where T : MethodLog
    {
        if(string.IsNullOrWhiteSpace(endpointName))
            return searchDescriptor;

        return searchDescriptor.Query(q => q && q.Term(t => t
                //.Field(f => f.Entry.Method)
                //.Value(endpointName)
            ));
    }
    public static SearchDescriptor<T> WithControllerIfExists<T>(this SearchDescriptor<T> searchDescriptor, string controllerName) where T : MethodLog
    {
        if (string.IsNullOrWhiteSpace(controllerName))
            return searchDescriptor;

        return searchDescriptor.Query(q => q && q.Term(t => t
                //.Field(f => f.Entry.Class)
                //.Value(controllerName)
            ));
    }
}
