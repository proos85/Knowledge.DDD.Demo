using System.Reactive.Subjects;
using Knowledge.DDD.Demo.Kernel.Domain;

namespace Knowledge.DDD.Demo.Infra.Messages;

public static class MessageQueue
{
    private static readonly Subject<IDomainEvent> DomainEventSubject = new();

    public static IObservable<IDomainEvent> ReceiveObservable() => DomainEventSubject;

    public static void PublishDomainEvent(IDomainEvent domainEvent) => DomainEventSubject.OnNext(domainEvent);
}