﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Renders the negotiation screen. Also invokes NegotiationManager's StartNegotiation() to set up game state.
// Maybe have it also handle user input???
public class RenderNegotiation : MonoBehaviour
{
    public NegotiationManager nm = NegotiationManager.Instance;

    public AbstractCharacter player;
    public AbstractCharacter enemy;

    public GameObject cardTemplatePrefab;
    public GameObject argPrefab;
    public GameObject handZone;

    public TextMeshProUGUI drawCount;
    public TextMeshProUGUI discardCount;
    public GameObject scourObject;
    public TextMeshProUGUI scourCount;

    public Button endTurnButton;

    public Camera mainCamera;

    // Called whenever we load into the Negotiation scene.
    void Start()
    {
        Debug.Log("RenderNegotiation calls NegotiationManager's StartNegotiation()");
        nm.StartNegotiation(this);      // Start negotiation! (This also sets up a whole bunch of gameobjects in nm that we can now use for this method)
        
        mainCamera = Camera.main;       // grab main camera
        handZone = GameObject.Find("Canvas/HandZone");
        argPrefab = Resources.Load("Prefabs/ArgumentDisplay") as GameObject;
        cardTemplatePrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;

        drawCount = GameObject.Find("Canvas/TrackDeck/Count").GetComponent<TextMeshProUGUI>();
        discardCount = GameObject.Find("Canvas/TrackDiscard/Count").GetComponent<TextMeshProUGUI>();
        scourCount = GameObject.Find("Canvas/TrackScour/Count").GetComponent<TextMeshProUGUI>();
        scourObject = GameObject.Find("Canvas/TrackScour");
        scourObject.SetActive(false);
        endTurnButton = GameObject.Find("Canvas/EndTurnButton").GetComponent<Button>();
        endTurnButton.onClick.AddListener(NegotiationManager.Instance.NextTurn);

        player = nm.player;
        enemy = nm.enemy;

        // Render core arguments
        GameObject corePlayer = Instantiate(argPrefab, GameObject.Find("Canvas/PlayerSide/SpawnCoreHere").transform.position, Quaternion.identity);
        corePlayer.GetComponent<DisplayArgument>().reference = player.GetCoreArgument();
        corePlayer.transform.SetParent(GameObject.Find("Canvas/PlayerSide").transform);
        corePlayer.SetActive(true);     // Invoke the core argument's OnEnable() function since the prefab is disabled by default

        GameObject coreEnemy = Instantiate(argPrefab, GameObject.Find("Canvas/EnemySide/SpawnCoreHere").transform.position, Quaternion.identity);
        coreEnemy.GetComponent<DisplayArgument>().reference = enemy.GetCoreArgument();
        coreEnemy.transform.SetParent(GameObject.Find("Canvas/EnemySide").transform);
        coreEnemy.SetActive(true);

        this.RenderHand();  // Render player hand
        this.RenderCounts();
    }

    public float renderCardHorizontalDistance;      // set in inspector

    public void RenderHand(){
        // get rid of the old hand to render a new one. maybe excessive - could we just rerender the hand?
        foreach (Transform child in handZone.transform){
            GameObject.Destroy(child.gameObject);
        }
        List<AbstractCard> playerHand = player.GetHand();
        for(int i = 0; i < playerHand.Count; i++){
            AbstractCard card = playerHand[i];
            GameObject cardDisplay = Instantiate(cardTemplatePrefab, handZone.transform.position + new Vector3(i * renderCardHorizontalDistance, 0, 0), Quaternion.identity);
            cardDisplay.transform.Rotate(0, 0, 20 - (10 *i));
            cardDisplay.transform.SetParent(handZone.transform);
            cardDisplay.GetComponent<DisplayCard>().reference = card;
            cardDisplay.GetComponent<DisplayCard>().Render();
            // cardDisplay.SetActive(true);        // calls OnEnable() for the card template prefab
        }
    }

    public void RenderNonCoreArguments(){
        Transform parent = GameObject.Find("Canvas/PlayerSide/SpawnNonCoreHere").transform;
        foreach (Transform child in parent){
            GameObject.Destroy(child.gameObject.GetComponent<DisplayArgument>().tooltipInstance);
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < player.nonCoreArguments.Count; i++){
            // float radius = 500;
            // Vector3 pos = GameObject.Find("Canvas/PlayerSide/SpawnNonCoreHere").transform.position + (new Vector3(Mathf.Cos(angle) * radius, -2, Mathf.Sin(angle) * radius));
            // float angle = i * Mathf.PI * 2f / radius;
            GameObject arg = Instantiate(argPrefab, new Vector3(parent.position.x, parent.position.y + 200.0f * i, 0), Quaternion.identity);
            arg.GetComponent<DisplayArgument>().reference = player.nonCoreArguments[i];
            arg.transform.SetParent(GameObject.Find("Canvas/PlayerSide/SpawnNonCoreHere").transform);
            arg.SetActive(true);
        }
    }

    public void RenderCounts(){
        drawCount.text = player.GetDrawPile().GetSize().ToString();
        discardCount.text = player.GetDiscardPile().GetSize().ToString();
        if (player.GetScourPile().GetSize() > 0){
            scourObject.SetActive(true);
        }
        scourCount.text = player.GetScourPile().GetSize().ToString();
    }

    void Update(){
        if (Input.GetKeyUp(KeyCode.E)){
            NegotiationManager.Instance.NextTurn();
        }
    }

    bool moveCameraRight = false;
    Vector3 playerPos;
    Vector3 enemyPos;
    public void Redraw(){
        foreach (var prefab in GameObject.FindGameObjectsWithTag("Tooltip")){
            Destroy(prefab);
        }
        RenderHand();
        RenderNonCoreArguments();
        RenderCounts();

        playerPos = GameObject.Find("Negotiation Background/CamFocusPlayer").transform.position + new Vector3(0, 0, -10);
        enemyPos = GameObject.Find("Negotiation Background/CamFocusEnemy").transform.position + new Vector3(0, 0, -10);
        moveCameraRight = !moveCameraRight;
    }

    public float duration = 1f;       // this doesn't seem to do anything for some reason
    void LateUpdate(){
        if (mainCamera.transform.position.z > -10){
            mainCamera.transform.position = mainCamera.transform.position + new Vector3(0, 0, -10);
        }
        if (moveCameraRight){
            StartCoroutine(MoveCameraTo(mainCamera.transform.position, enemyPos, duration));
        } else {
            StartCoroutine(MoveCameraTo(mainCamera.transform.position, playerPos, duration));
        }
    }

    IEnumerator MoveCameraTo(Vector3 oldPos, Vector3 newPos, float duration){
        for (float f = 0f; f < duration/4; f += 3*Time.deltaTime){
            mainCamera.transform.position = Vector3.Lerp(oldPos, newPos, f/duration);
            yield return 0;
        }
        mainCamera.transform.position = newPos;
    }

    public void DisplayCardSelectScreen(List<AbstractCard> cardsToDisplay, int numToSelect, bool mustSelectExact){
        GameObject prefab = Resources.Load("Prefabs/SelectCardOverlay") as GameObject;
        Transform parentLoc = GameObject.Find("Canvas").transform;

        SelectCardOverlay screen = Instantiate(prefab, parentLoc.position, Quaternion.identity).GetComponent<SelectCardOverlay>();
        screen.cardsToDisplay = cardsToDisplay;
        screen.selectXCards = numToSelect;
        screen.mustSelectExact = mustSelectExact;
        screen.gameObject.SetActive(true);
        screen.gameObject.transform.SetParent(parentLoc);

        StartCoroutine(WaitUntilSelected(screen));
    }

    IEnumerator WaitUntilSelected(SelectCardOverlay screen){
        while (!screen.isDone){
            yield return null;
        }
        nm.SelectedCards(screen.selectedCards);
    }
}
