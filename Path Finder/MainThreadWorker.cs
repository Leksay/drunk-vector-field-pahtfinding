using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadWorker : MonoBehaviour
{
    public static Queue<Action> MainThreadActions = new Queue<Action>();

    private const int _maxOperationCount = 13;

    private void Update()
    {
        if (MainThreadActions.Count == 0)
        {
            return;
        }

        int operationCount = MainThreadActions.Count > _maxOperationCount ? _maxOperationCount : MainThreadActions.Count;
        for (var i = 0; i < operationCount; i++)
        {
            var action = MainThreadActions.Dequeue();

            if (action != null)
            {
                action();
            }
            else
            {
                Debug.LogError("Action is null");
            }
        }
    }
}
