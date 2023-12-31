using UnityEngine;
using UnityEngine.Networking;

public class EmailSender : MonoBehaviour
{
    public void EmailButtonClick()
    {
        string mailto = "developer.roborovskii@gmail.com";
        string subject = EscapeURL("버그 리포트 / 기타 문의사항");
        string body = EscapeURL
        (
            "이 곳에 내용을 작성해주세요.\n\n\n\n" +
            "________\n\n" +
            "User ID : " + UserInfo.Data.gamerId + "\n" +
            "User Nickname : " + UserInfo.Data.nickname + "\n" +
            "Device Model : " + SystemInfo.deviceModel + "\n" +
            "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
            "________"
        );
        
        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }

    private string EscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}