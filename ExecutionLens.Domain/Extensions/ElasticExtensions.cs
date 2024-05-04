using ExecutionLens.Domain.Enums;
using ExecutionLens.Domain.Models;
using ExecutionLens.Domain.Models.Requests;
using Nest;

namespace ExecutionLens.Domain.Extensions;

public static class ElasticExtensions
{
    public static SearchDescriptor<MethodLog> ApplySort(this SearchDescriptor<MethodLog> descriptor, OrderBy sortOrder)
    {
        return descriptor.Sort(s =>
        {
            switch (sortOrder)
            {
                case OrderBy.Date_ASC:
                    s.Field(f => f.Field(Infer.Field<MethodLog>(ff => ff.EntryTime)).Order(SortOrder.Ascending));
                    break;

                case OrderBy.Date_DESC:
                    s.Field(f => f.Field(Infer.Field<MethodLog>(ff => ff.EntryTime)).Order(SortOrder.Descending));
                    break;

                case OrderBy.Score_ASC:
                    s.Field(f => f.Field("_score").Order(SortOrder.Ascending));
                    break;

                case OrderBy.Score_DESC:
                    s.Field(f => f.Field("_score").Order(SortOrder.Descending));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOrder), "Unsupported sort order!");
            }
            return s;
        });
    }

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

                        if (filters.Classes is not null && filters.Classes.Length != 0)
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Class.Suffix(nameof(ElasticTerm.keyword))),
                                    Terms = filters.Classes
                                }
                            );
                        }

                        if (filters.Methods is not null && filters.Methods.Length != 0)
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Method.Suffix(nameof(ElasticTerm.keyword))),
                                    Terms = filters.Methods
                                }
                            );
                        }

                        if (filters.HasException == BinaryChoice.Yes)
                        {
                            filterList.Add(
                                new TermQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.HasException),
                                    Value = true
                                }
                            );
                        }
                        else if (filters.HasException == BinaryChoice.No)
                        {
                            filterList.Add(
                                 new TermQuery
                                 {
                                     Field = Infer.Field<MethodLog>(f => f.HasException),
                                     Value = false
                                 }
                             );
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

                        if (filters.Classes is not null && filters.Classes.Count != 0)
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Class.Suffix(nameof(ElasticTerm.keyword))),
                                    Terms = filters.Classes
                                }
                            );
                        }

                        if (filters.Methods is not null && filters.Methods.Count != 0)
                        {
                            filterList.Add(
                                new TermsQuery
                                {
                                    Field = Infer.Field<MethodLog>(f => f.Method.Suffix(nameof(ElasticTerm.keyword))),
                                    Terms = filters.Methods
                                }
                            );
                        }

                        if (filters.IsEntryPoint == BinaryChoice.Yes)
                        {
                            filterList.Add(
                                new BoolQuery
                                {
                                    MustNot = new QueryContainer[] { new ExistsQuery { Field = Infer.Field<MethodLog>(f => f.NodePath) } }
                                }
                            );
                        }
                        else if (filters.IsEntryPoint == BinaryChoice.No)
                        {
                            filterList.Add(
                                new ExistsQuery { Field = Infer.Field<MethodLog>(f => f.NodePath) }
                            );
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
