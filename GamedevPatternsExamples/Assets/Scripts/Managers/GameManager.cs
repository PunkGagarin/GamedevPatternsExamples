using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private Stack<ICommand> commandStack = new();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UndoCommand();
        }
    }

    public void PushCommand(ICommand command)
    {
        commandStack.Push(command);
    }

    public void UndoCommand()
    {
        if (commandStack.Count > 0)
        {
            ICommand command = commandStack.Pop();
            command.Undo();
        }
        else
        {
            Debug.Log("Нет действий для отмены!");
        }
    }
}