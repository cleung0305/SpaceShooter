using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//This script controls the player, both movement and ingame functions
public class PlayerController : ShipController
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Animator shieldAnim;
    [SerializeField] Transform spawnPoint;

    [SerializeField] List<GameObject> bgPrefabs = new List<GameObject>();
    private float alphaLevel = 0.01f;
    private float maxAlphaLevel = 0.8f;
    private float minAlphaLevel = 0.2f;
    private float fadeInSpeed = 0.0005f;
    private float fadeOutSpeed = 0.01f;
    float deadTimer = 0f;
    [SerializeField] private GameObject playerShield;
    [SerializeField] private ShieldOnHit shieldOnHit;
    [SerializeField] bool mouseMovement = false;
    [SerializeField] List<float> weaponCDs = new List<float>();

    [SerializeField] float TS = 250;
    [SerializeField] float defaultTS = 250;
    bool inCombat = false;
    bool respawning = false;
    PlayerStats pStats;

    public override void Initialize(ShipStats newStats)
    {
        base.Initialize(newStats);
        DontDestroyOnLoad(this);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.SetPlayer(this);
        for (int i = 0; i < stats.weapons.Count; i++)
        {
            weaponCDs.Add(0);
        }
        pStats = (PlayerStats)newStats;
        TS = defaultTS;
    }

    void Update()
    {
        base.Update(); //call the base ShipController update to perform generic functions

        if(isDead)
        {
            deadTimer += Time.deltaTime;
            if(deadTimer >= 3 && !respawning)
            {
                respawning = true;
                gameManager.RespawnPlayer();
            }
        }

        HandleMusic();

        if (Input.GetButtonDown("Menu"))
        {
            mouseMovement = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

        if (Input.GetButtonDown("Toggle Mouse Rotation"))
            ToggleMouseMovement();

        if (Input.GetButton("Shoot") && weaponCooldown <= 0)
        {
            Shoot(null, new List<string> { "Federation", "Neutral", "Pirate" });
            weaponCDs[stats.currentWeapon] = 1 / stats.weapons[stats.currentWeapon].fireRate;
        }

        if (Input.GetButtonDown("Weapon1"))
        {
            SwitchWeapons(0);
        }
        else if (Input.GetButtonDown("Weapon2"))
        {
            SwitchWeapons(1);
        }
        else if (Input.GetButtonDown("Weapon3"))
        {
            SwitchWeapons(2);
        }
        else if (Input.GetButtonDown("Weapon4"))
        {
            SwitchWeapons(3);
        }
        else if (Input.GetButtonDown("Weapon5"))
        {
            SwitchWeapons(4);
        }

        if(Input.GetButtonDown("ToggleUI"))
        {
            pStats.ToggleUI();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void SwitchWeapons(int slot)
    {
        Debug.Log("switching weapon");
        shipWeapon = stats.GetWeapon(slot);
        stats.SetCurrentWeapon(slot);
    }

    protected override void DoUpdateChecks()
    {
        if (isDead && !audioSource.isPlaying)
        {
            Die();
        }
        else if (!isDead)
        {
            for(int i = 0; i < weaponCDs.Count; i++)
            {
                weaponCDs[i] -= Time.deltaTime;
            }

            weaponCooldown = weaponCDs[stats.currentWeapon];

            lastDamaged += Time.deltaTime;
            if (lastDamaged >= shieldRegenTime && shield < maxShield) //regenerate shields if damage has not been taken in the last 5 seconds
            {
                chargingShield = true;
                shield += shieldRegenRate * Time.deltaTime;
                if (shield > maxShield)
                {
                    shield = maxShield;
                    lastDamaged = 0;
                }
                UpdateBars();
            }
            else chargingShield = false;
        }
    }

    void HandleMusic()
    {
        if(inCombat && lastDamaged > 30)
        {
            inCombat = false;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetCombatState(inCombat);
        }
    }

    void EnterCombat()
    {
        inCombat = true;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetCombatState(inCombat);
    }

    void HandleMovement()
    {
        if (isDead)
            return;
        //player movement stuff
        if(Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
        {
            if (Input.GetAxisRaw("Vertical") > 0 && thrusterPower < 0) //If the player was going backwards and now wants to go forwards, don't make them have to wait for double the time
                thrusterPower = 0;
            thrusterPower = thrusterPower + Input.GetAxisRaw("Vertical") * acceleration * Time.deltaTime;
            if (thrusterPower < -1)
                thrusterPower = -1;
            if (thrusterPower > 1)
                thrusterPower = 1;
        }
        else
        {
            if (thrusterPower < 0)
                thrusterPower = thrusterPower + 1 * Time.deltaTime > 0 ? 0 : thrusterPower + 1 * Time.deltaTime;
            else if (thrusterPower > 0)
                thrusterPower = thrusterPower - 1 * Time.deltaTime < 0 ? 0 : thrusterPower - 1 * Time.deltaTime;
        }

        float verticalMove = thrusterPower * speed * (speedConst * shipRb.mass) * Time.deltaTime;
        verticalMove = (verticalMove < 0) ? verticalMove * reversePenalty : verticalMove;
        float horizontalMove = Input.GetAxis("Horizontal") * speed * (speedConst * shipRb.mass) * strafePenalty * Time.deltaTime;

        float rotationMove = 0;

        if(mouseMovement && Input.GetAxis("Rotation") == 0) //mouse rotate
        {
            float mouseRotation = Input.GetAxis("Mouse X");

            //These lines clamp the rotation speed. Moving your mouse faster won't make it rotate faster.
            if (mouseRotation > 1)
                mouseRotation = 1;
            else if (mouseRotation < -1)
                mouseRotation = -1;

            rotationMove += -1 * mouseRotation * TS * gameManager.GetMouseSensitivity() * Time.deltaTime;
        }
        else
        {
            rotationMove += Input.GetAxis("Rotation") * TS * Time.deltaTime;
        }


        if (rotationMove + verticalMove + horizontalMove != 0f) //if the player is moving in a certain direction or rotating
        {
            shipRb.angularVelocity = 0f; //set angular velocity to zero, which stops the player from rotating due to outside forces
        }

        HandleAnimation(Mathf.Abs(verticalMove));

        Vector2 force = new Vector2(horizontalMove, verticalMove);
        shipRb.MoveRotation(shipRb.rotation + rotationMove);
        shipRb.AddForce(transform.TransformDirection(force));
        if (Input.GetButton("Brake")) //if the player is braking
        {
            TS = defaultTS / 2f;
            turnSpeed = defaultTurnSpeed / 2f;
            shipRb.drag = defaultDrag * 5; //up the drag, which causes the player to stop faster
            shipRb.angularVelocity = 0f;
        }
        else
        {
            TS = defaultTS;
            turnSpeed = defaultTurnSpeed;
            shipRb.drag = defaultDrag;
        }
    }

    void HandleAnimation(float currentSpeed)
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            shipAnimator.SetBool("IsAccelerating", true);
        }
        else
        {
            shipAnimator.SetBool("IsAccelerating", false);
        }


        shipAnimator.SetFloat("ThrusterPower", thrusterPower);

        if (shield >= 1)
        {
            shieldAnim.SetBool("hasShield", true);
        }
        else if (shield < 1)
        {
            shieldAnim.SetBool("hasShield", false);
        }

        //shield fades in when regenerating, and fades out when stopped.
        if (chargingShield)
        {
            alphaLevel = (alphaLevel <= maxAlphaLevel) ? (alphaLevel + fadeInSpeed) : maxAlphaLevel;
            shieldAnim.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, alphaLevel);
        }
        if (!chargingShield)
        {
            alphaLevel = (alphaLevel >= minAlphaLevel) ? (alphaLevel - fadeOutSpeed) : minAlphaLevel;
            shieldAnim.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alphaLevel);
        }
    }

    public void SetSpawn(Transform point)
    {
        spawnPoint = point;
    }

    //since we don't want the player to be destroyed on death, we should override the default Die()
    protected override void Die()
    {
        //eventually will do more stuff here
        //StartCoroutine("Respawn") For some reason this causes the player to get stuck.
        //RespawnFunc();
    }

    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        shieldAnim.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetDelay(int weapon, float amount)
    {
        if(weaponCDs[weapon] < amount)
            weaponCDs[weapon] = amount;
    }

    public void SpawnAtPoint(Vector3 point)
    {
        RespawnFunc();
        transform.position = point;
    }

    public void RespawnFunc()
    {
        Debug.Log("calling respawnFunc");
        shipCollider.enabled = true;
        spriteRenderer.enabled = true;
        shipRb.velocity = Vector2.zero;
        shipRb.angularVelocity = 0;
        shield = maxShield;
        health = maxHealth;
        UpdateBars();
        shieldAnim.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        shieldAnim.SetBool("hasShield", true);
        isDead = false;
        respawning = false;
        deadTimer = 0;
    }

    public void ChangeBG(int backgroundNum)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Destroy(camera.transform.GetChild(0).gameObject);
        Instantiate(bgPrefabs[backgroundNum], camera.transform).transform.SetSiblingIndex(0);
    }

    public override void TakeDamage(float damage, ShipController damager, bool enterCombat = true)
    {
        if (isDead)
            return;

        lastDamaged = 0f;
        if(!inCombat && enterCombat)
            EnterCombat();
        if (shield >= damage)
        {
            playerShield.GetComponent<ShieldOnHit>().onHit();
            shield -= damage;
        }
        else
        {
            playerShield.GetComponent<ShieldOnHit>().onHit();
            damage -= shield;
            shield = 0;
            health -= damage;
        }

        if (health <= 0)
        {
            health = 0;
            if(damager != null)
            {
                try
                {
                    damager.GetStats().AlterReputation(stats.reputation, true);
                    damager.RemoveDeadTarget(this);
                }
                catch
                {

                }
                Debug.LogWarning("Could not remove from aggroTable");
            }
            PrepareForDeath();
        }
        else
        {
            if(damager != null)
                damager.GetStats().AlterReputation(stats.reputation, false);
        }

        try
        {
            UpdateBars();
        }
        catch
        {
            Debug.LogWarning(gameObject.name + " does not have a health bar");
        }

    }

    public void KillSelf()
    {
        Destroy(gameObject);
    }

    public void ToggleMouseMovement()
    {
        mouseMovement = !mouseMovement;
        if(mouseMovement)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetCurrentShield()
    {
        return shield;
    }

    public void RepairHull()
    {
        health = maxHealth;
        UpdateBars();
    }

}
