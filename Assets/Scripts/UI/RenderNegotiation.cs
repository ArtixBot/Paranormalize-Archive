using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Renders the negotiation screen. Also invokes NegotiationManager's StartNegotiation() to set up game state.
public class RenderNegotiation : MonoBehaviour
{
    public NegotiationManager nm;

    public AbstractCharacter player;
    public AbstractCharacter enemy;

    public GameObject coreArgPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Set variables
        nm = NegotiationManager.Instance;
        List<AbstractCharacter> characters = nm.tm.GetTurnList();       // This should only be size 2 at any given point.
        for (int i = 0; i < characters.Count; i++){
            if (characters[i].FACTION == FactionType.PLAYER){
                player = characters[i];
            } else {
                enemy = characters[i];
            }
        }
        coreArgPrefab = Resources.Load("Prefabs/CoreArgumentDisplay") as GameObject;

        // Render core arguments
        GameObject corePlayer = Instantiate(coreArgPrefab, GameObject.Find("Canvas/PlayerSide/SpawnCoreHere").transform.position, Quaternion.identity);
        corePlayer.GetComponent<DisplayArgument>().reference = player.GetCoreArgument();
        corePlayer.transform.SetParent(GameObject.Find("Canvas/PlayerSide").transform);
        corePlayer.SetActive(true);

        GameObject coreEnemy = Instantiate(coreArgPrefab, GameObject.Find("Canvas/EnemySide/SpawnCoreHere").transform.position, Quaternion.identity);
        coreEnemy.GetComponent<DisplayArgument>().reference = enemy.GetCoreArgument();
        coreEnemy.transform.SetParent(GameObject.Find("Canvas/EnemySide").transform);
        coreEnemy.SetActive(true);      // Don't need a SetActive(false) since the prefab is disabled by default when instantiated
        
        Debug.Log("RenderNegotiation calls NegotiationManager's StartNegotiation()");
        nm.StartNegotiation();      // Start negotiation!
    }

    // void Render(){

    // }
}
