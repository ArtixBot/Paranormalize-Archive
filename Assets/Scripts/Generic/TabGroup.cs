using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons = new List<TabButton>();
    public TabButton selectedTab;
    public List<GameObject> contentPanes = new List<GameObject>();

    public void Start(){
        foreach(Transform child in GameObject.Find("Page Group").transform){
            contentPanes.Add(child.gameObject);
        }
        foreach(GameObject pane in contentPanes){
            pane.SetActive(false);
        }
        contentPanes[0].SetActive(true);
    }
    
    public void Subscribe(TabButton button){
        tabButtons.Add(button);
        button.index = tabButtons.Count - 1;
        Debug.Log("Adding index: " + button.index);
        RerenderTabs();
        Debug.Log(GameObject.Find("Page Group"));
    }

    public void OnTabEnter(TabButton button){
        RerenderTabs();
        if (button != selectedTab){
            button.background.color = new Color(0.9f, 0.9f, 0.9f);
        }
    }

    public void OnTabExit(TabButton button){
        RerenderTabs();
    }

    public void OnTabSelected(TabButton button){
        button.background.color = new Color(1, 1, 1);
        selectedTab = button;
        RerenderTabs();
        foreach(GameObject pane in contentPanes){
            pane.SetActive(false);
        }
        contentPanes[button.index].SetActive(true);
    }

    private void RerenderTabs(){
        foreach (TabButton button in tabButtons){
            button.background.color = (button == selectedTab) ? new Color(1, 1, 1) : new Color(0.8f, 0.8f, 0.8f);
        }
    }
}
