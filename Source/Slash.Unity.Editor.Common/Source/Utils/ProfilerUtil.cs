namespace Slash.Unity.Editor.Common.Utils
{
    using System.Collections;
    using System.IO;
    using System.Reflection;

    using UnityEditor;

    using UnityEditorInternal;
#if UNITY_2018_1_OR_NEWER
    using ProfilerColumn = UnityEditorInternal.Profiling.ProfilerColumn;
    using ProfilerViewType = UnityEditorInternal.Profiling.ProfilerViewType;
#endif

    public class ProfilerUtil
    {
        private const string HtmlOutputPath = "profiler.html";

        [MenuItem("Slash/Profiler/Dump selected profiler frame to HTML")]
        public static void DumpProfilerFrame()
        {
            var property = new ProfilerProperty();
            property.SetRoot(GetSelectedFrame(), ProfilerColumn.TotalPercent, ProfilerViewType.Hierarchy);
            property.onlyShowGPUSamples = false;

            if (File.Exists(HtmlOutputPath))
            {
                File.Delete(HtmlOutputPath);
            }
            var stream = File.OpenWrite(HtmlOutputPath);
            var writer = new StreamWriter(stream);

            writer.WriteLine(@"<html>
<head>
<title>Unity Profiler Data</title>
<style type=""text/css"">
html, body {
font-family: Helvetica, Arial, sans-serif;
}
table {
width: 100%;
border-collapse: collapse;
}
th:first-child, td:first-child {
text-align: left;
}
th:not(:first-child), td:not(:first-child) {
text-align: right;
}
tbody tr:nth-child(odd) {
background-color: #EEE;
}
th, td {
margin: 0;
padding: 5px;
}
th {
padding-bottom: 10px;
}
td {
font-size: 12px;
}
</style>
</head>
<body>
<table>
<thead>
<tr><th>Path</th><th>Total</th><th>Self</th><th>Calls</th><th>GC Alloc</th><th>Total ms</th><th>Self ms</th></tr>
</thead>
<tbody>");

            while (property.Next(true))
            {
                writer.Write("<td style=\"padding-left:" + property.depth * 10 + "px\">");
                writer.Write(property.GetColumn(ProfilerColumn.FunctionName));
                writer.Write("</td>");

                writer.Write("<td>");
                writer.Write(property.GetColumn(ProfilerColumn.TotalPercent));
                writer.Write("</td>");

                writer.Write("<td>");
                writer.Write(property.GetColumn(ProfilerColumn.SelfPercent));
                writer.Write("</td>");

                writer.Write("<td>");
                writer.Write(property.GetColumn(ProfilerColumn.Calls));
                writer.Write("</td>");

                writer.Write("<td>");
                writer.Write(property.GetColumn(ProfilerColumn.GCMemory));
                writer.Write("</td>");

                writer.Write("<td>");
                writer.Write(property.GetColumn(ProfilerColumn.TotalTime));
                writer.Write("</td>");

                writer.Write("<td>");
                writer.Write(property.GetColumn(ProfilerColumn.SelfTime));
                writer.Write("</td>");

                writer.WriteLine("</tr>");
            }

            writer.WriteLine(@"</tbody>
</table>
</body>
</html>");

            writer.Close();
        }

        private static int GetSelectedFrame()
        {
            var editorAssembly = Assembly.GetAssembly(typeof(EditorApplication));
            var profilerWindowType = editorAssembly.GetType("UnityEditor.ProfilerWindow");
            var profilerWindowsField = profilerWindowType.GetField("m_ProfilerWindows",
                BindingFlags.NonPublic | BindingFlags.Static);
            var firstProfilerWindow = ((IList) profilerWindowsField.GetValue(null))[0];
            var getFrameMethod = profilerWindowType.GetMethod("GetActiveVisibleFrameIndex");
            return (int) getFrameMethod.Invoke(firstProfilerWindow, null);
        }
    }
}