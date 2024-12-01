using CustomCompiledQueries;
using JasperFx.CodeGeneration;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.AspNetCore.Mvc;
using Oakton;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ApplyOaktonExtensions();

builder.Services.AddMarten(options =>
{
    options.Connection("server=localhost;port=5432;database=poc-compiledqueries;user id=postgres;password=local;");
    options.UseNewtonsoftForSerialization(enumStorage: EnumStorage.AsString, nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters);

    options.Projections.Add(new BarCustomSimpleProjection(), ProjectionLifecycle.Inline);

    options.Schema.For<Bar>();

    options.RegisterCompiledQueryType(typeof(BarCompiledQuery));
    options.RegisterCompiledQueryType(typeof(BarListCompiledQuery));
    options.RegisterCompiledQueryType(typeof(BarExistsCompiledQuery));
})
    .InitializeWith(new BarInitialData())
    .UseLightweightSessions()
    .OptimizeArtifactWorkflow(TypeLoadMode.Static)
    .AddAsyncDaemon(DaemonMode.HotCold);

var app = builder.Build();

app.MapGet("/",async ([FromServices] IQuerySession session, CancellationToken ct) =>
{
    var bar = await session.QueryAsync(new BarCompiledQuery {Name = "foo"}, ct);
    return bar;
});

app.MapGet("/bars",async ([FromServices] IQuerySession session, CancellationToken ct) =>
{
    var bars = await session.QueryAsync(new BarListCompiledQuery(), ct);
    return bars;
});

app.MapGet("/bars-any",async ([FromServices] IQuerySession session, CancellationToken ct) =>
{
    var bars = await session.QueryAsync(new BarExistsCompiledQuery
    {
        Name = "active bar"
    }, ct);
    return bars;
});

app.MapPost("/bars", async ([FromServices] IDocumentSession session, CancellationToken ct) =>
{
    var @event = new BarCreated(Guid.NewGuid(), "foo", Status.Active);
    session.Events.StartStream<BarAggregate>(@event.Id, @event);

    await session.SaveChangesAsync(ct);

    var bar = await session.Events.AggregateStreamAsync<BarAggregate>(@event.Id, token: ct);
    return bar;
});

await app.RunOaktonCommands(args);
