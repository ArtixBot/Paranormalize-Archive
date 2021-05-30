using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardOverlay : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject populateGridLayout;
    public int numberToCreate; // number of objects to create. Exposed in inspector

    // Start is called before the first frame update
    void Start()
    {
        cardPrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;
        populateGridLayout = transform.Find("Scroll View/Viewport/Content").gameObject;
        Populate();
    }

	void Populate()
	{
		GameObject newObj; // Create GameObject instance

		for (int i = 0; i < numberToCreate; i++)
		{
			 // Create new instances of our prefab until we've created as many as we specified
			newObj = (GameObject)Instantiate(cardPrefab, transform);
            newObj.transform.SetParent(populateGridLayout.transform);
			if (newObj.GetComponent<DisplayCard>() != null){
            	newObj.GetComponent<DisplayCard>().reference = new DeckardTrashTalk();
            	newObj.GetComponent<DisplayCard>().Render();
			}
		}

	}
}
