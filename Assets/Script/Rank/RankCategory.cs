using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RankType
{
    Daily,
    Weekly,
    Monthly,
    Overall,
}

public class RankCategory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankCategory;
    [SerializeField] private List<GameObject> rankPanels;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private int _index;

    public void InitRankPanel()
    {
        _index = 0;
        leftButton.interactable = false;
        SetPanel();
        gameObject.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void OnClickRightButton()
    {
        _index++;
        leftButton.interactable = true;
        rightButton.interactable = true;

        if (_index == rankPanels.Count)
        {
            rightButton.interactable = false;
            _index--;
        }

        SetPanel();
    }
    
    public void OnClickLeftButton()
    {
        _index--;
        leftButton.interactable = true;
        rightButton.interactable = true;

        if (_index == -1)
        {
            leftButton.interactable = false;
            _index = 0;
        }

        SetPanel();
    }

    public void OnClickCloseButton()
    {
        for (int i = 0; i < rankPanels.Count; i++)
        {
            rankPanels[i].SetActive(false); 
        }
        
        gameObject.SetActive(false);
    }

    private void SetPanel()
    {
        RankType rankType = (RankType)_index;
        rankCategory.text = rankType.ToString();
        
        for (int i = 0; i < rankPanels.Count; i++)
        {
            if (i == _index)
            {
                rankPanels[i].SetActive(true);
            }
            else
            {
                rankPanels[i].SetActive(false);   
            }
        }
    }
}
