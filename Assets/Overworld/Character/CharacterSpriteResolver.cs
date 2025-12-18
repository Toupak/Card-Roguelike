using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Overworld.Character
{
    public class CharacterSpriteResolver : MonoBehaviour
    {
        [SerializeField] private SpriteLibrary spriteLibrary;
        
        public void SetSpriteLibrary(SpriteLibraryAsset spriteLibraryAsset)
        {
            spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
        }
    }
}
