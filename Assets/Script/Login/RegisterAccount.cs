using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterAccount : LoginBase
{
    [SerializeField] private Image imageID;
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private Image imagePW;
    [SerializeField] private TMP_InputField inputFieldPW;
    [SerializeField] private Image imageConfirmPW;
    [SerializeField] private TMP_InputField inputFieldConfirmPW;
    [SerializeField] private Image imageEmail;
    [SerializeField] private TMP_InputField inputFieldEmail;
    [SerializeField] private Button btnRegisterAccount;

    public void OnClickRegisterAccount()
    {
        ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;
        if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "비밀번호 확인")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;

        if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
        {
            GuideForIncorrectlyEnterData(imageConfirmPW, "비밀번호가 일치하지 않습니다.");
            return;
        }

        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnterData(imageEmail, "메일 형식이 잘못되었습니다. (ex. address@xx.xx");
            return;
        }

        btnRegisterAccount.interactable = false;
        SetMessage("계정 생성중입니다..");

        CustomSignUp();
    }

    private void CustomSignUp()
    {
        Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
        {
            btnRegisterAccount.interactable = true;

            if (callback.IsSuccess())
            {
                Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        SetMessage($"계정 생성 성공. {inputFieldID.text}님 환영합니다.");
                    }
                });
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 409:
                        message = "이미 존재하는 아이디입니다.";
                        break;
                    case 403:
                    case 401:
                    case 400:
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if (message.Contains("아이디"))
                {
                    GuideForIncorrectlyEnterData(imageID, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}
