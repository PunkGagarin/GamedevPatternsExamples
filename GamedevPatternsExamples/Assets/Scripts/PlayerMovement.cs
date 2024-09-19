using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private List<BaseObject> controlledObjects = new List<BaseObject>();

    void Update()
    {
        UpdateControlledObjects();

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            foreach (BaseObject obj in controlledObjects)
            {
                obj.AttemptMove(horizontal, vertical);
            }
        }
    }

    void UpdateControlledObjects()
    {
        controlledObjects.Clear();
        BaseObject[] allObjects = FindObjectsOfType<BaseObject>();
        foreach (BaseObject obj in allObjects)
        {
            if (obj.HasProperty("YOU"))
            {
                if (!controlledObjects.Contains(obj))
                    controlledObjects.Add(obj);
            }
        }
    }
}