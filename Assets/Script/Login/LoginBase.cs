using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    protected void ResetUI(params Image[] images)
    {
        textMeshProUGUI.text = string.Empty;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.white;
        }
    }

    protected void SetMessage(string msg)
    {
        textMeshProUGUI.text = msg;
    }

    protected void GuideForIncorrectlyEnterData(Image image, string msg)
    {
        textMeshProUGUI.text = msg;
        image.color = Color.red;
    }

    protected bool IsFieldDataEmpty(Image image, string field, string result)
    {
        if (field.Trim().Equals(""))
        {
            GuideForIncorrectlyEnterData(image, $"\"{result}\"필드를 채워주세요.");

            return true;
        }

        return false;
    }
}
