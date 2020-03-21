using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yeeter
{
    public class GameLoader : MonoBehaviour
    {
        [Tooltip("The text on which to display epic lmao loading messages.")]
        [SerializeField] private Text _text = null;

        private int _step = 0;

        private void Update()
        {
            switch (_step)
            {
                case 0:
                    _text.text = "Loading mods...";
                    StreamingAssetsDatabase.LoadModules();
                    _step = 1;
                    return;
                case 1:
                    _text.text = "Throwing out unused mods...";
                    StreamingAssetsDatabase.LoadActiveModules();
                    _step = 2;
                    return;
                case 2:
                    _text.text = "Loading textures...";
                    StreamingAssetsDatabase.LoadTexturesFromActiveModules();
                    _step = 3;
                    return;
                case 3:
                    _text.text = "Loading scripts...";
                    StreamingAssetsDatabase.LoadScriptsFromActiveModules();
                    _step = 4;
                    return;
                case 4:
                    _text.text = "Loading sounds...";
                    StreamingAssetsDatabase.LoadSoundsFromActiveModules();
                    StreamingAssetsDatabase.OnSoundsDoneLoading += () =>
                    {
                        _step = 5;
                    };
                    return;
                case 5:
                    _text.text = "Loading defs...";
                    StreamingAssetsDatabase.LoadDefsFromActiveModules();
                    _step = 6;
                    return;
                case 6:
                    _text.text = "Loading settings...";
                    StreamingAssetsDatabase.LoadSettings();
                    _step = 7;
                    return;
                case 7:
                    SoundManager.Initialize();
                    _text.text = "Loading Main Menu...";
                    SceneManager.LoadScene(1);
                    return;
            }
        }
    }
}