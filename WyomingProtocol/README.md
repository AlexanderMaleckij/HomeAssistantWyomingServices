## WyomingProtocol

This project is a library for building a Home Assistant–compatible Wyoming protocol server.
Cross-platform. Native AOT-compatible. Built with extensibility in mind.

### Minimal Setup

```csharp
var services = new ServiceCollection();

services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
services.AddWyomingProtocol();

services.AddWyomingEventHandler<YourEventHandler>("handler_id");

var serviceProvider = services.BuildServiceProvider();
var serverFactory = serviceProvider.GetRequiredService<IWyomingServerFactory>();
var server = serverFactory.Create("handler_id");

await server.RunAsync(IPAddress.Parse("0.0.0.0"), 10300, CancellationToken.None);

sealed class YourEventHandler : IWyomingEventHandler
{
	public async Task HandleAsync(IWyomingEvent wyomingEvent, IWyomingRequestContext context, CancellationToken cancellationToken)
	{
		switch (wyomingEvent)
        {
			// Implementation goes here...
		}
	}
}
```

The handler is registered as a scoped service. Wyoming server implementation creates a scope for each client connection.
The handler may additionally implement `IDisposable` or `IAsyncDisposable`. The server handles the cleanup.

### Supported events

The following Wyoming events are supported out of the box:

 - `describe`
 - `info`
 - `synthesize`
 - `synthesize-start`
 - `synthesize-chunk`
 - `synthesize-stop`
 - `synthesize-stopped`
 - `transcribe`
 - `transcript`
 - `audio-start`
 - `audio-chunk`
 - `audio-stop`
 - `recognize`
 - `intent`
 - `not-recognized`
 - `intents-start`
 - `intents-stop`
 - `handled`
 - `not-handled`
 - `handled-start`
 - `handled-chunk`
 - `handled-stop`

### Extensibility

The following code snippet demonstrates how to add an event not listed above (custom event).

```csharp
// Register a converter for your event so it can be used in the event handler.
services.AddWyomingEventConverter<CustomEventConverter>("custom-event");

sealed class CustomEvent : IWyomingEvent
{
    public string Type => "custom-event";

    // Optionally, define properties for data and payload.
}

sealed class CustomEventConverter : EventConverterBase<CustomEvent>
{
    public override CustomEvent Convert(RawWyomingEvent @event)
    {
        // Parse raw Wyoming event into class.
    }

    public override RawWyomingEvent Convert(CustomEvent @event)
    {
        // Convert class into raw Wyoming event.
    }
}
```
