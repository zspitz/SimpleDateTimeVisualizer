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

[assembly: DebuggerVisualizer(
        visualizer: typeof(SimpleDateTimeVisualizer.Visualizer),
        visualizerObjectSource: typeof(SimpleDateTimeVisualizer.Debuggee.VisualizerObjectSource),
        Target = typeof(int),
        Description = "Simple DateTime visualizer")]


namespace SimpleDateTimeVisualizer {
    public class Visualizer : DialogDebuggerVisualizer {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider) => 
            MessageBox.Show(
                objectProvider.TransferObject(5) switch {
                    string s => s,
                    IEnumerable e => string.Join(", ", e.Cast<object>()),
                    _ => "Unhandled type"
                }
            );
    }
}
