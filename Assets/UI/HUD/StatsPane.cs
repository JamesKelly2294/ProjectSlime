using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsPane : MonoBehaviour
{

    public TextMeshProUGUI biodiversityStatus, seaLevelStatus;

    public ProgressBar biodiversity, seaLevel;

    private GameResourceManager gameResourceManager;

    // Start is called before the first frame update
    void Start()
    {
        gameResourceManager = GameObject.FindObjectOfType<GameResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        biodiversity.progress = (((float)gameResourceManager.currentBiodiversity + (float)gameResourceManager.minBiodiversity) / (float)gameResourceManager.maxBiodiversity);
        biodiversityStatus.text = gameResourceManager.BiodiversityYearsRemaining.ToString() + " Years Remaining";

        seaLevel.progress = (((float)gameResourceManager.currentSeaLevels + (float)gameResourceManager.minSeaLevels) / (float)gameResourceManager.maxSeaLevels);
        seaLevelStatus.text = gameResourceManager.SeaLevelsYearsRemaining.ToString() + " Years Remaining";
    }
}
