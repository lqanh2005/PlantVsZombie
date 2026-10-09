using DG.Tweening;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private int sunValue = 25;

    [Header("Hiệu ứng thu thập")]
    [SerializeField] private Vector3 squashScale = new Vector3(1.15f, 0.85f, 1.15f);
    [SerializeField] private float squashDuration = 0.15f;
    [SerializeField] private float flyDuration = 0.6f;
    [SerializeField] private float flyEndScale = 0.5f;

    private bool isCollected;
    private Vector3 originalScale;
    private Rigidbody body;
    private bool originalKinematic;
    private Collider sunCollider;
    private Sequence collectSequence;

    private void Awake()
    {
        originalScale = transform.localScale;
        body = GetComponent<Rigidbody>();
        if (body != null)
            originalKinematic = body.isKinematic;
        sunCollider = GetComponent<Collider>();
    }

    public void Init(int value)
    {
        if (value > 0)
            sunValue = value;
        isCollected = false;
    }

    private void OnEnable()
    {
        isCollected = false;
        transform.localScale = originalScale;
        if (sunCollider != null)
            sunCollider.enabled = true;
        if (body != null)
            body.isKinematic = originalKinematic;
    }

    private void OnDisable()
    {
        collectSequence?.Kill();
        collectSequence = null;
    }

    public void Collect()
    {
        if (isCollected)
            return;

        isCollected = true;

        if (sunCollider != null)
            sunCollider.enabled = false;
        if (body != null)
            body.isKinematic = true;

        SunManager sunManager = GamePlayController.Instance.playerContain.sunManager;
        Vector3 target = GetFlyTarget(sunManager);
        float halfSquash = squashDuration * 0.5f;

        collectSequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.Scale(originalScale, squashScale), halfSquash).SetEase(Ease.OutQuad))
            .Append(transform.DOScale(originalScale, halfSquash).SetEase(Ease.OutBack))
            .Append(transform.DOMove(target, flyDuration).SetEase(Ease.InCubic))
            .Join(transform.DOScale(originalScale * flyEndScale, flyDuration).SetEase(Ease.InQuad))
            .OnComplete(() =>
            {
                if (sunManager != null)
                    sunManager.Add(sunValue);
                SimplePool.Despawn(gameObject);
            })
            .SetLink(gameObject);
    }

    private Vector3 GetFlyTarget(SunManager sunManager)
    {
        Camera cam = Camera.main;
        if (cam == null || sunManager == null || !sunManager.TryGetSunTextScreenPoint(out Vector2 screenPoint))
            return transform.position + Vector3.up * 3f;

        float depth = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, depth));
    }
}
