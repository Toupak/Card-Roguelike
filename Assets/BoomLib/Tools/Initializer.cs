using UnityEngine;
using UnityEngine.Assertions;

namespace BoomLib.Tools
{
    public static class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

        public static void Execute()
        {
            Object persistObject = Resources.Load("PERSISTOBJECTS");
            Assert.IsNotNull(persistObject, $"[{nameof(Initializer)}] : error : could not find PERSISTOBJECTS in Resource folder.");

            if (persistObject != null)
                Object.DontDestroyOnLoad(Object.Instantiate(persistObject));
        }
    }
}
