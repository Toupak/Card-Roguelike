using Run_Loop;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Overworld.Character
{
    public class CharacterSpriteResolver : MonoBehaviour
    {
        [SerializeField] private SpriteLibrary spriteLibrary;

        private void Start()
        {
            SetSpriteLibrary();
        }

        private void SetSpriteLibrary()
        {
            if (RunLoop.instance != null && RunLoop.instance.characterData != null)
                spriteLibrary.spriteLibraryAsset = RunLoop.instance.characterData.spriteLibrary;
        }
    }
}
