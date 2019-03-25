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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int value = Random.Range(0, 300);

            valueList.Add(value);
            lineGraph.AddValue(valueList.Count.ToString(), value);
        }
    }
}
