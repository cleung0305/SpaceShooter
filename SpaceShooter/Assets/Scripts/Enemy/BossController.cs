using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : NpcController
{
    [SerializeField] protected List<TurretController> turrets = new List<TurretController>();
    [SerializeField] protected BossHitZone hitZone;

    public bool canShoot = false;
    [SerializeField] float internalClock = 0f;
    [SerializeField] protected float nextActionTime;

    [SerializeField] Vector2 moveTime = new Vector2(2,5);
    [SerializeField] Vector2 rotateTime = new Vector2(3,10);

    [SerializeField] protected float actionDuration;
    [SerializeField] protected float lastActionTime = 0f;
    [SerializeField] protected float waitTime = 5f;
    [SerializeField] protected bool canPerformAction = true;
    [SerializeField] protected bool isWaiting = false;

    [SerializeField] protected float moveRatio = .25f;
    [SerializeField] protected float rotateRatio = .25f;
    [SerializeField] protected float outOfRangeRatio = 3f;
    //[SerializeField] protected float moveTurnSpeed = .25f;
    //[SerializeField] protected float rotateTurnSpeed = .25f;
    //[SerializeField] protected float outOfRangeTurnSpeed = 2.5f;

    [SerializeField] GameObject hyperDrivePiece;
    //intervalClock keeps track of the time of the boss.
    //moveTime is how long the ShipController gets to move for
    //rotateTime is how long the ShipController gets to rotate for
    //actionDuration is the duration of whatever action was done (move or rotate)
    //lastActionTime is the time that the last action was completed. This + actionDuration is the time when the ShipController has to "cooldown"
    //nextActionTime is the time when the next action can be started. This has to be set
    bool canMove, canRotate;

    private void Awake()
    {
        base.Awake();
    }

    public override void Initialize(ShipStats newStats)
    {
        base.Initialize(newStats);
        aggroTable.Initialize(this);
        UpdateBars();
        GameObject turretContainer = transform.Find("Turrets").gameObject;

        /*foreach (Transform turret in turretContainer.transform)
        {
            TurretController turretController = turret.GetComponentInChildren<TurretController>();
            turretController.SetBoss(this);
            turrets.Add(turretController);
        }*/

        aggroTable.AddShip(GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>(), 100);
        aggroElements = aggroTable.GetElements();

        //moveTurnSpeed = moveRatio * defaultTurnSpeed;
        //rotateTurnSpeed = rotateRatio * defaultTurnSpeed;
        //outOfRangeTurnSpeed = outOfRangeRatio * defaultTurnSpeed;
    }

    protected override void Update()
    {
        
    }

    protected override void Move()
    {
        Rotate();
        float distance = Vector2.Distance(currentTarget.transform.position, transform.position);
        if (!TurretsInRange() && !isWaiting) //if the target is further away than half of the weapons range
        {
            if (Mathf.Abs(lastDistanceToTarget - distance) < 5f && distance < 5f)
            {
                thrusterPower -= acceleration * 2 * Time.deltaTime;
                if (thrusterPower < 0)
                    thrusterPower = 0;
            }
            else
            {
                thrusterPower += acceleration * Time.deltaTime;
                if (thrusterPower > 1)
                    thrusterPower = 1;
            }
        }
        else
        {
            thrusterPower -= acceleration * 2 * Time.deltaTime;
            if (thrusterPower < 0)
                thrusterPower = 0;
        }
        lastDistanceToTarget = distance;
        shipRb.AddForce((transform.up * speed * speedConst * shipRb.mass) * Time.deltaTime * thrusterPower);
    }

    protected override void FixedUpdate()
    {
        DoUpdateChecks();
        if (shield <= 0)
        {
            gameObject.layer = 0;
        }

        if (currentTarget == null)
            LookForTargets();
        else
            targetPosition = currentTarget.transform.position;
        internalClock += Time.deltaTime;
        if (hitZone.ShipsInZone().Count > 0 && weaponCooldown <= 0)
        {
            StallTime(5, true);
            Shoot(currentTarget.gameObject, enemyFactions);
            return; //Don't do anything else if the boss is shooting
        }


        if (canPerformAction) //if it is time to perform an action
        {
            lastActionTime = internalClock;
            PerformAction();
        }
        if (internalClock >= lastActionTime + actionDuration) //if it is time to wait after performing an action
        {
            canMove = false;
            canRotate = false;
            isWaiting = true;
        }
        if (isWaiting)
        {
            if (internalClock >= lastActionTime + actionDuration + waitTime)
            {
                isWaiting = false;
                canPerformAction = true;
            }
        }
        else
        {
            if (!TurretsInRange() && canMove)
            {
                turnSpeed = outOfRangeRatio * defaultTurnSpeed;
                Move();
            }
            else if (canMove)
            {
                turnSpeed = moveRatio * defaultTurnSpeed;
                Move();
            }
            else if (canRotate)
            {
                turnSpeed = rotateRatio * defaultTurnSpeed;
                Rotate();
            }
        }
        nextActionTime = lastActionTime + actionDuration + waitTime;
    }

    protected override bool TargetInAttackRange()
    {
        return TurretsInRange();
    }

    void PerformAction()
    {
        bool turretInRange = TurretsInRange();

        int choice = turretInRange ? Random.Range(0, 2) : 1;
        if(choice == 0)
        {
            canRotate = true;
            canMove = false;
            actionDuration = Random.Range(rotateTime.x, rotateTime.y);
            waitTime = 7;
        }
        else if (choice == 1)
        {
            canMove = true;
            canRotate = true;
            actionDuration = Random.Range(moveTime.x, moveTime.y);
            waitTime = 5;
        }

        if(!turretInRange)
        {
            waitTime = 0;
        }

        canPerformAction = false;
    }

    protected void StallTime(float time, bool immediateStop = false)
    {
        if (immediateStop)
            shipRb.velocity = Vector2.zero;
        canMove = false;
        canRotate = false;
        isWaiting = true;
        waitTime = time;
        nextActionTime += time;
    }

    protected virtual bool TurretsInRange()
    {
        foreach (TurretController turret in turrets)
        {
            if (turret.CanHitTarget())
            {
                return true;
            }
        }
        return false;
    }

    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    protected override void Die()
    {
        Instantiate(hyperDrivePiece, transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

    public void RemoveTurret(TurretController turret)
    {
        turrets.Remove(turret);
    }

    public bool IsMoving()
    {
        return canMove || canRotate;
    }
}
