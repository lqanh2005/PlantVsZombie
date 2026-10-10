using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantCard : MonoBehaviour
{
    public int plantId;
    public Button btn;
    public Image plantImage;
    public TMP_Text sunTxt;
    public PlantType plantType;
    public GameObject plantPrefab;
    public RectTransform cooldownOverlay;
    [SerializeField] private Color cooldownColor = new Color(0f, 0f, 0f, 0.6f);
    [SerializeField] private Color notEnoughSunColor = new Color(0.4f, 0.4f, 0.4f, 1f);
    [SerializeField] private Color selectedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    private int sunCost;
    private float rechargeTime;
    private float cooldownTimer;
    private SunManager sunManager;
    private readonly List<Graphic> dimGraphics = new List<Graphic>();
    private readonly List<Color> originalColors = new List<Color>();
    private Color currentTint = Color.white;
    private bool isSelected;

    public int SunCost => sunCost;
    public bool IsCoolingDown => cooldownTimer > 0f;
    public bool CanAfford => sunManager == null || sunManager.CanAfford(sunCost);
    public bool IsSelectable => !IsCoolingDown && CanAfford;

    public void Init(int id, Sprite sprite, int sunCost)
    {
        plantId = id;
        plantImage.sprite = sprite;
        this.sunCost = sunCost;
        sunTxt.text = sunCost.ToString();
        rechargeTime = 0f;
        Setup();
    }

    public void Init()
    {
        Setup(PlantDatabase.Instance.GetPlantDataByType(plantType, plantId));
    }

    public void Init(PlantType type, int id)
    {
        plantType = type;
        plantId = id;
        Setup(PlantDatabase.Instance.GetPlantData(type, id));
    }

    private void Setup(PlantData data)
    {
        if (data == null)
        {
            btn.interactable = false;
            return;
        }

        plantImage.sprite = data.icon;
        sunCost = data.sunCost;
        sunTxt.text = sunCost.ToString();
        plantPrefab = data.plantPrefab;
        rechargeTime = data.rechargeTime;
        Setup();
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        RefreshState();
    }

    public void StartCooldown()
    {
        if (rechargeTime <= 0f)
            return;

        cooldownTimer = rechargeTime;
        RefreshState();
    }

    private void Update()
    {
        if (!IsCoolingDown)
            return;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0f)
            cooldownTimer = 0f;
        RefreshState();
    }

    private void OnDestroy()
    {
        if (sunManager != null)
            sunManager.OnSunChanged -= HandleSunChanged;
    }

    private void Setup()
    {
        btn.interactable = true;
        cooldownTimer = 0f;
        isSelected = false;
        EnsureCooldownOverlay();
        CacheDimGraphics();
        BindSunManager();
        RefreshState();

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            if (!IsSelectable)
                return;
            GamePlayController.Instance.playerContain.plantSpawner.ToggleSelect(this);
        });
    }

    private void BindSunManager()
    {
        if (sunManager != null)
            sunManager.OnSunChanged -= HandleSunChanged;

        sunManager = GamePlayController.Instance.playerContain.sunManager;
        if (sunManager != null)
            sunManager.OnSunChanged += HandleSunChanged;
    }

    private void HandleSunChanged(int currentSun)
    {
        RefreshState();
    }

    private void RefreshState()
    {
        btn.interactable = IsSelectable;

        if (!CanAfford)
            ApplyTint(notEnoughSunColor);
        else if (isSelected)
            ApplyTint(selectedColor);
        else
            ApplyTint(Color.white);

        if (cooldownOverlay == null)
            return;

        float ratio = rechargeTime > 0f ? cooldownTimer / rechargeTime : 0f;
        cooldownOverlay.gameObject.SetActive(ratio > 0f);
        cooldownOverlay.anchorMax = new Vector2(1f, ratio);
    }

    private void ApplyTint(Color tint)
    {
        if (currentTint == tint)
            return;

        currentTint = tint;
        for (int i = 0; i < dimGraphics.Count; i++)
        {
            if (dimGraphics[i] == null)
                continue;
            dimGraphics[i].color = originalColors[i] * tint;
        }
    }

    private void CacheDimGraphics()
    {
        ApplyTint(Color.white);
        dimGraphics.Clear();
        originalColors.Clear();

        foreach (Graphic graphic in GetComponentsInChildren<Graphic>(true))
        {
            if (cooldownOverlay != null && graphic.transform == cooldownOverlay)
                continue;

            dimGraphics.Add(graphic);
            originalColors.Add(graphic.color);
        }
    }

    private void EnsureCooldownOverlay()
    {
        if (cooldownOverlay != null)
            return;

        GameObject go = new GameObject("CooldownOverlay", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(transform, false);
        go.transform.SetAsLastSibling();

        cooldownOverlay = go.GetComponent<RectTransform>();
        cooldownOverlay.anchorMin = Vector2.zero;
        cooldownOverlay.anchorMax = Vector2.one;
        cooldownOverlay.pivot = new Vector2(0.5f, 0f);
        cooldownOverlay.offsetMin = Vector2.zero;
        cooldownOverlay.offsetMax = Vector2.zero;

        Image image = go.GetComponent<Image>();
        image.color = cooldownColor;
        image.raycastTarget = false;
    }
}
