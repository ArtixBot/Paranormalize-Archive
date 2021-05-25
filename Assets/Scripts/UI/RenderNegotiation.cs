using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Camera mainCamera;

    // Called whenever we load into the Negotiation scene.
    void Start()
    {
        Debug.Log("RenderNegotiation calls NegotiationManager's StartNegotiation()");
        nm.StartNegotiation();      // Start negotiation! (This also sets up a whole bunch of variables in nm that we can now use for this method)
        
        handZone = GameObject.Find("Canvas/HandZone");
        argPrefab = Resources.Load("Prefabs/ArgumentDisplay") as GameObject;
        cardTemplatePrefab = Resources.Load("Prefabs/CardTemplate") as GameObject;

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

        mainCamera = Camera.main;       // grab main camera
        this.RenderHand();  // Render player hand
    }

    public void RenderHand(){
        // get rid of the old hand to render a new one. maybe excessive - could we just rerender the hand?
        foreach (Transform child in handZone.transform){
            GameObject.Destroy(child.gameObject);
        }
        List<AbstractCard> playerHand = player.GetHand();
        for(int i = 0; i < playerHand.Count; i++){
            AbstractCard card = playerHand[i];
            GameObject cardDisplay = Instantiate(cardTemplatePrefab, handZone.transform.position + new Vector3(i * 200.0f, 0, 0), Quaternion.identity);
            cardDisplay.GetComponent<DisplayCard>().reference = card;
            cardDisplay.transform.SetParent(handZone.transform);
            cardDisplay.SetActive(true);        // calls OnEnable() for the card template prefab
        }
    }

    // public void RenderNonCoreArguments(){
    //     // foreach (Transform child in GameObject.Find("Canvas/PlayerSide").transform){
    //     //     if (!child.gameObject.GetComponent<DisplayArgument>().reference.isCore){
    //     //         GameObject.Destroy(child.gameObject);
    //     //     }
    //     // }
    //     int playerArgCount = player.nonCoreArguments.Count;
    //     for (int i = 0; i < playerArgCount; i++){
    //         float radius = 500;
    //         float angle = i * Mathf.PI * 2f / radius;
    //         Vector3 enemyPos = GameObject.Find("Canvas/PlayerSide/SpawnNonCoreHere").transform.position + (new Vector3(Mathf.Cos(angle) * radius, -2, Mathf.Sin(angle) * radius));
    //         GameObject arg = Instantiate(coreArgPrefab, enemyPos, Quaternion.Euler(0, 0, 0));

    //         arg.GetComponent<DisplayArgument>().reference = player.nonCoreArguments[i];
    //         arg.transform.SetParent(GameObject.Find("Canvas/PlayerSide").transform);
    //         arg.SetActive(true);
    //     }
    // }

    bool moveTheCam = false;
    bool swap = true;
    Vector3 playerPos;
    Vector3 enemyPos;
    void Update(){
        if (Input.GetKeyUp(KeyCode.E)){
            NegotiationManager.Instance.NextTurn();
            RenderHand();
            // RenderNonCoreArguments();

            playerPos = GameObject.Find("Negotiation Background/CamFocusPlayer").transform.position + new Vector3(0, 0, -10);
            enemyPos = GameObject.Find("Negotiation Background/CamFocusEnemy").transform.position + new Vector3(0, 0, -10);
            moveTheCam = true;
        }
    }

    public float cameraSpeed = 100f;
    bool moveCameraRight = true;
    void LateUpdate(){
        if (moveTheCam){
            float value = (moveCameraRight) ? (cameraSpeed * Time.deltaTime) : -(cameraSpeed * Time.deltaTime);

            Vector3 camPos = new Vector3(mainCamera.transform.position.x + value, mainCamera.transform.position.y, -10);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camPos, cameraSpeed);

            if (moveCameraRight && mainCamera.transform.position.x > enemyPos.x){
                moveTheCam = false;
                moveCameraRight = false;
            } else if (!moveCameraRight && mainCamera.transform.position.x < playerPos.x){
                moveTheCam = false;
                moveCameraRight = true;
            }
        }
    }
}
