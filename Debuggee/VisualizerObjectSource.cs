using System;
using System.IO;
using static System.Linq.Enumerable;

namespace SimpleDateTimeVisualizer.Debuggee {
    public class VisualizerObjectSource : Microsoft.VisualStudio.DebuggerVisualizers.VisualizerObjectSource {
        public override void TransferData(object target, Stream incomingData, Stream outgoingData) {
            int? repetitions = Deserialize(incomingData) switch {
                int i when i > 0 => i,
                string s when int.TryParse(s, out int i) && i > 0 => i,
                _ => null
            };

            object toSerialize =
                repetitions is null ? $"Invalid value for repetitions" :
                target switch {
                    int i => Repeat(i, repetitions.Value).ToArray(),
                    DateTime dt => Repeat(dt, repetitions.Value).ToArray(),
                    null => $"{nameof(target)} is null",
                    _ => $"Not implemented for target of type {target.GetType().FullName}" as object
                };

            Serialize(outgoingData, toSerialize);
        }
    }
}
