using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Cell
{
    public Vector2Int ArrayIndexes;
    public Vector3 Position;
    public Vector3 DirectionToTarget;
    public float GoalDistance;
    public Color CellColor;
    public bool Visited;
    public Text _text;

    private  Material _cellMaterial;
    private Transform _directionT;

    public Cell(Vector2Int arrayIndexes, Vector3 position, Material cellMaterial, Color startColor, Transform directionT, Text text)
    {
        ArrayIndexes = arrayIndexes;
        Position = position;
        _cellMaterial = cellMaterial;
        cellMaterial.color = startColor;
        _directionT = directionT;
        _text = text;
    }

    public bool isWall { get; set; }

    public void ClearCell()
    {
        DirectionToTarget = Vector3.zero;
        GoalDistance = 0;
        Visited = false;
    }

    public void DistableVector() => _directionT.gameObject.SetActive(false);
    
    public void SetText(float distance)
    {
        _text.text = (distance).ToString();
    }
    
    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetVectorDirection(Vector3 direction)
    {
        if (isWall)
        {
            return;
        }
        
        _directionT.gameObject.SetActive(true);
        _directionT.right = -direction;
    }
    
    public void SetColor(Color color) => CellColor = color;
    public void ApplyMaterialColor() => _cellMaterial.color = CellColor;

    public static implicit operator bool(Cell cell) => cell != null;
}