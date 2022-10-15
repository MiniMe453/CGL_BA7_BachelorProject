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
    private List<CanFrame> temperatureData = new List<CanFrame>();
    public LineChart chart;
    public GameObject listView;
    public TextMeshProUGUI dateTimeText;
    public List<TSCU_List_Entry> list_Entries = new List<TSCU_List_Entry>();
    public bool isGraphPaused = false;
    public bool showListView = false;

    protected override void Init()
    {
        TimeManager.EOnDateTimeUpdated += UpdateDateTimeText;
    }

    void OnDisable()
    {
        TimeManager.EOnDateTimeUpdated -= UpdateDateTimeText;
    }

    private void UpdateDateTimeText(DateTimeStruct newDateTime)
    {
        dateTimeText.text = TimeManager.TimeToStringFull(newDateTime);
    }
    public override void OnCANFrameRecieved(CanFrame frame)
    {
        if (isGraphPaused)
            return;

        temperatureData.Insert(0, frame);
        if (temperatureData.Count > 52)
            temperatureData.RemoveAt(temperatureData.Count - 1);

        if (showListView)
            UpdateListView();
        else
            UpdateGraphView((float)frame.data[0]);
    }

    public void ClearGraph()
    {
        chart.GetChartData().DataSets[0].Entries.Clear();
        chart.SetDirty();
    }

    private void UpdateGraphView(float newValue)
    {
        List<LineEntry> lineEntries = chart.GetChartData().DataSets[0].Entries;

        LineEntry lineEntry = new LineEntry();
        lineEntry.Value = (float)newValue;
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

    private void UpdateListView()
    {
        for (int i = 0; i < temperatureData.Count; i++)
        {
            if (i >= list_Entries.Count)
                break;

            CanFrame tData = temperatureData[i];
            float temperature = Mathf.Round((float)tData.data[0] * 100) / 100f;

            float delta = 0;
            if (i > 0)
            {
                delta = (float)temperatureData[i].data[0] - (float)temperatureData[i - 1].data[0];
                delta = Mathf.Round(delta * 100) / 100f;
            }

            list_Entries[i].SetVariables(TimeManager.TimeToStringTime(tData.timestamp), temperature.ToString(), delta.ToString(), "GOOD");
        }
    }

    public void SwitchView()
    {
        showListView = !showListView;

        if (showListView)
        {
            chart.gameObject.SetActive(false);
            listView.SetActive(true);
        }
        else
        {
            chart.gameObject.SetActive(true);
            listView.SetActive(false);
        }
    }
}
