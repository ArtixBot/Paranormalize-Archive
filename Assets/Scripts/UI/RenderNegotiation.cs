using System.Collections;
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

    private GameObject cardTemplatePrefab;
    private GameObject argPrefab;
    private GameObject handZone;

    private TextMeshProUGUI drawCount;
    private TextMeshProUGUI discardCount;
    private GameObject scourObject;
    private TextMeshProUGUI scourCount;
    private TextMeshProUGUI actionCount;

    private Button endTurnButton;

    private Camera mainCamera;

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
        actionCount = GameObject.Find("Canvas/TrackAP/Count").GetComponent<TextMeshProUGUI>();
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

    public int renderCardHorizontalDistance;      // set in inspector

    public void RenderHand(){
        // get rid of the old hand to render a new one. maybe excessive - could we just rerender the hand?
        foreach (Transform child in handZone.transform){
            GameObject.Destroy(child.gameObject);
        }
        List<AbstractCard> playerHand = player.GetHand();
        int cardsInHand = playerHand.Count;

        int startXPos = (-renderCardHorizontalDistance/2) * (cardsInHand - 1);
        int startAng = 5 * (cardsInHand - 1);
        for (int i = 0; i < cardsInHand; i++){
            GameObject cardDisplay = Instantiate(cardTemplatePrefab, handZone.transform.position + new Vector3(startXPos + (i * renderCardHorizontalDistance), 0, 0), Quaternion.identity);
            cardDisplay.transform.Rotate(0, 0, startAng - (10 *i));
            cardDisplay.transform.SetParent(handZone.transform);
            cardDisplay.GetComponent<DisplayCard>().reference = playerHand[i];
            cardDisplay.GetComponent<DisplayCard>().Render();
            // cardDisplay.SetActive(true);        // calls OnEnable() for the card template prefab
        }
    }

    public float radius = 340f;

    public void RenderNonCoreArguments(){
        Transform parent = GameObject.Find("Canvas/PlayerSide/SpawnNonCoreHere").transform;
        foreach (Transform child in parent){
            if (child.gameObject.GetComponent<DisplayArgument>()){
                GameObject.Destroy(child.gameObject.GetComponent<DisplayArgument>().tooltipInstance);
            }
            GameObject.Destroy(child.gameObject);
        }
        
        for (int i = 0; i < player.nonCoreArguments.Count; i++){
            float angle = ((i+1) * Mathf.PI * 2f) / 10;     // 10 should be a placeholder number
            Vector3 newPos = new Vector3(parent.position.x + Mathf.Cos(angle) * radius, parent.position.y + Mathf.Sin(angle) * radius, 0);
            
            GameObject arg = Instantiate(argPrefab, newPos, Quaternion.identity);
            arg.GetComponent<DisplayArgument>().reference = player.nonCoreArguments[i];
            arg.transform.SetParent(parent);
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
        actionCount.text = player.curAP + "/" + player.maxAP;
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
