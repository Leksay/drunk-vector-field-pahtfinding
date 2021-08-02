using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private float _lineDistance;
    private float _diagonalDistance;

    #region PathCalculation

    public void CalculatePath(Map map, Cell endPoint)
    {
        if (map.Cells.Length == 0 || endPoint.isWall)
        {
            return;
        }

        map.ClearCells();

        _lineDistance = (map.Cells[0, 0].Position - map.Cells[0, 1].Position).magnitude;
        _diagonalDistance = (map.Cells[0, 0].Position - map.Cells[1, 1].Position).magnitude;

        Queue<Cell> openList = new Queue<Cell>();
        openList.Enqueue(endPoint);
        
        while (openList.Count > 0)
        {
            var selected = openList.Dequeue();

            if (selected.isWall)
            {
                continue;
            }

            if (selected == endPoint)
            {
                selected.GoalDistance = 0;
            }
            
            var neighboursIndexes = GetNeighboursIndexes(map, selected);

            for (int i = 0; i < neighboursIndexes.Length; i++)
            {
                var neighbour = map.Cells[neighboursIndexes[i].x, neighboursIndexes[i].y];

                if (neighbour.Visited)
                {
                    CompareVisited(selected, neighbour);
                }
                else
                {
                    CompareNotVisited(selected, neighbour);
                    openList.Enqueue(neighbour);
                    neighbour.Visited = true;
                }
            }
        }

        MainThreadWorker.MainThreadActions.Enqueue(() =>
        {
            foreach (var mapCell in map.Cells)
            {
                mapCell.SetText(mapCell.GoalDistance);
            }
        });
    }

    private void CompareVisited(Cell selected, Cell neighbour)
    {
       // float addDistance = (selected.Position - neighbour.Position).magnitude;
        if (neighbour.GoalDistance > selected.GoalDistance)
        {
            return;
        }
        //neighbour.GoalDistance = selected.GoalDistance + addDistance;
    }
    
    private void CompareNotVisited(Cell selected, Cell neighbour)
    {
        float addDistance = (selected.Position - neighbour.Position).magnitude;
        
        neighbour.GoalDistance = selected.GoalDistance + addDistance;
    }
    
    #endregion

    #region VectorField

    public Vector3[,] GenerateVectorField(Map map)
    {
        int xSize = map.Cells.GetLength(0);
        int ySize = map.Cells.GetLength(1);

        Vector3[,] vectorField = new Vector3[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                vectorField[x, y] = ToClosesNeighbourVector(map, map.Cells[x, y]);
            }
        }
        
        return vectorField;
    }

    private Vector3 ToClosesNeighbourVector(Map map, Cell origin)
    {
        Vector3 toClosestNeighbourVector = Vector3.zero;

        var neighbours = GetNeighbours(map, origin);

        Cell closestCell = neighbours[0];
        foreach (var neighbour in neighbours)
        {
            if (neighbour.isWall)
            {
                continue;
            }

            var sameDistanceCells = new List<Cell>();
            if (neighbour.GoalDistance < closestCell.GoalDistance)
            {
                closestCell = neighbour;
            }
        }

        toClosestNeighbourVector = closestCell.Position - origin.Position;

        return toClosestNeighbourVector;
    }

    #endregion

    public static Cell[] GetNeighbours(Map map, Cell cell)
    {
        int cellX = cell.ArrayIndexes.x;
        int cellY = cell.ArrayIndexes.y;
        
        // set startX and startY value greater than 0
        int startX = cellX - 1 >= 0 ? cellX - 1 : cellX;
        int startY = cellY - 1 >= 0 ? cellY - 1 : cellY;

        // set endX and endY value less then array length
        int endX = cellX + 1 < map.Cells.GetLength(0) ? cellX + 1 : cellX;
        int endY = cellY + 1 < map.Cells.GetLength(1) ? cellY + 1 : cellY;

        HashSet<Cell> neighbours = new HashSet<Cell>();
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (x == cellX && y == cellY)
                {
                    continue;
                }
                
                neighbours.Add(map.Cells[x,y]);   
            }
        }

        return neighbours.ToArray();
    }

    public static Vector2Int[] GetNeighboursIndexes(Map map, Cell cell)
    {
        int cellX = cell.ArrayIndexes.x;
        int cellY = cell.ArrayIndexes.y;
        
        // set startX and startY value greater than 0
        int startX = cellX - 1 >= 0 ? cellX - 1 : cellX;
        int startY = cellY - 1 >= 0 ? cellY - 1 : cellY;

        // set endX and endY value less then array length
        int endX = cellX + 1 < map.Cells.GetLength(0) ? cellX + 1 : cellX;
        int endY = cellY + 1 < map.Cells.GetLength(1) ? cellY + 1 : cellY;

        List<Vector2Int> indexes = new List<Vector2Int>();
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (x == cellX && y == cellY)
                {
                    continue;
                }
                
                indexes.Add(new Vector2Int(x,y));   
            }
        }

        return indexes.ToArray();
    }
}
