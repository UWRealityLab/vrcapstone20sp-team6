using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    
    public ShipActions ship_actions;
    private Text showProgress;
    private int numDelivered;

    public List<IslandRadiusUpdate> islands;

    // Start is called before the first frame update
    void Start()
    {
        showProgress = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        IslandRadiusUpdate visitedIsland = null;
        // Detect if ship is next to dock
        foreach (IslandRadiusUpdate unvisitedIsland in islands) {
            if (unvisitedIsland.inRadius) {
                // Detect if anchor is set
                if (ship_actions.anchorDown) {
                    // Detect if hoist is lowered
                    if (ship_actions.hoistDown) {
                        // Add to score
                        numDelivered++;
                        showProgress.text = "Supplies delivered to: " + numDelivered + " out of " + islands.size() + " islands";
                        visitedIsland = unvisitedIsland;
                        break;
                    }
                }
            }
        }
        // Remove island from list
        if (visitedIsland != null) {
            islands.Remove(visitedIsland);
        }
    }
}
