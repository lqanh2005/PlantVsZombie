
using UnityEngine;
using UnityEngine.InputSystem;

public class SunClickHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask sunLayer = ~0;
    private void Update()
    {
        if (Mouse.current == null || Time.timeScale == 0f)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit,
                Mathf.Infinity, sunLayer))
            {
                Sun sun = hit.collider.GetComponent<Sun>();

                if (sun != null)
                
                    sun.Collect();
                
            }
        }
    }
}
