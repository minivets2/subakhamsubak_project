using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : LoginBase
{
    [SerializeField] private Image imageID;
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private Image imagePW;
    [SerializeField] private TMP_InputField inputFieldPW;
    [SerializeField] private Button btnLogin;

    public void OnClickLogin()
    {
        ResetUI(imageID, imagePW);

        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;

        btnLogin.interactable = false;

        StartCoroutine(nameof(LoginProgress));

        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    private void ResponseToLogin(string ID, string PW)
    {
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProgress));

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}님 환영합니다.");
            }
            else
            {
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401:
                        message = callback.GetMessage().Contains("customId") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호입니다.";
                        break;
                    case 403:
                        message = callback.GetMessage().Contains("user") ? "차단당한 유저입니다." : "차단당한 디바이스입니다.";
                        break;
                    case 410:
                        message = "탈퇴가 진행중인 유저입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if (message.Contains("비밀번호"))
                {
                    GuideForIncorrectlyEnterData(imagePW, message);
                }
                else
                {
                    GuideForIncorrectlyEnterData(imageID, message);
                }
            }
        });
    }

    private IEnumerator LoginProgress()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;
            
            SetMessage($"로그인 중입니다... {time:F1}");
            
            yield return null;
        }
    }
}
