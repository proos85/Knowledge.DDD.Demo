using System.Diagnostics;
using System.Reactive.Linq;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot.Events;

namespace Knowledge.DDD.Demo.Infra.Messages;

public static class SnackOrderCompleteEventReceiver
{
    public static void Start()
    {
        MessageQueue.ReceiveObservable()
            .OfType<SnackOrderProcessed>()
            .Subscribe(snackOrderProcessedEvent =>
            {
                Debug.WriteLine("{0}-----------------------------------------------------------", args: Environment.NewLine);

                Debug.WriteLine("A snack order was processed with the following amount: {0}" +
                                "{2}With the following snacks: '{1}'", 
                    snackOrderProcessedEvent.OrderAmount,
                    string.Join(", ", snackOrderProcessedEvent.OrderSnacks.Select(snack => snack.SnackName.Name)),
                    Environment.NewLine);

                Debug.WriteLine("-----------------------------------------------------------{0}", args: Environment.NewLine);
            });
    }
}