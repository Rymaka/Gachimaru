using System.Collections.Generic;
using UnityEngine;

namespace Gachimaru.Gameplay
{
    public class CharactersContainer : MonoBehaviour
    {
        public List<CharacterBase> Characters = new List<CharacterBase>();

        public void AddCharacter(CharacterBase character)
        {
            Characters.Add(character);
        }
        
        public void RemoveCharacter(CharacterBase character)
        {
            Characters.Remove(character);
        }
    }  
}

