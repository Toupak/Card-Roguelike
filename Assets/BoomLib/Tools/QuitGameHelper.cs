namespace BoomLib.Tools
{
    public static class QuitGameHelper
    {
        public static void Quit()
        {
            /*
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER
                Application.OpenURL(webplayerQuitURL);
            #else
                Application.Quit();
            #endif
            */
        }
    }
}
