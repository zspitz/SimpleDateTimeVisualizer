using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Windows;

[assembly: DebuggerVisualizer(
        visualizer: typeof(SimpleDateTimeVisualizer.Visualizer),
        visualizerObjectSource: typeof(SimpleDateTimeVisualizer.Debuggee.VisualizerObjectSource),
        Target = typeof(DateTime),
        Description = "Simple DateTime visualizer")]

namespace SimpleDateTimeVisualizer {
    public class Visualizer : DialogDebuggerVisualizer {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider) {
            var response = objectProvider.TransferObject(5);

            var msg = response switch {
                string s => s,
                IEnumerable e => string.Join(", ", e.Cast<object>()),
                _ => "Unhandled type"
            };

            MessageBox.Show(msg);
        }
    }
}
