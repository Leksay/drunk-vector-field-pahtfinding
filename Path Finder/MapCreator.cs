using System;
using UnityEngine;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour
{

    [Header("Map")]
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private float _height;
    [SerializeField] private float _margin;

    private Map _map;

    [Header("Cell")] 
    [SerializeField] private Color _onCreateColor;

    private Cell[,] _cells;

    private void Awake()
    {
        _map = GetComponent<Map>();
    }

    public void CreateMap()
    {
        int size = _map.MapSettings.Size;
        _cells = new Cell[size,size];
        int startPosition = -size / 2;

        Transform map = new GameObject("Map").transform;
        
        for (var i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                _cells[i, j] = CreateCell(new Vector2Int(i, j), startPosition + i, startPosition + j, map, $"Cell [{i} {j}]");
            }
        }
    }

    private Cell CreateCell(Vector2Int arrayIndexes, int x, int z, Transform parent, string cellName)
    {
        float cellScale = _map.MapSettings.CellScale;
        Vector3 position = new Vector3(x, _height, z) * cellScale;
        
        GameObject cell = Instantiate(_cellPrefab, position, Quaternion.identity, parent);
        
        cell.transform.localScale *= cellScale;
        cell.name = cellName;

        var cellComponent = cell.AddComponent<PathCellComponent>();
        var directionT = cell.transform.GetChild(0);
        var text = cell.GetComponentInChildren<Text>();
        cellComponent.Cell = new Cell(arrayIndexes, cell.transform.position, cell.GetComponent<MeshRenderer>().material, _onCreateColor, directionT, text);

        return cellComponent.Cell;
    }

    public Cell[,] GetCells() => _cells;
}

public class PathCellComponent : MonoBehaviour
{
    public Cell Cell;
}
