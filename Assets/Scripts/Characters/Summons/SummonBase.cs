using UnityEngine;
using Zenject;

namespace Gachimaru.Gameplay
{
    public class SummonBase : CharacterBase
    {
        [Inject] private CharactersContainer _charactersContainer;
        
        [SerializeField] protected float _lifetime;
        [SerializeField] protected CharacterAI CharacterAI;

        private CharacterBase _summoner;

        protected override void OnAwake()
        {
            _charactersContainer.AddCharacter(this);
        }

        public void Init(Group friendGroup, Group aggressiveGroup)
        {
            CharacterAI.Init(friendGroup, aggressiveGroup);
        }
    }
}

