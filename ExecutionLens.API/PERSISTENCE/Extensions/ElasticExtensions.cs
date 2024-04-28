using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.DOMAIN.Models;
using Nest;

namespace ExecutionLens.API.PERSISTENCE.Extensions;

public static class ElasticExtensions
{
    public static SearchDescriptor<MethodLog> ApplySearchFilters(this SearchDescriptor<MethodLog> descriptor, SearchFilter filters, QueryContainer? existingQuery = null)
    {
        return descriptor
            .Query(q => q
                .Bool(b => b
                    .Must(must =>
                    {
                        var boolQuery = new BoolQuery();

                        if (existingQuery is not null)
                        {
                            boolQuery.Must = [existingQuery];
                        }

                        var filterList = new List<QueryContainer>();

                        if (filters.DateStart.HasValue || filters.DateEnd.HasValue)
                        {
                            var dateRangeQuery = new DateRangeQuery
                            {
                                Field = Infer.Field<MethodLog>(f => f.EntryTime),
                                GreaterThanOrEqualTo = filters.DateStart.HasValue ? filters.DateStart.Value : null,
                                LessThanOrEqualTo = filters.DateEnd.HasValue ? filters.DateEnd.Value : null
                            };
                            filterList.Add(dateRangeQuery);
                        }

                        if (filters.Controllers != null && filters.Controllers.Any())
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Class.Suffix("keyword")),
                                    Terms = filters.Controllers
                                }
                            );
                        }

                        if (filters.Endpoints != null && filters.Endpoints.Any())
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Method.Suffix("keyword")),
                                    Terms = filters.Endpoints
                                }
                            );
                        }

                        if (!string.IsNullOrEmpty(filters.HasException))
                        {
                            if (filters.HasException.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                            {
                                filterList.Add(
                                    new TermQuery { Field = Infer.Field<MethodLog>(f => f.HasException), Value = true }
                                );
                            }
                            else if (filters.HasException.Equals("No", StringComparison.OrdinalIgnoreCase))
                            {
                                filterList.Add(
                                     new TermQuery { Field = Infer.Field<MethodLog>(f => f.HasException), Value = false }
                                 );
                            }
                        }

                        if (filterList.Count != 0)
                        {
                            boolQuery.Filter = filterList;
                        }

                        return boolQuery;
                    })
                )
            ) ;
    }

    public static SearchDescriptor<MethodLog> ApplyFilters(this SearchDescriptor<MethodLog> descriptor, GraphFilters filters, QueryContainer? existingQuery = null)
    {
        return descriptor
            .Query(q => q
                .Bool(b => b
                    .Must(must =>
                    {
                        var boolQuery = new BoolQuery();

                        if (existingQuery is not null)
                        {
                            boolQuery.Must = [existingQuery];
                        }

                        var filterList = new List<QueryContainer>();

                        if (filters.DateStart.HasValue || filters.DateEnd.HasValue)
                        {
                            var dateRangeQuery = new DateRangeQuery
                            {
                                Field = Infer.Field<MethodLog>(f => f.EntryTime),
                                GreaterThanOrEqualTo = filters.DateStart.HasValue ? filters.DateStart.Value : null,
                                LessThanOrEqualTo = filters.DateEnd.HasValue ? filters.DateEnd.Value : null
                            };
                            filterList.Add(dateRangeQuery);
                        }

                        if (filters.Controllers != null && filters.Controllers.Any())
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Class.Suffix("keyword")),
                                    Terms = filters.Controllers
                                }
                            );
                        }

                        if (filters.Endpoints != null && filters.Endpoints.Any())
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Method.Suffix("keyword")),
                                    Terms = filters.Endpoints
                                }
                            );
                        }

                        if (!string.IsNullOrEmpty(filters.IsEntryPoint))
                        {
                            if (filters.IsEntryPoint.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                            {
                                filterList.Add(
                                    new BoolQuery
                                    {
                                        MustNot = new QueryContainer[] { new ExistsQuery { Field = Infer.Field<MethodLog>(f => f.NodePath) } }
                                    }
                                );
                            }
                            else if (filters.IsEntryPoint.Equals("No", StringComparison.OrdinalIgnoreCase))
                            {
                                filterList.Add(
                                    new ExistsQuery { Field = Infer.Field<MethodLog>(f => f.NodePath) }
                                );
                            }
                        }

                        if (filterList.Count != 0)
                        {
                            boolQuery.Filter = filterList;
                        }

                        return boolQuery;
                    })
                )
            );
    }
}
