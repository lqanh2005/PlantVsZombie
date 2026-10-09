using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = new Color(1f, 0.25f, 0.25f);
    [SerializeField] private float flashDuration = 0.08f;
    [SerializeField] private int flashCount = 2;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    private struct FlashTarget
    {
        public Renderer renderer;
        public int materialIndex;
        public int propertyId;
        public Color originalColor;
    }

    private readonly List<FlashTarget> targets = new List<FlashTarget>();
    private MaterialPropertyBlock block;
    private Tween flashTween;
    private float progress;

    private void Awake()
    {
        block = new MaterialPropertyBlock();
        CacheTargets();
    }

    private void OnDisable()
    {
        flashTween?.Kill();
        flashTween = null;
        Apply(0f);
    }

    public void Play()
    {
        if (targets.Count == 0)
            return;

        flashTween?.Kill();
        progress = 0f;
        flashTween = DOTween.To(() => progress, value => { progress = value; Apply(value); }, 1f, flashDuration)
            .SetLoops(flashCount * 2, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .OnKill(() => Apply(0f))
            .SetLink(gameObject);
    }

    private void CacheTargets()
    {
        targets.Clear();
        foreach (Renderer rend in GetComponentsInChildren<Renderer>(true))
        {
            if (rend is ParticleSystemRenderer)
                continue;

            Material[] materials = rend.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                Material material = materials[i];
                if (material == null)
                    continue;

                int propertyId;
                if (material.HasProperty(BaseColorId))
                    propertyId = BaseColorId;
                else if (material.HasProperty(ColorId))
                    propertyId = ColorId;
                else
                    continue;

                targets.Add(new FlashTarget
                {
                    renderer = rend,
                    materialIndex = i,
                    propertyId = propertyId,
                    originalColor = material.GetColor(propertyId)
                });
            }
        }
    }

    private void Apply(float t)
    {
        if (block == null)
            return;

        foreach (FlashTarget target in targets)
        {
            if (target.renderer == null)
                continue;

            target.renderer.GetPropertyBlock(block, target.materialIndex);
            block.SetColor(target.propertyId, Color.Lerp(target.originalColor, flashColor, t));
            target.renderer.SetPropertyBlock(block, target.materialIndex);
        }
    }
}
