using UnityEngine;

public class WordBlock : MonoBehaviour
{
    public enum WordType
    {
        Noun,
        Verb,
        Property
    }

    public WordType wordType;
    public string wordValue;

    void Start()
    {
        // Инициализация, если необходимо
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        RuleManager.instance.ParseRules();
    }
}