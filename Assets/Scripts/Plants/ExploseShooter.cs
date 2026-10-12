using UnityEngine;

public class ExploseShooter : ShooterPlantBase
{
    [SerializeField] private Renderer plantRenderer;

    private Color originalColor;
    private MaterialPropertyBlock propertyBlock;

    protected override void Awake()
    {
        base.Awake();
        propertyBlock = new MaterialPropertyBlock();

        if (plantRenderer != null)
            originalColor = plantRenderer.sharedMaterial.color;
    }

    protected override void OnCooldownChanged(bool isCoolingDown)
    {
        if (plantRenderer == null || propertyBlock == null)
            return;

        Color color = isCoolingDown
            ? new Color(
                originalColor.r * 0.15f,
                originalColor.g * 0.15f,
                originalColor.b * 0.15f,
                originalColor.a
            )
            : originalColor;
        Debug.LogError($"ExploseShooter: OnCooldownChanged - isCoolingDown: {isCoolingDown}, color: {color}");
        plantRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", color);
        plantRenderer.SetPropertyBlock(propertyBlock);
    }
}
