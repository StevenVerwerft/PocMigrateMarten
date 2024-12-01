using System.Linq.Expressions;
using Marten;
using Marten.Events;
using Marten.Events.Aggregation;
using Marten.Events.Projections;
using Marten.Internal.Sessions;
using Marten.Linq;
using Marten.Metadata;
using Marten.Schema;

namespace CustomCompiledQueries;

public class Bar
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public Status Status { get; set; } = Status.Inactive;
}

public enum Status
{
    Active,
    Inactive,
    Deleted
}

public class BarCompiledQuery : ICompiledQuery<Bar, Bar?>
{
    public string Name { get; set; } = string.Empty;
    public Expression<Func<IMartenQueryable<Bar>, Bar?>> QueryIs()
    {
        return q => q
            .FirstOrDefault(x =>
                x.Name == Name && x.Status == Status.Active);
    }
}

public class BarListCompiledQuery : ICompiledListQuery<Bar>
{
    public Expression<Func<IMartenQueryable<Bar>, IEnumerable<Bar>>> QueryIs()
    {
        return q => q.Where(bar => bar.Status == Status.Active || bar.Status == Status.Inactive);
        // return q => q.Where(bar => bar.Status.In(Status.Active, Status.Inactive));
    }
}

public class BarExistsCompiledQuery : ICompiledQuery<Bar, bool>
{
    public string Name { get; set; } = "";
    public Expression<Func<IMartenQueryable<Bar>, bool>> QueryIs()
    {
        return q => q
            .Where(x => x.Name == Name)
            .Any(x => x.Status == Status.Active);
    }
}

public class BarInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        List<Bar> bars =
        [
            new()
            {
                Status = Status.Active,
                Name = "active bar",
                Id = new("c5fe7aef-3c12-4db7-890e-c04d2d68495d")
            },
            new()
            {
                Status = Status.Inactive,
                Name = "inactive bar",
                Id = new("37945ec3-0c4b-43ba-911b-f08c37bd0ed9")
            },
            new()
            {
                Status = Status.Deleted,
                Name = "deleted bar",
                Id = new("e189c989-268c-447b-af6d-a8c395005eb9")
            },
        ];

        await store.BulkInsertAsync(bars, cancellation: cancellation, mode: BulkInsertMode.OverwriteExisting);
    }
}

public interface IDomainEvent;
public record BarCreated(Guid Id, string Name, Status Status) : IDomainEvent;

public class BarAggregate : ISoftDeleted
{
    public Guid Id { get; set; }
    public Status Status { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    private void Apply(BarCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Status = @event.Status;
    }

    internal void Handle(IDomainEvent @event)
    {
        switch (@event)
        {
            case BarCreated e:
                Apply(e);
                break;
        }
    }
}

public class BarCustomProjection : CustomProjection<BarAggregate, Guid>
{
    public BarCustomProjection()
    {
        AggregateByStream();
    }

    public override ValueTask ApplyChangesAsync(DocumentSessionBase session,
        EventSlice<BarAggregate, Guid> slice,
        CancellationToken cancellation,
        ProjectionLifecycle lifecycle = ProjectionLifecycle.Inline)
    {
        var aggregate = slice.Aggregate;
        foreach (var data in slice.AllData())
        {
            switch (data)
            {
                case BarCreated @event:
                    aggregate = new BarAggregate();
                    aggregate.Handle(@event);
                    break;
                case IDomainEvent @event:
                    if (aggregate is null)
                        break;
                    aggregate.Handle(@event);
                    break;
            }
        }

        if(aggregate is not null)
            session.Store(aggregate);

        return default;
    }
}

public class BarCustomSimpleProjection : CustomProjection<BarAggregate, Guid>
{
    public BarCustomSimpleProjection()
    {
        AggregateByStream();
    }

    public override BarAggregate Apply(BarAggregate? snapshot, IReadOnlyList<IEvent> events)
    {
        snapshot ??= new BarAggregate();
        foreach (var @event in events)
        {
            switch (@event.Data)
            {
                case BarCreated e:
                    snapshot.Handle(e);
                    break;
                case IDomainEvent e:
                    snapshot.Handle(e);
                    break;
            }
        }
        return snapshot;
    }
}
