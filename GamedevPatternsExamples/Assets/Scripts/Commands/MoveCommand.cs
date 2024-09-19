namespace DefaultNamespace
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MoveCommand : ICommand
    {
        private struct Movement
        {
            public Transform objTransform;
            public Vector3 initialPosition;
            public Vector3 finalPosition;

            public Movement(Transform transform, Vector3 initialPos, Vector3 finalPos)
            {
                objTransform = transform;
                initialPosition = initialPos;
                finalPosition = finalPos;
            }
        }

        private List<Movement> movements = new List<Movement>();

        public void AddMovement(Transform objTransform, Vector3 initialPosition, Vector3 finalPosition)
        {
            movements.Add(new Movement(objTransform, initialPosition, finalPosition));
        }

        public void Execute()
        {
            // Команда уже выполнена во время движения
        }

        public void Undo()
        {
            foreach (Movement movement in movements)
            {
                movement.objTransform.position = movement.initialPosition;
            }
        }
    }
}