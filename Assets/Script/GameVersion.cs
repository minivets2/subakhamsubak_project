using TMPro;
using UnityEngine;

public class GameVersion : MonoBehaviour
{
    private TMP_Text _version;

    private void Start()
    {
        _version = GetComponent<TMP_Text>();
        _version.text = "v." + Application.version;
    }
}
