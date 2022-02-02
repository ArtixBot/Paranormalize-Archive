using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Renders the negotiation screen. Also invokes NegotiationManager's StartNegotiation() to set up game state.
// Maybe have it also handle user input???
public class RenderNegotiation : MonoBehaviour
{
    public NegotiationManager nm = NegotiationManager.Instance;

    public AbstractCharacter player;
    public AbstractEnemy enemy;

    private GameObject cardTemplatePrefab;
    private GameObject argPrefab;
    private GameObject intentPrefab;
    private GameObject handZone;

    private TextMeshProUGUI drawCount;
    private TextMeshProUGUI discardCount;
    private GameObject scourObject;
    private TextMeshProUGUI scourCount;
    private TextMeshProUGUI actionCount;

    [SerializeField]
    private SelectCardOverlay selectCardOverlay;
    [SerializeField]
    private RewardViewOverlay rewardViewOverlay;

    private Button endTurnButton;
    private TextMeshProUGUI roundText;

    private Camera mainCamera;

    // Called whenever we load into the Negotiation scene.
    void Start()
    {
        Debug.Log("RenderNegotiation calls NegotiationManager's StartNegotiation()");
        
        nm.StartNegotiation(this, null);      // Start negotiation! (This also sets up a whole bunch of gameobjects in nm that we can now use for this method)
        
        mainCamera = Camera.main;       // grab main camera
        handZone = GameObject.Find("Canvas/HandZone");
        argPrefab = Resources.Load("Prefabs/ArgumentDisplay") as GameObject;
        cardTemplatePrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;
        intentPrefab = Resources.Load("Prefabs/IntentDisplay") as GameObject;

        drawCount = GameObject.Find("Canvas/TrackDeck/Count").GetComponent<TextMeshProUGUI>();
        discardCount = GameObject.Find("Canvas/TrackDiscard/Count").GetComponent<TextMeshProUGUI>();
        scourCount = GameObject.Find("Canvas/TrackScour/Count").GetComponent<TextMeshProUGUI>();
        actionCount = GameObject.Find("Canvas/TrackAP/Count").GetComponent<TextMeshProUGUI>();
        scourObject = GameObject.Find("Canvas/TrackScour");
        scourObject.SetActive(false);
        endTurnButton = GameObject.Find("Canvas/EndTurnButton").GetComponent<Button>();
        endTurnButton.onClick.AddListener(NegotiationManager.Instance.NextTurn);

        roundText = GameObject.Find("Canvas/EndTurnButton/TurnCounter").GetComponent<TextMeshProUGUI>();

        player = nm.player;
        enemy = nm.enemy;

        // Render core arguments
        GameObject corePlayer = Instantiate(argPrefab, GameObject.Find("Canvas/PlayerSide/SpawnCoreHere").transform.position, Quaternion.identity);
        corePlayer.GetComponent<DisplayArgument>().reference = player.GetCoreArgument();
        corePlayer.transform.SetParent(GameObject.Find("Canvas/PlayerSide/SpawnCoreHere").transform);
        corePlayer.SetActive(true);     // Invoke the core argument's OnEnable() function since the prefab is disabled by default

        GameObject coreEnemy = Instantiate(argPrefab, GameObject.Find("Canvas/EnemySide/SpawnCoreHere").transform.position, Quaternion.identity);
        coreEnemy.GetComponent<DisplayArgument>().reference = enemy.GetCoreArgument();
        coreEnemy.transform.SetParent(GameObject.Find("Canvas/EnemySide/SpawnCoreHere").transform);
        coreEnemy.SetActive(true);

        // this.RenderHand();  // Render player hand
        // this.RenderCounts();
        Redraw();
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
        
        for (int i = 0; i < player.GetArguments().Count; i++){
            float angle = ((i+1) * Mathf.PI * 2f) / 10;     // 10 should be a placeholder number
            Vector3 newPos = new Vector3(parent.position.x + Mathf.Cos(angle) * radius, parent.position.y + Mathf.Sin(angle) * radius, 0);
            
            GameObject arg = Instantiate(argPrefab, newPos, Quaternion.identity);
            arg.GetComponent<DisplayArgument>().reference = player.GetArguments()[i];
            arg.transform.SetParent(parent);
            arg.SetActive(true);
        }

        parent = GameObject.Find("Canvas/EnemySide/SpawnNonCoreHere").transform;
        foreach (Transform child in parent){
            if (child.gameObject.GetComponent<DisplayArgument>()){
                GameObject.Destroy(child.gameObject.GetComponent<DisplayArgument>().tooltipInstance);
            }
            GameObject.Destroy(child.gameObject);
        }
        
        for (int i = 0; i < enemy.GetArguments().Count; i++){
            float angle = ((i+1) * Mathf.PI * 2f) / 10;     // 10 should be a placeholder number
            Vector3 newPos = new Vector3(parent.position.x + Mathf.Cos(angle) * radius, parent.position.y + Mathf.Sin(angle) * radius, 0);
            
            GameObject arg = Instantiate(argPrefab, newPos, Quaternion.identity);
            arg.GetComponent<DisplayArgument>().reference = enemy.GetArguments()[i];
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
        actionCount.text = player.curAP.ToString();
    }

    public void RenderIntents(){
        Transform parent = GameObject.Find("IntentTracker/SpawnIntentsHere").transform;
        foreach (Transform child in parent){
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < enemy.intents.Count; i++){
            GameObject arg = Instantiate(intentPrefab, new Vector3(64 + parent.position.x + 64 * i, parent.position.y, parent.position.z), Quaternion.identity);
            arg.transform.SetParent(GameObject.Find("IntentTracker/SpawnIntentsHere").transform);
            arg.GetComponent<RenderEnemyIntent>().reference = enemy.intents[i];
            if (enemy.intents[i].intentType == EnemyIntent.IntentType.ATTACK){
                arg.GetComponent<RenderEnemyIntent>().image.sprite = Resources.Load<Sprite>("Images/intent-attack");
            } else if (enemy.intents[i].intentType == EnemyIntent.IntentType.BUFF_SKILL || enemy.intents[i].intentType == EnemyIntent.IntentType.DEBUFF_SKILL){
                arg.GetComponent<RenderEnemyIntent>().image.sprite = Resources.Load<Sprite>("Images/intent-skill");
            } else if (enemy.intents[i].intentType == EnemyIntent.IntentType.TRAIT){
                arg.GetComponent<RenderEnemyIntent>().image.sprite = Resources.Load<Sprite>("Images/intent-trait");
            } else {
                arg.GetComponent<RenderEnemyIntent>().image.sprite = Resources.Load<Sprite>("Images/Arguments/adaptive");
            }
        }
    }

    public void Redraw(){
        foreach (var prefab in GameObject.FindGameObjectsWithTag("Tooltip")){
            Destroy(prefab);
        }
        RenderHand();
        RenderNonCoreArguments();
        RenderCounts();
        RenderIntents();
        roundText.text = "Turn " + NegotiationManager.Instance.round;
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

    public void DisplayVictoryScreen(List<AbstractCard> draftCards, int moneyEarned, int masteryEarned){
        rewardViewOverlay.moneyToReward = moneyEarned;
        rewardViewOverlay.masteryToReward = masteryEarned;
        rewardViewOverlay.listOfDraftCards = draftCards;
        
        rewardViewOverlay.gameObject.SetActive(true);       // editor bug causes this to run twice for some reason -- doesn't happen once built though
        rewardViewOverlay.GetComponent<RewardViewOverlay>().Render();
    }

    public void EndNegotiationRender(){
        StartCoroutine(ReturnToOverworld());
    }

    private IEnumerator ReturnToOverworld(){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Overworld");
        while (!asyncLoad.isDone){
            yield return null;
        }
    }
}
