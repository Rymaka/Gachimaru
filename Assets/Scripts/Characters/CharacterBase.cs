using UnityEngine;

namespace Gachimaru.Gameplay
{
    public class CharacterBase : MonoBehaviour
    {
        public PlayerMovementController MovementController => _movementController;
        [SerializeField] private PlayerMovementController _movementController;
        [field: SerializeField] public Group CharacterGroup { get; private set; }

        private void Awake()
        {
            OnAwake();
        }
        
        protected virtual void OnAwake(){}
    }

    public enum Group
    {
        Player,
        Enemy,
        Friend
    }
}
