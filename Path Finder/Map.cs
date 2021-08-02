using UnityEngine;

[RequireComponent(typeof(MapCreator))]
public class Map : MonoBehaviour
{
    public static Map Instance;

    public Cell[,] Cells;
    public Vector3[,] VectorField;
    public MapSettings MapSettings;
    public Cell EndPoint;

    private MapCreator _mapCreator;
    private MapEntitiesSetuper _entitiesSetuper;

    private void OnEnable()
    {
        _entitiesSetuper = GetComponent<MapEntitiesSetuper>();
        _entitiesSetuper.OnNewTarget += SetNewTarget;
        _entitiesSetuper.OnWallCreate += SetCellToWall;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void SetNewTarget(Cell newTarget)
    {
        EndPoint = newTarget;
        EndPoint.SetText(string.Empty);
        EndPoint.DistableVector();
    }

    private void OnDisable()
    {
        _entitiesSetuper.OnWallCreate -= SetNewTarget;
        _entitiesSetuper.OnWallCreate -= SetCellToWall;
    }

    private void Start()
    {
        _mapCreator = GetComponent<MapCreator>();
        _mapCreator.CreateMap();
        Cells = _mapCreator.GetCells();
    }

    public float DistanceToTarget(Cell cell) => (EndPoint.Position - cell.Position).magnitude;
    
    public void ClearCells()
    {
        foreach (var cell in Cells)
        {
            if (!cell.isWall)
            {
                cell.ClearCell();
            }
        }
    }

    public Color CalculateDistanceColor(float distance)
    {
        // distance / size set distance in 0..1 range

        float normalizedDistance = distance / (MapSettings.Size * MapSettings.CellScale);
        float r = Mathf.Lerp(MapSettings.NearColor.r, MapSettings.FarColor.r, normalizedDistance);
        float g = Mathf.Lerp(MapSettings.NearColor.g, MapSettings.FarColor.g, normalizedDistance);
        float b = Mathf.Lerp(MapSettings.NearColor.b, MapSettings.FarColor.b, normalizedDistance);

        return new Color(r, g, b, 1);
    }

    public void SetCellToWall(Cell cell)
    {
        if (cell.isWall)
        {
            return;
        }

        cell.isWall = true;
        if (cell.isWall)
        {
            cell.SetText("W");
            cell.GoalDistance = float.MaxValue;
            cell.SetColor(MapSettings.WallColor);
            cell.ApplyMaterialColor();
        }
    }

    public void DrawVectorField()
    {
        if (VectorField.GetLength(0) != Cells.GetLength(0) || VectorField.GetLength(1) != Cells.GetLength(1))
        {
            Debug.LogError($"Vector3 field length not Equal to Cells length cells: {Cells.Length} <-> vectorField {VectorField.Length}");
            return;
        }

        for (int x = 0; x < VectorField.GetLength(0); x++)
        {
            for (int y = 0; y < VectorField.GetLength(1); y++)
            {
                Cells[x, y].SetVectorDirection(VectorField[x, y]);
            }
        }
    }
}
