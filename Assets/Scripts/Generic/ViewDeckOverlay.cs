using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Very similar to SelectCardOverlay, but the cards in this case can be viewed (and upgradable if mastery points are available)
public class ViewDeckOverlay : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject populateGridLayout;
    public List<AbstractCard> cardsToDisplay = new List<AbstractCard>();

    void OnEnable(){
        cardPrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;
        populateGridLayout = transform.Find("Scroll View/Viewport/Card Display").gameObject;
        cardsToDisplay = GameState.mainChar.permaDeck.ToList();
        
        foreach(Transform child in populateGridLayout.transform){
            Destroy(child.gameObject);
        }
        PopulateGrid(cardsToDisplay);
    }

    public void PopulateGrid(List<AbstractCard> cardsToRender){
		for (int i = 0; i < cardsToDisplay.Count; i++){
			GameObject newObj = (GameObject)Instantiate(cardPrefab, transform);
            newObj.transform.SetParent(populateGridLayout.transform);
			if (newObj.GetComponent<DisplayCard>() != null){
            	newObj.GetComponent<DisplayCard>().reference = cardsToRender[i];
				// newObj.GetComponent<DisplayCard>().isInCardOverlay = true;		// should be handled with Render() already
            	newObj.GetComponent<DisplayCard>().Render();
			}
		}
	}
}
