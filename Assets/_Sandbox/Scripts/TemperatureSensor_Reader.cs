using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Can;
using Rover.Settings;
using Rover.DateTime;
using Unity.UI;
using AwesomeCharts;
using TMPro;

public class TemperatureSensor_Reader : MonoBehaviourCan
{
    private List<float> temperatureData = new List<float>();
    public LineChart chart;
    public TextMeshProUGUI dateTimeText;
    public bool isGraphPaused = false;

    protected override void Init()
    {
        TimeManager.EOnDateTimeUpdated += UpdateDateTimeText;
    }

    void OnDisable()
    {
        TimeManager.EOnDateTimeUpdated -= UpdateDateTimeText;
    }

    private void UpdateDateTimeText(string newDateTime)
    {
        dateTimeText.text = newDateTime;
    }
    public override void OnCANFrameRecieved(CanFrame frame)
    {
        List<LineEntry> lineEntries = chart.GetChartData().DataSets[0].Entries;
        temperatureData.Insert(0, (float)frame.data[0]);

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

        if (!isGraphPaused)
            chart.SetDirty();
    }

    public void ClearGraph()
    {
        chart.GetChartData().DataSets[0].Entries.Clear();
        chart.SetDirty();
    }
}
