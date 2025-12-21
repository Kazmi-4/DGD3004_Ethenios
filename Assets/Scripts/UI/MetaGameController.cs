using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.UI
{
    /// <summary>
    /// Controls switching between gameplay and main menu UI.
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        [Header("UI References")]
        public MainUIController mainMenu;
        public Canvas[] gamePlayCanvasii;

        [Header("Game")]
        public GameController gameController;

        private bool showMainCanvas = false;
        private InputAction menuAction;

        void OnEnable()
        {
            // Safely fetch menu action
            if (InputSystem.actions != null)
            {
                menuAction = InputSystem.actions.FindAction("Player/Menu");
                if (menuAction != null)
                    menuAction.Enable();
            }

            ApplyMenuState(showMainCanvas);
        }

        void OnDisable()
        {
            if (menuAction != null)
                menuAction.Disable();
        }

        void Update()
        {
            if (menuAction != null && menuAction.WasPressedThisFrame())
            {
                ToggleMainMenu(!showMainCanvas);
            }
        }

        public void ToggleMainMenu(bool show)
        {
            if (showMainCanvas == show)
                return;

            ApplyMenuState(show);
        }

        private void ApplyMenuState(bool show)
        {
            showMainCanvas = show;

            Time.timeScale = show ? 0f : 1f;

            // Main menu
            if (mainMenu != null)
                mainMenu.gameObject.SetActive(show);

            // Gameplay UI
            if (gamePlayCanvasii != null)
            {
                foreach (var canvas in gamePlayCanvasii)
                {
                    if (canvas != null)
                        canvas.gameObject.SetActive(!show);
                }
            }
        }
    }
}
