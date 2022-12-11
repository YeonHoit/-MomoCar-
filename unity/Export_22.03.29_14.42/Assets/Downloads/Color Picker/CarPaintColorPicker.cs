using System;
using UnityEngine;
using UnityEngine.UI;

public class CarPaintColorPicker : MonoBehaviour
{
    [Header("Static")]
    public static CarPaintColorPicker carPaintColorPicker; //전역 참조 변수

    [Header("Variable")]
    public Color color; //색상
    public GameObject satvalGO;
    public GameObject satvalKnob;
    public GameObject hueGO;
    public GameObject hueKnob;

    private Action _update;
    private Action dragH;
    private Action dragSV;
    private Action applyColor;
    private Color[] hueColors;
    private Color[] satvalColors;

    private void Awake()
    {
        carPaintColorPicker = this; //전역 참조 변수 지정
    }

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        /* 텍스쳐 생성 */
        hueColors = new Color[] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta,
        };

        satvalColors = new Color[] {
            new Color( 0f, 0f, 0f ),
            new Color( 0f, 0f, 0f ),
            new Color( 1f, 1f, 1f ),
            hueColors[0],
        };

        var hueTex = new Texture2D(1, 7);
        for (int i = 0; i < 7; i++)
        {
            hueTex.SetPixel(0, i, hueColors[i % 6]);
        }
        hueTex.Apply();
        hueGO.GetComponent<Image>().sprite = Sprite.Create(hueTex, new Rect(0, 0.5f, 1, 6), new Vector2(0.5f, 0.5f));
        var hueSz = GetWidgetSize(hueGO);

        var satvalTex = new Texture2D(2, 2);
        satvalGO.GetComponent<Image>().sprite = Sprite.Create(satvalTex, new Rect(0.5f, 0.5f, 1, 1), new Vector2(0.5f, 0.5f));

        void resetSatValTexture()
        {
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    satvalTex.SetPixel(i, j, satvalColors[i + j * 2]);
                }
            }
            satvalTex.Apply();
        }

        /* RGB 값을 HSV로 변경하여 ColorPicker 수정 */
        var satvalSz = GetWidgetSize(satvalGO);

        RGBToHSV(color, out float Hue, out float Saturation, out float Value);

        void applyHue()
        {
            var i0 = Mathf.Clamp((int)Hue, 0, 5);
            var i1 = (i0 + 1) % 6;
            var resultColor = Color.Lerp(hueColors[i0], hueColors[i1], Hue - i0);
            satvalColors[3] = resultColor;
            resetSatValTexture();

            hueKnob.transform.localPosition = new Vector2(hueKnob.transform.localPosition.x, Hue / 6 * satvalSz.y);
        }

        void applySaturationValue()
        {
            var sv = new Vector2(Saturation, Value);
            var isv = new Vector2(1 - sv.x, 1 - sv.y);
            var c0 = isv.x * isv.y * satvalColors[0];
            var c1 = sv.x * isv.y * satvalColors[1];
            var c2 = isv.x * sv.y * satvalColors[2];
            var c3 = sv.x * sv.y * satvalColors[3];
            var resultColor = c0 + c1 + c2 + c3;
            satvalKnob.transform.localPosition = new Vector2(Saturation * satvalSz.x, Value * satvalSz.y);

            color = resultColor; //색상 갱신
        }

        applyHue();
        applySaturationValue();

        /* 커스텀 함수 */
        dragH = () =>
        {
            GetLocalMouse(hueGO, out Vector2 mp);
            Hue = mp.y / hueSz.y * 6;
            applyHue();
            applySaturationValue();

            if (CarPaint.carPaint.carSelectedPaintPartSurface == 0) CarPaint.carPaint.carSelectedPaintPartSurface = 1; //기본 재질이면 유광으로 변경
            CarPaint.carPaint.carSelectedPaintPartColor = color; //도색 부위 색상 갱신
        };

        dragSV = () =>
        {
            GetLocalMouse(satvalGO, out Vector2 mp);
            Saturation = mp.x / satvalSz.x;
            Value = mp.y / satvalSz.y;
            applySaturationValue();

            if (CarPaint.carPaint.carSelectedPaintPartSurface == 0) CarPaint.carPaint.carSelectedPaintPartSurface = 1; //기본 재질이면 유광으로 변경
            CarPaint.carPaint.carSelectedPaintPartColor = color; //도색 부위 색상 갱신
        };

        applyColor = () => //색상을 적용하는 함수
        {
            RGBToHSV(color, out Hue, out Saturation, out Value);
            applyHue();
            applySaturationValue();
        };
    }

    private void Update()
    {
        if (_update != null) _update();
    }

    /* 이미지에서 포인터를 눌렀을 때 실행되는 함수 */
    public void OnPointerDown()
    {
        if (GetLocalMouse(hueGO, out _))
        {
            _update = dragH;
        }
        else if (GetLocalMouse(satvalGO, out _))
        {
            _update = dragSV;
        }
    }

    /* 이미지에서 포인터를 땠을 때 실행되는 함수 */
    public void OnPointerUp()
    {
        _update = null;
    }

    private static void RGBToHSV(Color color, out float h, out float s, out float v)
    {
        var cmin = Mathf.Min(color.r, color.g, color.b);
        var cmax = Mathf.Max(color.r, color.g, color.b);
        var d = cmax - cmin;
        if (d == 0)
        {
            h = 0;
        }
        else if (cmax == color.r)
        {
            h = Mathf.Repeat((color.g - color.b) / d, 6);
        }
        else if (cmax == color.g)
        {
            h = (color.b - color.r) / d + 2;
        }
        else
        {
            h = (color.r - color.g) / d + 4;
        }
        s = cmax == 0 ? 0 : d / cmax;
        v = cmax;
    }

    private static bool GetLocalMouse(GameObject go, out Vector2 result)
    {
        var rt = (RectTransform)go.transform;
        var mp = rt.InverseTransformPoint(Input.mousePosition);
        result.x = Mathf.Clamp(mp.x, rt.rect.min.x, rt.rect.max.x);
        result.y = Mathf.Clamp(mp.y, rt.rect.min.y, rt.rect.max.y);
        return rt.rect.Contains(mp);
    }

    private static Vector2 GetWidgetSize(GameObject go)
    {
        var rt = (RectTransform)go.transform;
        return rt.rect.size;
    }

    /* 색상을 적용하는 함수 */
    public void ApplyColor(Color32 color)
    {
        this.color = color;
        applyColor();
    }
}