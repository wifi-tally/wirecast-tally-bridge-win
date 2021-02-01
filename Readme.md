# WirecastTallyBridge

Prototype of something that could integrate Wirecast into [WiFi Tally Light](https://wifi-tally.github.io/).

Basic idea: A C#/.NET application that uses Wirecast OLE API to wrap it into a (seemingly) event driven
TCP API. This obviously only works for Windows and has no use on Mac.

## Development

Download [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48).

Enter the directory with this source code and run `dotnet run`.


## Open Questions

* It will start Wirecast if it is not running. Even when you are closing it, it will pop up again.
* Hiding a layer will still state that a shot is in preview or live. Vice-versa it is not possible to determine if a "live" shot in a hidden layer is actually hidden (it could be visible if the layer was hidden and no transition has taken place since then).
* During transitions only the _next_ shot is considered to be live. So the shot that is faded out will appear in preview (or released) even though it could be still visible during the transition.
* Question: Can Wirecast.cs be distributed under an OS License?