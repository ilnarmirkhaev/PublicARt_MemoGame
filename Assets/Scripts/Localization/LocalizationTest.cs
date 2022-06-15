using UnityEngine;

namespace Localization
{
    public class LocalizationTest : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Changing language to RUSSIAN");
                LocalizedString.ChangeLanguage(LocalizedString.Language.RU);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Changing language to ENGLISH");
                LocalizedString.ChangeLanguage(LocalizedString.Language.EN);
            }
            
        }
    }
}