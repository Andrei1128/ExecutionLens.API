using ExecutionLens.Domain.Enums;
using ExecutionLens.Domain.Models.Requests;
using Nest;

namespace ExecutionLens.Domain.Extensions;

public static class AdvancedFilterExtensions
{
    public static QueryContainer ToQueryContainer(this List<AdvancedFilter> filters)
    {
        var query = new QueryContainer();

        if (filters is null)
            return query;

        var filterGroups = filters.GroupBy(g => g.Target);

        foreach (var group in filterGroups)
        {
            Field target = GetField(group.Key);

            var queries = new QueryContainer();

            foreach (var filter in group)
            {
                queries |= filter.Operation switch
                {
                    FilterOperation.Is => new TermQuery
                    {
                        Field = $"{target}.{"keyword"}",
                        Value = filter.Value
                    },
                    FilterOperation.Contains => new MatchQuery
                    {
                        Field = target,
                        Query = filter.Value
                    },
                    FilterOperation.Like => new WildcardQuery
                    {
                        Field = target,
                        Wildcard = $"*{filter.Value}*"
                    },
                    FilterOperation.IsNot => !new TermQuery
                    {
                        Field = $"{target}.{"keyword"}",
                        Value = filter.Value
                    },
                    FilterOperation.NotContains => !new MatchQuery
                    {
                        Field = target,
                        Query = filter.Value
                    },
                    FilterOperation.NotLike => !new WildcardQuery
                    {
                        Field = target,
                        Wildcard = $"*{filter.Value}*"
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(filter.Operation), $"Unsupported filter operation!")
                };
            }

            query &= new BoolQuery
            {
                Should = new List<QueryContainer> { queries },
            };
        }

        return query;
    }


    private static Field GetField(FilterTarget target)
    {
        return target switch
        {
            FilterTarget.Input => "input.value",
            FilterTarget.Output => "output.value",
            FilterTarget.Information => "informations.message",
            _ => throw new ArgumentOutOfRangeException(nameof(target), "Unsupported target!"),
        };
    }
}
