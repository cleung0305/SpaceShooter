using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStatsUI : MonoBehaviour
{

    [SerializeField] GameObject bg;
    [SerializeField] GameObject border;
    [SerializeField] Image shipIcon;
    [SerializeField] Image factionIcon;
    [SerializeField] TextMeshProUGUI hullText;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI factionText;
    [SerializeField] TextMeshProUGUI reputationText;
    [SerializeField] TextMeshProUGUI unitsText;

    [SerializeField] List<Image> hyperdriveComps = new List<Image>();
    [SerializeField] GameObject combineBtn;
    int obtainedParts = 0;
    public void UpdateDisplay(Sprite shipSprite, Sprite factionSprite, string hulltxt, string shieldtxt, string speedtxt, string factiontxt, string reptxt, string unitstxt, bool h1, bool h2, bool h3)
    {
        bg.SetActive(true);
        border.SetActive(true);
        shipIcon.sprite = shipSprite;
        factionIcon.sprite = factionSprite;
        hullText.text = hulltxt;
        shieldText.text = shieldtxt;
        speedText.text = speedtxt;
        factionText.text = factiontxt;
        reputationText.text = reptxt;
        unitsText.text = unitstxt;
    }

    public void Hide()
    {
        bg.SetActive(false);
        border.SetActive(false);
    }

    public void AquireHyperDrivePart(int partNum)
    {
        hyperdriveComps[partNum].gameObject.SetActive(true);
        obtainedParts += 1;
        if(obtainedParts == 3)
        {
            combineBtn.SetActive(true);
        }
    }

    public void EndGame()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SceneTransition(6, "Outro", "intro", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
