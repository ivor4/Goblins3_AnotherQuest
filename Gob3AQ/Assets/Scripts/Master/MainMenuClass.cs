using UnityEditor;
using UnityEngine;
using MVerse.VARMAP.GameMenu;

namespace MVerse.MainMenu
{
    public class MainMenuClass : MonoBehaviour
    {
        private static MainMenuClass _singleton;

        private void Awake()
        {
            if(_singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(Screen.safeArea);

            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            if (GUILayout.Button("Start Game"))
            {
                VARMAP_GameMenu.START_GAME(out _);
            }
            
            if(GUILayout.Button("One Boss Fight - Space Mama"))
            {
                VARMAP_GameMenu.LOAD_OBF(VARMAP.Types.OBFight.OBF_SPACE_MAMA, out _);
            }

            
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }
    }
}