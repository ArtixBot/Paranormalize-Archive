using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDrafts : MonoBehaviour{

    public GameObject cardPrefab;
    public List<AbstractCard> cardsToDraft;

    void OnEnable(){
        cardPrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;
    }

    public void Render(){
        if (cardsToDraft == null){
            cardsToDraft = new List<AbstractCard>();
        }
        PopulateGrid(cardsToDraft);
    }

    private void PopulateGrid(List<AbstractCard> cardsToRender){
        for (int i = 0; i < cardsToRender.Count; i++){
			GameObject newObj = (GameObject)Instantiate(cardPrefab, this.gameObject.transform);
            newObj.transform.SetParent(this.gameObject.transform);
			if (newObj.GetComponent<DisplayCard>() != null){
            	newObj.GetComponent<DisplayCard>().reference = cardsToRender[i];
            	newObj.GetComponent<DisplayCard>().Render();
			}
		}
    }
}
