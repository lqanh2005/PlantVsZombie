using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class EditorUIBuilder
{
    public static readonly Color ButtonColor = new Color(0.7254902f, 0.42745098f, 0.039215688f, 1f);
    public static readonly Color PanelColor = new Color(0.36f, 0.22f, 0.1f, 1f);

    public static Sprite DefaultSprite => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

    public static Button CreateButton(string name, Transform parent, string label, float fontSize)
    {
        GameObject go = CreateUIObject(name, parent, typeof(Image), typeof(Button));
        Image image = go.GetComponent<Image>();
        image.color = ButtonColor;
        image.sprite = DefaultSprite;
        image.type = Image.Type.Sliced;

        Button button = go.GetComponent<Button>();
        button.targetGraphic = image;

        TextMeshProUGUI text = CreateText("Text", go.transform, label, fontSize);
        Stretch(text.rectTransform);
        return button;
    }

    public static TextMeshProUGUI CreateText(string name, Transform parent, string value, float fontSize)
    {
        GameObject go = CreateUIObject(name, parent, typeof(TextMeshProUGUI));
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = FontStyles.Bold;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;
        text.outlineWidth = 0.2f;
        text.outlineColor = Color.black;
        text.raycastTarget = false;
        return text;
    }

    public static GameObject CreateUIObject(string name, Transform parent, params System.Type[] components)
    {
        List<System.Type> types = new List<System.Type> { typeof(RectTransform) };
        types.AddRange(components);
        GameObject go = new GameObject(name, types.ToArray());
        go.layer = LayerMask.NameToLayer("UI");
        go.transform.SetParent(parent, false);
        return go;
    }

    public static GameObject CreatePanel(string name, Transform parent, Vector2 size)
    {
        GameObject panel = CreateUIObject(name, parent, typeof(Image));
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        Image image = panel.GetComponent<Image>();
        image.color = PanelColor;
        image.sprite = DefaultSprite;
        image.type = Image.Type.Sliced;
        return panel;
    }

    public static GameObject CreateDimBackground(string name, Transform parent)
    {
        GameObject go = CreateUIObject(name, parent, typeof(Image));
        Stretch(go.GetComponent<RectTransform>());
        go.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.7f);
        return go;
    }

    public static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
    }

    public static void Place(RectTransform rect, Vector2 anchor, Vector2 anchoredPosition, Vector2 size)
    {
        rect.anchorMin = rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;
    }

    public static T FindInScene<T>(Scene scene) where T : Component
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            T found = root.GetComponentInChildren<T>(true);
            if (found != null)
                return found;
        }
        return null;
    }
}
