using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    LineGraphController lineGraph;
    List<int> valueList;
    LineGraphController.LineGraphParameter parameter;

    void Start()
    {
        lineGraph = GameObject.Find("LineGraph").GetComponent<LineGraphController>();

        parameter.xSize = 50;
        parameter.ySize = 5;
        parameter.yAxisSeparatorSpan = 10;
        parameter.valueSpan = 1;

        lineGraph.ChangeParam(parameter);

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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            parameter = lineGraph.GetParameter();
            parameter.xSize = 10;
            lineGraph.ChangeParam(parameter);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            parameter = lineGraph.GetParameter();
            parameter.ySize = 1;
            lineGraph.ChangeParam(parameter);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            parameter = lineGraph.GetParameter();
            parameter.yAxisSeparatorSpan = 50;
            lineGraph.ChangeParam(parameter);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            parameter = lineGraph.GetParameter();
            parameter.valueSpan = 5;
            lineGraph.ChangeParam(parameter);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            parameter = lineGraph.GetParameter();
            parameter.xSize = 50;
            parameter.ySize = 5;
            parameter.yAxisSeparatorSpan = 10;
            parameter.valueSpan = 1;
            lineGraph.ChangeParam(parameter);
        }
    }
}
