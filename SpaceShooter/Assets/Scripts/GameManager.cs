using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    int shipId = 0;

    [Header("Music")]
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource musicPlayer2;
    [SerializeField] AudioClip menuTheme;
    [SerializeField] AudioClip noCombat;
    [SerializeField] AudioClip combat;
    [SerializeField] AudioClip pause;
    [SerializeField] AudioClip introOutro;

    [Header("Boss Tracks")]
    [SerializeField] AudioClip boss1;
    [SerializeField] AudioClip boss2;
    [SerializeField] AudioClip boss3;

    [SerializeField] AudioMixer mixer;
    [SerializeField] Image whiteScreen;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] PlayerController player;
    [SerializeField] List<GameObject> sectorContainers = new List<GameObject>();
    [SerializeField] SectorContainer currentSector;
    [SerializeField] SectorContainer previousSector;

    float mouseSensitivity = 1;
    float scrollSpeed = 10;
    float playerRep = 0;
    int primaryPlayer = 0;
    float fadeAmount = 0f;
    float fadeSpeed = 1f;
    bool crossFading = false;
    bool canChangeMusic = true;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (crossFading)
        {
            fadeAmount += fadeSpeed * Time.deltaTime;
            //if primary player is 0 we should fade that one in
            if (primaryPlayer == 0)
            {
                musicPlayer.volume = fadeAmount;
                musicPlayer2.volume = 1 - fadeAmount;
            }
            else
            {
                musicPlayer.volume = 1 - fadeAmount;
                musicPlayer2.volume = fadeAmount;
            }
            if (fadeAmount >= 1)
            {
                crossFading = false;
                fadeAmount = 0;
            }
        }
    }

    public void UnlockMusic()
    {
        canChangeMusic = true;
    }

    //called whenever a ShipController is added. The ShipController will get a new ID
    public int RegisterShip()
    {
        shipId += 1;
        return shipId;
    }

    public void SetCombatState(bool inCombat)
    {
        if (inCombat)
        {
            CrossFadeAudio("inCombat", .25f);
        }
        else
        {
            CrossFadeAudio("inPlay", .25f);
        }
    }

    public void SetMusicVolume(float amt)
    {
        mixer.SetFloat("Music", (1 - amt) * -40);
    }

    public float GetMusicVolume()
    {
        float value;
        mixer.GetFloat("Music", out value);
        return 1 - (value / -40);
    }

    public void SetSFXVolume(float amt)
    {
        mixer.SetFloat("SFX", (1 - amt) * -40);
    }

    public float GetSFXVolume()
    {
        float value;
        mixer.GetFloat("SFX", out value);
        return 1 - (value / -40);
    }

    public void BeginIntroSequence()
    {
        SceneTransition(2, "Intro", "intro");
    }

    public void CrossFadeAudio(string newClip, float fadeSpeed = 1)
    {
        if (!canChangeMusic)
        {
            Debug.LogError("Cannot Transition to: " + newClip + " because the MusicPlayer is locked at this time.");
            return;
        }
        AudioClip fadeInClip = null;

        if (newClip == "intro")
        {
            fadeInClip = introOutro;
        }
        else if (newClip == "inPlay")
        {
            fadeInClip = noCombat;

        }
        else if (newClip == "inCombat")
        {
            fadeInClip = combat;
        }
        else if (newClip == "pause")
        {
            fadeInClip = pause;
        }
        else if (newClip == "menu")
        {
            fadeInClip = menuTheme;
        }
        else if (newClip == "boss1")
        {
            fadeInClip = boss1;
        }
        else if (newClip == "boss2")
        {
            fadeInClip = boss2;
        }
        else if (newClip == "boss3")
        {
            fadeInClip = boss3;
        }
        else if (newClip == "boss3")
        {
            fadeInClip = boss3;
        }



        if (primaryPlayer == 0)
        {
            musicPlayer2.clip = fadeInClip;
            musicPlayer2.volume = 0;
            musicPlayer2.Play();
            primaryPlayer = 1;
        }
        else
        {
            musicPlayer.clip = fadeInClip;
            musicPlayer.volume = 0;
            musicPlayer.Play();
            primaryPlayer = 0;
        }

        crossFading = true;
        this.fadeSpeed = fadeSpeed;
    }

    public void SceneTransition(float transitionTime, string sceneName, string newMusic = null, bool bossFight = false, bool spawnPlayer = false)
    {
        Debug.Log("calling scene transition with:" + sceneName);
        if (sceneName == SceneManager.GetActiveScene().name)
            return;

        if (sceneName == "MainMenu")
        {
            DestroyPlayer();
        }

        if ((sceneName == "Center" || spawnPlayer) && player == null)
        {
            Debug.Log("initial spawn player");
            player = Instantiate(playerPrefab, new Vector3(0, -10, 0), Quaternion.identity, null).GetComponent<PlayerController>();
        }

        StartCoroutine(FadeIn(transitionTime / 2, sceneName));
        if (newMusic != null)
        {
            CrossFadeAudio(newMusic, transitionTime);
            if (bossFight)
            {
                canChangeMusic = false;
            }
        }
    }



    IEnumerator FadeIn(float time, string scene)
    {
        Color tempColor = whiteScreen.color;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            tempColor.a = elapsedTime / time;
            whiteScreen.color = tempColor;
            yield return null;
        }
        if (scene == "Boss Scene")
        {
            player.ChangeBG(1);
            player.SpawnAtPoint(new Vector3(7.5f, -7.5f, 0));
        }
        else if (scene == "Boss 2 Scene")
        {
            player.ChangeBG(2);
            player.SpawnAtPoint(new Vector3(-31, 53, 0));
        }
        else if (scene == "Boss 3 Scene")
        {
            player.ChangeBG(3);
            player.SpawnAtPoint(new Vector3(-31, 53, 0));
        }
        else if (scene == "Center")
        {
            player.ChangeBG(0);
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "Boss Scene" || currentScene == "Boss 2 Scene" || currentScene == "Boss 3 Scene")
                ResetPlayerReputation();
            if (playerRep > -1)
                player.SpawnAtPoint(new Vector3(0, -10, 0));
            else
                player.SpawnAtPoint(new Vector3(78, -280, 0));
        }
        SceneManager.LoadScene(scene);
        StartCoroutine(FadeOut(time));
    }

    IEnumerator FadeOut(float time)
    {
        Color tempColor = whiteScreen.color;
        float timeLeft = time;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            tempColor.a = timeLeft / time;
            whiteScreen.color = tempColor;
            yield return null;
        }
    }

    public void ResetPlayerReputation()
    {
        player.GetComponent<PlayerStats>().SetReputation(playerRep);
    }

    public void DestroyPlayer()
    {
        Destroy(player);
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public void EnterBossFight(float transitionTime, string sceneName, string bossMusic, Vector3 spawnPos)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        playerRep = stats.reputation;
        SceneTransition(transitionTime, sceneName, bossMusic, true);
        stats.SetReputation(0);
    }

    public void RespawnPlayer()
    {
        canChangeMusic = true;
        if (SceneManager.GetActiveScene().name != "Center")
        {
            SceneTransition(3f, "Center", "inPlay", false);
        }
        else
        {
            if (player.GetStats().reputation > -1)
                player.SpawnAtPoint(new Vector3(0, -10, 0));
            else
                player.SpawnAtPoint(new Vector3(78, -280, 0));
        }
    }

    public void EnterSector(SectorContainer sector)
    {
        Debug.Log("Entering sector with sector number:" + sector.GetSectorNumber());

        if(currentSector != null && currentSector.GetSectorNumber() != sector.GetSectorNumber())
        {
            currentSector.Hide();
        }

        currentSector = sector;

        currentSector.Show();

    }

    public void SetMouseSensitivity(TMP_InputField sensitivity)
    {
        mouseSensitivity = float.Parse(sensitivity.text);
    }

    public float GetMouseSensitivity()
    {
        return mouseSensitivity;
    }

    public void SetScrollSpeed(TMP_InputField speed)
    {
        scrollSpeed = float.Parse(speed.text);
    }

    public float GetScrollSpeed()
    {
        return scrollSpeed;
    }
}