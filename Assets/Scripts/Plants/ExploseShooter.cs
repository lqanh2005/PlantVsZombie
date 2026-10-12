using UnityEngine;

public class ExploseShooter : ShooterPlantBase
{
    [SerializeField] private Renderer plantRenderer;
    [SerializeField] private float cooldownDarken = 0.15f;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    private Color originalColor;
    private MaterialPropertyBlock propertyBlock;
    private bool hasVisualState;
    private bool isShowingCooldown;

    protected override void Awake()
    {
        base.Awake();
        propertyBlock = new MaterialPropertyBlock();

        if (plantRenderer != null)
            originalColor = plantRenderer.sharedMaterial.color;
    }

    public override void Init(PlantData data)
    {
        hasVisualState = false;
        base.Init(data);
    }

    protected override void OnCooldownChanged(bool isCoolingDown)
    {
        if (plantRenderer == null || propertyBlock == null)
            return;

        if (hasVisualState && isShowingCooldown == isCoolingDown)
            return;

        hasVisualState = true;
        isShowingCooldown = isCoolingDown;

        Color color = isCoolingDown
            ? new Color(
                originalColor.r * cooldownDarken,
                originalColor.g * cooldownDarken,
                originalColor.b * cooldownDarken,
                originalColor.a
            )
            : originalColor;
        plantRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColorId, color);
        plantRenderer.SetPropertyBlock(propertyBlock);
    }
}
