using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LineGraphController : MonoBehaviour
{
    [SerializeField]
    private Sprite dotSprite;
    [SerializeField]
    private Font font;

    // グラフを表示する範囲
    private RectTransform viewport;
    // グラフの要素を配置するContent
    // グラフの要素はグラフの点、ライン
    private RectTransform content;

    // 軸のGameObject
    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject xUnitLabel;
    private GameObject yUnitLabel;

    private float xSize = 10;
    private float ySize = 5;
    private GameObject previousDot;

    private List<KeyValuePair<string, int>> valueList;

    private enum ZOrder
    {
        CONNECTION,
        DOT,
        LABEL
    };

    private void Awake()
    {
        viewport = this.transform.Find("Viewport") as RectTransform;
        content = viewport.Find("Content") as RectTransform;
        xAxis = this.transform.Find("X Axis").gameObject;
        yAxis = this.transform.Find("Y Axis").gameObject;
        xUnitLabel = this.transform.Find("X Unit Label").gameObject;
        yUnitLabel = this.transform.Find("Y Unit Label").gameObject;
        valueList = new List<KeyValuePair<string, int>>()
        {
            new KeyValuePair<string, int>("1", 5),
            new KeyValuePair<string, int>("2", 10),
            new KeyValuePair<string, int>("3", 7),
            new KeyValuePair<string, int>("4", 1),
            new KeyValuePair<string, int>("5", 20)
        };
    }

    private void Start()
    {
        InitializeAxis();
        for(int i = 0;i < valueList.Count; i++)
        {
            int value = valueList[i].Value;
            GameObject dot = CreateNewDot(i, value);

            if (previousDot)
            {
                RectTransform rectTransform1 =
                    previousDot.GetComponent<RectTransform>();
                RectTransform rectTransform2 =
                    dot.GetComponent<RectTransform>();

                CreateConnection(
                        rectTransform1.anchoredPosition,
                        rectTransform2.anchoredPosition);
            }

            CreateValueLabelByDot(i, value);
            CreateXLabel(i, valueList[i].Key);

            previousDot = dot;
        }
        FixContentSize();
    }

    /// <summary>
    /// 値を追加する
    /// </summary>
    /// <param name="label">ラベルの文字列</param>
    /// <param name="value">値</param>
    public void AddValue(string label, int value)
    {
        valueList.Add(new KeyValuePair<string, int>(label, value));
    }

    /// <summary>
    /// グラフがスクロールされた時の処理
    /// </summary>
    /// <param name="scrollPosition">スクロールの位置</param>
    public void OnGraphScroll(Vector2 scrollPosition)
    {
        Vector2 contentSize = content.sizeDelta;
        Vector2 viewportSize =
            new Vector2(viewport.rect.width, viewport.rect.height);
        Vector2 maxScrollSize = contentSize - viewportSize;
        Vector2 scroll =
            new Vector2(
                    maxScrollSize.x * scrollPosition.x,
                    maxScrollSize.y * scrollPosition.y);

        FixXLabelPosition(new Vector2(content.anchoredPosition.x, 0));
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

    /// <summary>
    /// 新しい点を作成する
    /// </summary>
    /// <returns>The new dot.</returns>
    /// <param name="index">X軸方向で何個目か</param>
    /// <param name="value">Y軸方向の値</param>
    private GameObject CreateNewDot(int index, int value)
    {
        GameObject dot = new GameObject("dot", typeof(Image));
        Image image = dot.GetComponent<Image>();
        image.useSpriteMesh = true;
        image.sprite = dotSprite;
        RectTransform rectTransform = dot.GetComponent<RectTransform>();
        rectTransform.SetParent(content);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.localScale = Vector2.one;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchoredPosition =
            new Vector2((index + 1) * xSize, value * ySize);
        rectTransform.SetSiblingIndex((int)ZOrder.DOT);

        return dot;
    }

    /// <summary>
    /// 点と点をつなぐ線を作成する
    /// </summary>
    /// <param name="pos1">点1の位置</param>
    /// <param name="pos2">点2の位置</param>
    private void CreateConnection(Vector2 pos1, Vector2 pos2)
    {
        GameObject connection = new GameObject("connection", typeof(Image));
        RectTransform rectTransform = connection.GetComponent<RectTransform>();
        rectTransform.SetParent(content);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.localScale = Vector2.one;
        Vector2 dir = (pos2 - pos1).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float distance = Vector2.Distance(pos1, pos2);
        rectTransform.sizeDelta = new Vector2(distance, 2);
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
        rectTransform.anchoredPosition = pos1 + dir * distance * 0.5f;
        rectTransform.SetSiblingIndex((int)ZOrder.CONNECTION);
    }

    /// <summary>
    /// 点の近くに値のラベルを表示する
    /// </summary>
    /// <param name="index">X軸方向で何個目か</param>
    /// <param name="value">Y軸方向の値</param>
    private void CreateValueLabelByDot(int index, int value)
    {
        GameObject label = new GameObject("label", typeof(Text));
        Text text = label.GetComponent<Text>();
        text.text = value.ToString();
        text.alignment = TextAnchor.MiddleCenter;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.fontSize = 10;
        text.font = font;
        text.color = Color.black;
        Vector2 offset = new Vector2(0, 8);
        RectTransform rectTransform = label.GetComponent<RectTransform>();
        rectTransform.SetParent(content);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.localScale = Vector2.one;
        rectTransform.anchoredPosition =
            new Vector2((index + 1) * xSize, value * ySize) + offset;
        rectTransform.SetSiblingIndex((int)ZOrder.LABEL);
    }

    /// <summary>
    /// Contentのサイズを調整する
    /// </summary>
    private void FixContentSize()
    {
        Vector2 buffer = new Vector2(10, 10);
        float width = (valueList.Count + 1) * xSize;
        float height = GetMaxValue() * ySize;

        content.sizeDelta = new Vector2(width, height) + buffer;
    }

    /// <summary>
    /// 現在の最大値を取得する
    /// </summary>
    /// <returns>最大値</returns>
    private int GetMaxValue()
    {
        int max = int.MinValue;

        foreach(KeyValuePair<string, int>pair in valueList)
        {
            max = max < pair.Value ? pair.Value : max;
        }

        return max;
    }

    /// <summary>
    /// X軸方向のラベルを作成する
    /// </summary>
    /// <param name="index">X軸方向で何個目か</param>
    /// <param name="labelText">表示するラベルのテキスト</param>
    private void CreateXLabel(int index, string labelText)
    {
        GameObject label = new GameObject("xLabel(" + index + ")", typeof(Text));
        Text text = label.GetComponent<Text>();
        text.text = labelText;
        text.alignment = TextAnchor.UpperCenter;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.fontSize = 10;
        text.font = font;
        text.color = Color.black;
        Vector2 origin = xAxis.GetComponent<RectTransform>().anchoredPosition;
        Vector2 offset = new Vector2(0, -5);
        RectTransform rectTransform = label.GetComponent<RectTransform>();
        rectTransform.SetParent(this.transform);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.localScale = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition =
                origin + new Vector2((index + 1) * xSize, 0) + offset;
    }

    /// <summary>
    /// X軸の外にあるラベルを非表示にする
    /// </summary>
    /// <param name="diffPosition">元の一からどれだけずれているか</param>
    private void FixXLabelPosition(Vector2 diffPosition)
    {
        RectTransform xAxisRect = xAxis.GetComponent<RectTransform>();
        Vector2 origin = xAxisRect.anchoredPosition;
        Vector2 xLimit = origin + new Vector2(xAxisRect.sizeDelta.x, 0);

        for(int i = 0;i < this.transform.childCount; i++)
        {
            RectTransform child = this.transform.GetChild(i) as RectTransform;

            if(child == null)
            {
                continue;
            }

            Match match = Regex.Match(child.name, "^xLabel\\(([0-9]+)\\)$");

            if (match.Groups.Count > 1)
            {
                int index = int.Parse(match.Groups[1].Value);
                float x = origin.x + (index + 1) * xSize;
                float y = child.anchoredPosition.y;
                Vector2 basePosition = new Vector2(x, y);
                Vector2 position = basePosition + diffPosition;

                child.anchoredPosition = position;
                child.gameObject.SetActive(
                        origin.x <= position.x &&
                        position.x <= xLimit.x);
            }
        }
    }
}
