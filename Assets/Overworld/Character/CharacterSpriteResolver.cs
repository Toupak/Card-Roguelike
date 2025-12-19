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
            if (RunLoop.instance != null)
                spriteLibrary.spriteLibraryAsset = RunLoop.instance.CharacterData.spriteLibrary;
        }
    }
}
