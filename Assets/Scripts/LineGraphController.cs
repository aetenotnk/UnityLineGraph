using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineGraphController : MonoBehaviour
{
    // グラフを表示する範囲
    private RectTransform viewport;
    // グラフの要素を配置するContent
    // グラフの要素はグラフの点、ライン
    private RectTransform content;

    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject xUnitLabel;
    private GameObject yUnitLabel;

    private void Awake()
    {
        viewport = this.transform.Find("Viewport") as RectTransform;
        content = viewport.Find("Content") as RectTransform;
        xAxis = this.transform.Find("X Axis").gameObject;
        yAxis = this.transform.Find("Y Axis").gameObject;
        xUnitLabel = this.transform.Find("X Unit Label").gameObject;
        yUnitLabel = this.transform.Find("Y Unit Label").gameObject;
    }

    private void Start()
    {
        InitializeAxis();
    }

    /// <summary>
    /// X軸、Y軸の位置、サイズを設定
    /// </summary>
    private void InitializeAxis()
    {
        Vector2 origin = content.position;
        RectTransform xAxisTransform =
            xAxis.GetComponent<RectTransform>();
        RectTransform yAxisTransform =
            yAxis.GetComponent<RectTransform>();

        xAxis.transform.position = origin;
        yAxis.transform.position = origin;
        xAxisTransform.sizeDelta =
            new Vector2(viewport.rect.width, xAxisTransform.sizeDelta.y);
        yAxisTransform.sizeDelta =
            new Vector2(viewport.rect.height, yAxisTransform.sizeDelta.y);

        RectTransform rectTransform = this.transform as RectTransform;
        Vector2 xPadding = new Vector2(-5, 5);
        Vector2 yPadding = new Vector2(5, -5);
        Vector2 rightBottom =
            new Vector2(rectTransform.sizeDelta.x, 0) + xPadding;
        Vector2 leftTop =
            new Vector2(0, rectTransform.sizeDelta.y) + yPadding;

        ((RectTransform)xUnitLabel.transform).localPosition = rightBottom;
        ((RectTransform)yUnitLabel.transform).localPosition = leftTop;
    }
}
