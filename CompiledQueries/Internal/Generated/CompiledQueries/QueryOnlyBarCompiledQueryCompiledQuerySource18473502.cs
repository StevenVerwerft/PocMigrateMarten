// <auto-generated/>
#pragma warning disable
using CustomCompiledQueries;
using Marten.Linq;
using Marten.Linq.QueryHandlers;
using System;

namespace Marten.Generated.CompiledQueries
{
    // START: QueryOnlyBarCompiledQueryCompiledQuery18473502
    public class QueryOnlyBarCompiledQueryCompiledQuery18473502 : Marten.Internal.CompiledQueries.ClonedCompiledQuery<CustomCompiledQueries.Bar, CustomCompiledQueries.BarCompiledQuery>
    {
        private readonly Marten.Linq.QueryHandlers.IMaybeStatefulHandler _inner;
        private readonly CustomCompiledQueries.BarCompiledQuery _query;
        private readonly Marten.Linq.QueryStatistics _statistics;

        public QueryOnlyBarCompiledQueryCompiledQuery18473502(Marten.Linq.QueryHandlers.IMaybeStatefulHandler inner, CustomCompiledQueries.BarCompiledQuery query, Marten.Linq.QueryStatistics statistics) : base(inner, query, statistics)
        {
            _inner = inner;
            _query = query;
            _statistics = statistics;
        }



        public override void ConfigureCommand(Weasel.Postgresql.ICommandBuilder builder, Marten.Internal.IMartenSession session)
        {
            var parameters1 = builder.AppendWithParameters(@"select d.data from public.mt_doc_bar as d where (d.data ->> 'Name' = ^ and d.data ->> 'Status' = ^) LIMIT ^;", '^');

            parameters1[0].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text;
            parameters1[0].Value = _query.Name;
            parameters1[1].Value = "Active";
            parameters1[1].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
            parameters1[2].Value = 1;
            parameters1[2].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;
        }

    }

    // END: QueryOnlyBarCompiledQueryCompiledQuery18473502
    
    
    // START: QueryOnlyBarCompiledQueryCompiledQuerySource18473502
    public class QueryOnlyBarCompiledQueryCompiledQuerySource18473502 : Marten.Internal.CompiledQueries.CompiledQuerySource<CustomCompiledQueries.Bar, CustomCompiledQueries.BarCompiledQuery>
    {
        private readonly Marten.Linq.QueryHandlers.IMaybeStatefulHandler _maybeStatefulHandler;

        public QueryOnlyBarCompiledQueryCompiledQuerySource18473502(Marten.Linq.QueryHandlers.IMaybeStatefulHandler maybeStatefulHandler)
        {
            _maybeStatefulHandler = maybeStatefulHandler;
        }



        public override Marten.Linq.QueryHandlers.IQueryHandler<CustomCompiledQueries.Bar> BuildHandler(CustomCompiledQueries.BarCompiledQuery query, Marten.Internal.IMartenSession session)
        {
            return new Marten.Generated.CompiledQueries.QueryOnlyBarCompiledQueryCompiledQuery18473502(_maybeStatefulHandler, query, null);
        }

    }

    // END: QueryOnlyBarCompiledQueryCompiledQuerySource18473502
    
    
}

