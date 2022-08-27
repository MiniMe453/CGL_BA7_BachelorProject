using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Can;
using Rover.Settings;
using Unity.UI;
using AwesomeCharts;

public class TemperatureSensor_Reader : MonoBehaviourCan
{
    private List<float> temperatureData = new List<float>();
    public LineChart chart;
    public override void OnCANFrameRecieved(CanFrame frame)
    {
        List<LineEntry> lineEntries = chart.GetChartData().DataSets[0].Entries;
        temperatureData.Insert(0, (float)frame.data[0]);
        Debug.Log((float)frame.data[0] + " | " + node.TimeSinceLastMessage);

        LineEntry lineEntry = new LineEntry();
        lineEntry.Value = (float)frame.data[0];
        lineEntries.Insert(0, lineEntry);

        if (lineEntries.Count > 52)
        {
            lineEntries.RemoveAt(lineEntries.Count - 1);
        }

        for (int i = 0; i < chart.GetChartData().DataSets[0].Entries.Count; i++)
        {
            lineEntries[i].Position = chart.AxisConfig.HorizontalAxisConfig.Bounds.Max - i;
        }

        chart.SetDirty();
    }
}
