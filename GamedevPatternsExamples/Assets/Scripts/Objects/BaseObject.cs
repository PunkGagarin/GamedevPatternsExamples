using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseObject : MonoBehaviour
{
    public string objectName;
    private List<string> properties = new List<string>();

    private Rigidbody2D rb2D;
    private float moveTime = 0.1f;
    private float inverseMoveTime;
    private BoxCollider2D boxCollider;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        inverseMoveTime = 1f / moveTime;
    }

    public void AddProperty(string property)
    {
        if (!properties.Contains(property))
        {
            properties.Add(property);
        }
    }

    public void RemoveProperty(string property)
    {
        if (properties.Contains(property))
        {
            properties.Remove(property);
        }
    }

    public void ClearProperties()
    {
        properties.Clear();
    }

    public bool HasProperty(string property)
    {
        return properties.Contains(property);
    }

    public bool AttemptMove(int xDir, int yDir)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end);
        boxCollider.enabled = true;
        // Проверяем, есть ли препятствие на пути

        if (hit.transform == null)
        {
            // Нет препятствий, перемещаемся
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        else
        {
            BaseObject hitObject = hit.transform.GetComponent<BaseObject>();
            if (hitObject != null)
            {
                if (hitObject.HasProperty("PUSH"))
                {
                    // Пытаемся сдвинуть объект
                    if (hitObject.AttemptMove(xDir, yDir))
                    {
                        StartCoroutine(SmoothMovement(end));
                        return true;
                    }
                }
                if (hitObject.HasProperty("STOP"))
                {
                    // Нельзя пройти
                    return false;
                }
                if (hitObject.HasProperty("WIN") && this.HasProperty("YOU"))
                {
                    // Игрок победил
                    Debug.Log("Победа!");
                    // Реализуйте победу уровня
                    return true;
                }
                if (hitObject.HasProperty("DEFEAT") && this.HasProperty("YOU"))
                {
                    // Игрок проиграл
                    Destroy(gameObject);
                    return false;
                }
            }
            else
            {
                // Если объект не имеет BaseObject, перемещаемся
                StartCoroutine(SmoothMovement(end));
                return true;
            }
        }

        return false;
    }

    IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
}