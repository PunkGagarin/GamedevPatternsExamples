using UnityEngine;
using System.Collections.Generic;

public class RuleManager : MonoBehaviour
{
    public static RuleManager instance;

    public Dictionary<string, List<string>> objectProperties = new();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ParseRules();
    }

    public void ParseRules()
    {
        objectProperties.Clear();

        WordBlock[] wordBlocks = FindObjectsOfType<WordBlock>();

        foreach (WordBlock word in wordBlocks)
        {
            Vector2 wordPosition = word.transform.position;

            CheckForRule(wordPosition, Vector2.right);
            CheckForRule(wordPosition, Vector2.up);
        }

        ApplyProperties();
    }

    void CheckForRule(Vector2 startPos, Vector2 direction)
    {
        List<WordBlock> wordsInLine = new List<WordBlock>();

        LayerMask wordBlockLayer = LayerMask.GetMask("WordBlock");

        for (int i = 0; i < 3; i++)
        {
            Vector2 checkPos = startPos + direction * i;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPos, .4f, wordBlockLayer);

            WordBlock wordBlock = null;

            foreach (Collider2D collider in colliders)
            {
                wordBlock = collider.GetComponent<WordBlock>();
                if (wordBlock != null)
                {
                    wordsInLine.Add(wordBlock);
                    break;
                }
            }

            if (wordBlock == null)
            {
                return;
            }
        }

        if (wordsInLine.Count == 3)
        {
            if (wordsInLine[0].wordType == WordBlock.WordType.Noun &&
                wordsInLine[1].wordType == WordBlock.WordType.Verb &&
                (wordsInLine[2].wordType == WordBlock.WordType.Property || wordsInLine[2].wordType == WordBlock.WordType.Noun))
            {
                string subject = wordsInLine[0].wordValue.ToUpper();
                string verb = wordsInLine[1].wordValue.ToUpper();
                string predicate = wordsInLine[2].wordValue.ToUpper();

                if (verb == "IS")
                {
                    if (!objectProperties.ContainsKey(subject))
                    {
                        objectProperties[subject] = new List<string>();
                    }
                    objectProperties[subject].Add(predicate);
                    Debug.Log($"Найдено правило: {subject} {verb} {predicate}");
                }
            }
        }
    }

    void ApplyProperties()
    {
        BaseObject[] allObjects = FindObjectsOfType<BaseObject>();

        foreach (BaseObject obj in allObjects)
        {
            obj.ClearProperties();

            string objName = obj.objectName.ToUpper();

            if (objectProperties.ContainsKey(objName))
            {
                foreach (string property in objectProperties[objName])
                {
                    obj.AddProperty(property);
                    Debug.Log($"Объект {objName} получает свойство {property}");
                }
            }

            foreach (KeyValuePair<string, List<string>> entry in objectProperties)
            {
                if (entry.Value.Contains(objName))
                {
                    obj.AddProperty(entry.Key);
                    Debug.Log($"Объект {objName} становится {entry.Key}");
                }
            }
        }
    }
}