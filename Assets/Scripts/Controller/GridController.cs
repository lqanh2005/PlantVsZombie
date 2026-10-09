using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour
{
    public GameObject gridParent;
    public GameObject tilePrefab;
    [SerializeField] int cols = 9;
    [SerializeField] int rows = 5;

    [Header("Board Size")]
    public float boardWidth = 18f;
    public float boardDepth = 10f;
    public float tileHeight = 0.1f;
    private Color colorA = new Color(0.35f, 0.65f, 0.25f);
    private Color colorB = new Color(0.45f, 0.75f, 0.30f);
    public void Init()
    {
        GenertateGrid();
    }
    private void GenertateGrid()
    {
        for (int i = this.transform.childCount - 1; i >= 0; i--) SimplePool.Despawn(this.transform.GetChild(i).gameObject);
        float tileWidth = boardWidth / cols;
        float tileHeight = boardDepth / rows;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new Vector3(-boardWidth / 2f + (col + 0.5f) * tileWidth, 0, -boardDepth / 2f + (row + 0.5f) * tileHeight);
                GameObject tile = SimplePool.Spawn(tilePrefab, pos, Quaternion.identity);
                tile.transform.SetParent(gridParent.transform);
                tile.transform.localScale = new Vector3(tileWidth / 10f, 1f, tileHeight / 10f);
                Renderer rederer = tile.GetComponent<Renderer>();
                if (rederer != null)
                {
                    rederer.material.color = (row + col) % 2 == 0 ? colorA : colorB;
                }
            }
        }
    }
    public bool TryGetTile(Vector3 worldPosition, out int row, out int col)
    {
        float tileWidth = boardWidth / cols;
        float tileHeight = boardDepth / rows;
        Vector3 local = this.transform.InverseTransformPoint(worldPosition);
        col = Mathf.FloorToInt((local.x + boardWidth / 2f) / tileWidth);
        int gridRowBottom = Mathf.FloorToInt((local.z + boardDepth / 2f) / tileHeight);
        row = rows -1 - gridRowBottom;
        return row >= 0 && row < rows &&
           col >= 0 && col < cols;
    }
    public Vector3 GetCellCenterWorld(int row, int col)
    {
        float tileWidth = boardWidth / cols;
        float tileDepth = boardDepth / rows;

        Vector3 localPosition = new Vector3(
            -boardWidth / 2f + (col + 0.5f) * tileWidth,
            0f,
            -boardDepth / 2f + (rows - row - 0.5f) * tileDepth
        );

        return transform.TransformPoint(localPosition);
    }
}