using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 5f);
    }
}
