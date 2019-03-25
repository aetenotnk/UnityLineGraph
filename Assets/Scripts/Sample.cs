using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    LineGraphController lineGraph;
    List<int> valueList;

    void Start()
    {
        lineGraph = GameObject.Find("LineGraph").GetComponent<LineGraphController>();

        valueList = new List<int>()
        {
            5, 10, 7, 1, 20, 100
        };

        for (int i = 0; i < valueList.Count; i++)
        {
            lineGraph.AddValue((i + 1).ToString(), valueList[i]);
        }

        lineGraph.SetXUnitText("時間(s)");
        lineGraph.SetYUnitText("個体数");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int value = Random.Range(0, 300);

            valueList.Add(value);
            lineGraph.AddValue(valueList.Count.ToString(), value);
        }

        // xSize: 50, ySize: 5, yAxisSeparatorSpan: 10, xValueSpan: 1
        if (Input.GetKeyDown(KeyCode.Z))
        {
            lineGraph.ChangeParam(10, 5, 10, 1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            lineGraph.ChangeParam(50, 1, 10, 1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            lineGraph.ChangeParam(50, 5, 50, 1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            lineGraph.ChangeParam(50, 5, 10, 5);
        }
    }
}
