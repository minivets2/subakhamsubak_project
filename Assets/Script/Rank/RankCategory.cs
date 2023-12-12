using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private int _index;

    public void InitRankPanel()
    {
        _index = 0;
        SetPanel();
        gameObject.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void OnClickRightButton()
    {
        _index++;

        if (_index == rankPanels.Count)
            _index--;
        
        SetPanel();
    }
    
    public void OnClickLeftButton()
    {
        _index--;
        
        if (_index == -1)
            _index = 0;
        
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
