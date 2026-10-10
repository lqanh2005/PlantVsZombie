
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlantCardUI : MonoBehaviour
{
    public int plantId;
    public PlantType plantType;

    [SerializeField] private Button btn;
    [SerializeField] private Image plantImage;
    [SerializeField] private TMP_Text sunTxt;

    [Header("Selection")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField]
    private Color selectedColor =
        new Color(0.35f, 0.35f, 0.35f, 1f);

    [Header("Animation")]
    [SerializeField] private float moveDuration = 0.3f;

    [SerializeField] private RectTransform rectTransform;

    private Canvas rootCanvas;
    private bool isSelected;
    private bool isMoving;

    public bool IsSelected => isSelected;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;

        if (btn != null)
        {
            btn.onClick.AddListener(OnClick);
        }
    }

    public void SetData(PlantData data)
    {
        plantId = data.plantId;
        plantType = data.plantType;

        plantImage.sprite = data.icon;
        sunTxt.text = data.sunCost.ToString();
    }

    private void OnClick()
    {
        if (isSelected || isMoving)
            return;

        HomeController.Instance.selectedSeedGrid
            .TrySelectPlant(this);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (plantImage != null)
            plantImage.color = selected ? selectedColor : normalColor;

        if (sunTxt != null)
            sunTxt.color = selected ? selectedColor : normalColor;
    }

    public void MoveToSlot(RectTransform targetSlot)
    {
        if (targetSlot == null || isMoving)
            return;

        if (rootCanvas == null)
            rootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;

        if (rootCanvas == null)
        {
            Debug.LogError("PlantCardUI: Không tìm thấy Canvas!");
            return;
        }

        isMoving = true;

        rectTransform.DOKill();

        // Lưu vị trí hiện tại trước khi đổi parent.
        Vector3 startPosition = rectTransform.position;

        rectTransform.SetParent(rootCanvas.transform, true);
        rectTransform.position = startPosition;

        rectTransform.DOMove(targetSlot.position, moveDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                rectTransform.SetParent(targetSlot, false);

                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = Quaternion.identity;

                isMoving = false;
            });
    }
}
