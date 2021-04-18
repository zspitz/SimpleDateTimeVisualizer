# SimpleDateTimeVisualizer
Simplest possible debugging visualizer to illustrate https://github.com/zspitz/DateTimeVisualizer/issues/7

The visualizer targets [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime).

It's supposed to transfer a number to the debuggee side, along with the target value:

```csharp
// https://github.com/zspitz/SimpleDateTimeVisualizer/blob/main/Debugger/Visualizer.cs

var response = objectProvider.TransferObject(5);
```

The debuggee side should create an array containing the target date; the length of the array is the number received:

```csharp
// https://github.com/zspitz/SimpleDateTimeVisualizer/blob/main/Debuggee/VisualizerObjectSource.cs

int? repetitions = Deserialize(incomingData) switch {
    int i when i > 0 => i,
    string s when int.TryParse(s, out int i) && i > 0 => i,
    _ => null
};

object toSerialize =
    repetitions is null ? $"Invalid value for repetitions" :
    target switch {
        DateTime dt => Repeat(dt, repetitions.Value).ToArray(),
        null => $"{nameof(target)} is null",
        _ => $"Not implemented for target of type {target.GetType().FullName}" as object
    };
```

If everything works properly, the debugger side should receive the serialized/deserialized array, and display its contents, `Join`ed into a single string:

```csharp
// https://github.com/zspitz/SimpleDateTimeVisualizer/blob/main/Debugger/Visualizer.cs

var msg = response switch {
    string s => s,
    IEnumerable e => string.Join(", ", e.Cast<object>()),
    _ => "Unhandled type"
};

MessageBox.Show(msg);
```

Everything works fine when the expression's type is `object`. But when the expression's type is of `DateTime`, the `DateTime` itself isn't passed to the debuggee side:

![Visualizer on value type](https://user-images.githubusercontent.com/312166/115158106-426dde80-a095-11eb-8250-86d729e74a72.gif)

Some notes:

* It doesn't seem to be a mismatch between 32-bit debugger and 64-bit debuggee; nor does it matter what the TFM of the target process is. 
* The problem is only when overriding `TransferData`; overriding `GetData` doesn't seem to have this problem. This suggests a possible workaround: call `GetData` to return the target value, and then pass the target value into `TransferData`.
* A visualizer targeting `int` seems to have a similar problem, except that in this case the target process crashes.

To reproduce:

* Build both the Debugger and Debuggee projects
* Copy the contents of `Debugger\bin\Debug\net48` to `Documents\Visual Studio 2019\Visualizers`
* Copy the contents of `Debuggee\bin\Debug` to `Documents\Visual Studio 2019\Visualizers`
* Run the test project. There is a source-code breakpoint which will be hit.
