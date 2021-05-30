using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectCardOverlay : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject populateGridLayout;
	public Button confirmButton;
    public TextMeshProUGUI title;

	public List<AbstractCard> cardsToDisplay = new List<AbstractCard>();
	public List<AbstractCard> selectedCards = new List<AbstractCard>();

	public bool isDone = false;

	// public so we can fiddle around with values in-editor, but these values should be supplied when instantiating the prefab
	public int selectXCards;				// User must select [selectXCards] cards.
	public bool mustSelectExact;		// If true, user must select EXACTLY [selectXCards], else select UP TO [selectXCards].

    // TODO: Change to OnEnable and have Start() just notify if the prefab is disabled
    void Start(){
		// Debug.Log("SelectCardOverlay started; assign cards to cardsToDisplay BEFORE enabling the prefab!");
    }

	void OnEnable(){
		// initialization and cleanup from previous instance
        cardPrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;
        populateGridLayout = transform.Find("Scroll View/Viewport/Content").gameObject;
		confirmButton = transform.Find("Button").gameObject.GetComponent<Button>();
		title = transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>();
		confirmButton.onClick.AddListener(ReturnSelectedCards);

		isDone = false;
		selectedCards.Clear();
		confirmButton.interactable = false;

		string query = (mustSelectExact) ? " " : " up to ";
		string plural = (selectXCards == 1) ? " card" : " cards";
		title.text = "Select" + query + selectXCards + plural;
        PopulateGrid(cardsToDisplay);
		EnableButtonIfConditionsMet();			// check for when we can return nothing
	}

	public void PopulateGrid(List<AbstractCard> cardsToRender){
		for (int i = 0; i < cardsToRender.Count; i++){
			GameObject newObj = (GameObject)Instantiate(cardPrefab, transform);
            newObj.transform.SetParent(populateGridLayout.transform);
			if (newObj.GetComponent<DisplayCard>() != null){
            	newObj.GetComponent<DisplayCard>().reference = cardsToRender[i];
				newObj.GetComponent<DisplayCard>().isInCardOverlay = true;
            	newObj.GetComponent<DisplayCard>().Render();
			}
		}
	}

	public List<AbstractCard> GetSelectedCards(){
		return selectedCards;
	}

	public void EnableButtonIfConditionsMet(){
		int currentCount = selectedCards.Count;
		if ( (mustSelectExact && currentCount == selectXCards) || (!mustSelectExact && currentCount <= selectXCards) ){
			confirmButton.interactable = true;
			return;
		}
		confirmButton.interactable = false;
	}

	void ReturnSelectedCards(){
		isDone = true;
		Destroy(this.gameObject);
	}
}
