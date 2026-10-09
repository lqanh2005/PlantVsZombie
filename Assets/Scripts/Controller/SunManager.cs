using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    [SerializeField] private int startingSun = 150;
    [SerializeField] private TMP_Text sunText;
    [SerializeField] private string sunFormat = "Sun: {0}";

    [Header("Hiệu ứng nhận Sun")]
    [SerializeField] private float punchScale = 0.15f;
    [SerializeField] private float punchDuration = 0.2f;

    private Tween punchTween;

    public int CurrentSun { get; private set; }
    public event Action<int> OnSunChanged;

    public void Init()
    {
        CurrentSun = startingSun;
        Refresh();
    }

    public bool CanAfford(int cost)
    {
        return CurrentSun >= cost;
    }

    public bool TrySpend(int cost)
    {
        if (!CanAfford(cost))
            return false;

        CurrentSun -= cost;
        Refresh();
        return true;
    }

    public void Add(int amount)
    {
        if (amount <= 0)
            return;

        CurrentSun += amount;
        Refresh();
        PunchText();
    }

    public bool TryGetSunTextScreenPoint(out Vector2 screenPoint)
    {
        screenPoint = Vector2.zero;
        if (sunText == null)
            return false;

        Canvas canvas = sunText.canvas;
        Camera uiCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;
        screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, sunText.rectTransform.position);
        return true;
    }

    private void PunchText()
    {
        if (sunText == null)
            return;

        punchTween?.Kill(true);
        sunText.rectTransform.localScale = Vector3.one;
        punchTween = sunText.rectTransform
            .DOPunchScale(Vector3.one * punchScale, punchDuration, 6, 0.5f)
            .SetLink(sunText.gameObject);
    }

    private void Refresh()
    {
        if (sunText != null)
            sunText.text = string.Format(sunFormat, CurrentSun);
        OnSunChanged?.Invoke(CurrentSun);
    }
}
