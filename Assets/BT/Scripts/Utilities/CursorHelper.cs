using UnityEngine;

namespace BombThrower.Utilities
{
    public static class CursorHelper
    {
        public static bool IsLocked => Cursor.lockState == CursorLockMode.Locked || Cursor.lockState == CursorLockMode.Confined;

        public static void LockCursor()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                return;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void ConfineCursor()
        {
            if (Cursor.lockState == CursorLockMode.Confined)
                return;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public static void UnlockCursor()
        {
            if (Cursor.lockState == CursorLockMode.None)
                return;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public static void SetCursorTexture(Texture2D texture2D, Vector2 hotspot = default, CursorMode cursorMode = CursorMode.Auto)
        {
            Cursor.SetCursor(texture2D, hotspot, cursorMode);
        }
    }
}
