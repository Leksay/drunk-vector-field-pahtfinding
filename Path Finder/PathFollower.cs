using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathFollower : MonoBehaviour
{
    private Vector2Int _currentCell;
    private Transform _myT;

    private void OnEnable()
    {
        int startX = Map.Instance.Cells.GetLength(0);
        int startY = Map.Instance.Cells.GetLength(1);

        startX = Random.Range(0, startX);
        startY = Random.Range(0, startY);

        _myT = transform;
        _currentCell = new Vector2Int(startX, startY);

        _myT.position = Map.Instance.Cells[startX, startY].Position;
    }
}
