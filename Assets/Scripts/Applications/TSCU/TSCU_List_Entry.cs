using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TSCU_List_Entry : MonoBehaviour
{
    public TextMeshProUGUI timeStamp;
    public TextMeshProUGUI temp;
    public TextMeshProUGUI delta;
    public TextMeshProUGUI status;

    public void SetVariables(string ts, string tmp, string dlt, string stat)
    {
        timeStamp.text = ts;
        temp.text = tmp;
        delta.text = dlt;
        status.text = stat;
    }
}
