// <auto-generated/>
#pragma warning disable
using CustomCompiledQueries;
using Marten.Linq;
using Marten.Linq.QueryHandlers;
using System;

namespace Marten.Generated.CompiledQueries
{
    // START: DirtyTrackingBarListCompiledQueryCompiledQuery1925507866
    public class DirtyTrackingBarListCompiledQueryCompiledQuery1925507866 : Marten.Internal.CompiledQueries.ClonedCompiledQuery<System.Collections.Generic.IEnumerable<CustomCompiledQueries.Bar>, CustomCompiledQueries.BarListCompiledQuery>
    {
        private readonly Marten.Linq.QueryHandlers.IMaybeStatefulHandler _inner;
        private readonly CustomCompiledQueries.BarListCompiledQuery _query;
        private readonly Marten.Linq.QueryStatistics _statistics;

        public DirtyTrackingBarListCompiledQueryCompiledQuery1925507866(Marten.Linq.QueryHandlers.IMaybeStatefulHandler inner, CustomCompiledQueries.BarListCompiledQuery query, Marten.Linq.QueryStatistics statistics) : base(inner, query, statistics)
        {
            _inner = inner;
            _query = query;
            _statistics = statistics;
        }



        public override void ConfigureCommand(Weasel.Postgresql.ICommandBuilder builder, Marten.Internal.IMartenSession session)
        {
            var parameters1 = builder.AppendWithParameters(@"select d.id, d.data from public.mt_doc_bar as d where (d.data ->> 'Status' = ^ or d.data ->> 'Status' = ^);", '^');
            parameters1[0].Value = "Active";
            parameters1[0].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
            parameters1[1].Value = "Inactive";
            parameters1[1].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
        }

    }

    // END: DirtyTrackingBarListCompiledQueryCompiledQuery1925507866
    
    
    // START: DirtyTrackingBarListCompiledQueryCompiledQuerySource1925507866
    public class DirtyTrackingBarListCompiledQueryCompiledQuerySource1925507866 : Marten.Internal.CompiledQueries.CompiledQuerySource<System.Collections.Generic.IEnumerable<CustomCompiledQueries.Bar>, CustomCompiledQueries.BarListCompiledQuery>
    {
        private readonly Marten.Linq.QueryHandlers.IMaybeStatefulHandler _maybeStatefulHandler;

        public DirtyTrackingBarListCompiledQueryCompiledQuerySource1925507866(Marten.Linq.QueryHandlers.IMaybeStatefulHandler maybeStatefulHandler)
        {
            _maybeStatefulHandler = maybeStatefulHandler;
        }



        public override Marten.Linq.QueryHandlers.IQueryHandler<System.Collections.Generic.IEnumerable<CustomCompiledQueries.Bar>> BuildHandler(CustomCompiledQueries.BarListCompiledQuery query, Marten.Internal.IMartenSession session)
        {
            return new Marten.Generated.CompiledQueries.DirtyTrackingBarListCompiledQueryCompiledQuery1925507866(_maybeStatefulHandler, query, null);
        }

    }

    // END: DirtyTrackingBarListCompiledQueryCompiledQuerySource1925507866
    
    
}

