using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TabsView : MonoBehaviour
{
    [SerializeField] private List<TabButton> _buttons = new List<TabButton>();
    [SerializeField] private List<TabContent> _contents = new List<TabContent>();
    [SerializeField] private List<Image> _tabs = new List<Image>();
    [SerializeField] private Color32 _activeColor;
    [SerializeField] private Color32 _disableColor;
    public void ShowButtonAndHideOther(int number)
    {
        for(int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].gameObject.SetActive(true);
        }

        _buttons[number-1].gameObject.SetActive(false);
    }

    public void ShowTabContent(TabContent tabContent)
    {
        for(int i = 0; i < _contents.Count; i++)
        {
            _contents[i].gameObject.SetActive(false);
        }

        tabContent.gameObject.SetActive(true);
    }

    private void DisableAllTabs()
    {
        for(int i = 0; i < _tabs.Count; i++)
        {
            _tabs[i].color = _disableColor;
        }
    }

    public void SetActiveTab(Image tabImage)
    {
        DisableAllTabs();
        tabImage.color = _activeColor;
    }
}
