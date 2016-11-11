using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// The class for FloatyAI contains functionality unique to floaty.
/// </summary>
public class FloatyAI : EnemyAI
{

    private bool attacking = false;
    private bool bouncing = false;

    private Quaternion groundCheckLocationRotation;
    private float targetHeightFromPlayer = 9f;
    private float verticalMoveSpeedBoost = 2f;

    public int knockBack;
    public int stun;

    private float bounceForce = 300f;

    private RectTransform enemyCanvas;

    private Vector3 canvasOffset;

    public AudioClip floatyAttackNoise;

    /// <summary>
    /// Runs when the gameobject starts. Initializes key variables.
    /// </summary>
    new void Start()
    {
        base.Start();
        this.enemyCanvas = this.GetComponentInChildren<RectTransform>();
        this.canvasOffset = this.enemyCanvas.transform.position - this.transform.position;
        this.longestRange = 20;
        this.groundCheckLocationRotation = this.groundCheck.rotation;
        base.fears = "Player";
        this.target = PreserveAcrossLevels.playerInstance.gameObject.transform;
    }

    /// <summary>
    /// Runs each FixedUpdate. Makes the floaty start acting if the player is close enough.
    /// </summary>
    new void FixedUpdate()
    {
        //check if player is in range
        if (Vector3.Distance(transform.position, target.position) <= detectRange)
        {
            this.facePlayer(this.target);
            this.makeActionDecision();
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Runs after each frame is drawn. Updates the position of the GUI around the Floaty.
    /// </summary>
    void LateUpdate()
    {
        this.groundCheck.transform.rotation = this.groundCheckLocationRotation;
        this.enemyCanvas.position = this.transform.position + canvasOffset;
    }

    /// <summary>
    /// Makes the Floaty change what is plans to do based on how far away it is from the player.
    /// </summary>
    protected override void makeActionDecision()
    {
        //Decide on an action
        if (!attacking && !bouncing)
        {
            if (Vector3.Distance(transform.position, target.position) < longestRange && (Mathf.Abs(this.transform.position.y - (this.targetHeightFromPlayer + target.position.y)) < 0.3))
            {
                StartCoroutine(this.attackCoroutine(target.position));
            }
            else
            {
                this.pathToLocation(target.position);
            }
        }
    }

    /// <summary>
    /// Makes the floaty NPC look at the target.
    /// </summary>
    /// <param name="target">The target of the enemy. The player.</param>
    private void facePlayer(Transform target)
    {
        this.transform.LookAt(target);
    }

    #region Movement Functions

    protected void moveCharacter(float horizontalDirection, float verticalDirection)
    {
        this.recoveryTimer = 0;
        //Stun Timer
        if (recoveryTimer != 0)
        {
            horizontalDirection = 0;
            verticalDirection = 0;
        }
        else
        {
            rigidbody.velocity = new Vector3(horizontalDirection * movementSpeed, (verticalDirection * movementSpeed), 0);
        }
    }

    //This is the method used for moving the enemy from current location to a set destination - overrides from NPCBehaviour
    public override void pathToLocation(Vector3 destination)
    {
        float horizontalDirection = 0;
        float verticalDirection = 0;
        if (this.gameObject.transform.position.x > destination.x)
        {
            horizontalDirection = -1;
        }
        else
        {
            horizontalDirection = 1;
        }

        if (this.gameObject.transform.position.y > (destination.y + this.targetHeightFromPlayer))
        {
            verticalDirection = -1;
        }
        else if (this.gameObject.transform.position.y < (destination.y + this.targetHeightFromPlayer))
        {
            verticalDirection = 1;
        }

        this.moveCharacter(horizontalDirection, verticalDirection);
    }

    #endregion

    /// <summary>
    /// A coroutine that makes the floaty fly at the player in an attack.
    /// </summary>
    /// <param name="target">The transform of the target to attack.</param>
    /// <returns></returns>
    private IEnumerator attackCoroutine(Vector3 target)
    {
        AudioSource.PlayClipAtPoint(this.floatyAttackNoise, this.transform.position, 2f);
        this.attacking = true;
        float attackStartTime = Time.time;
        Vector3 targetPosition;
        if (this.transform.position.x < target.x)
        {
            targetPosition = new Vector3(target.x, target.y + 1, 0);
        }
        else
        {
            targetPosition = new Vector3(target.x, target.y + 1, 0);
        }
        Vector3 newVelocity = Vector3.zero;
        while ((attackStartTime + 2f > Time.time) && attacking && (this.transform.position != targetPosition))
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, (.5f / (1 / (Time.time - attackStartTime))));
            Vector3.SmoothDamp(this.transform.position, targetPosition, ref newVelocity, 1f);
            this.GetComponent<Rigidbody>().velocity = newVelocity;
            yield return null;
        }
        this.attacking = false;
    }

    /// <summary>
    /// Makes the floaty boucne away from the player and overrides its normal movement.
    /// </summary>
    /// <param name="other">The object the floaty is bouncing away from.</param>
    /// <returns></returns>
    private IEnumerator bounceCoroutine(Transform other)
    {
        this.bouncing = true;
        float bounceStartTime = Time.fixedTime;
        Vector3 direction = this.gameObject.transform.position - other.position;
        //this.GetComponent<Rigidbody>().AddForce(direction.normalized * bounceForce);
        while (bounceStartTime + .5f > Time.fixedTime && ((Mathf.Abs(this.transform.position.x - this.target.position.x)) < 5))
        {
            Vector3 velocity = Vector3.zero;
            Vector3.SmoothDamp(this.transform.position, direction.normalized * 50, ref velocity, 1f);
            this.GetComponent<Rigidbody>().velocity = velocity;
            yield return null;
        }
        this.bouncing = false;
    }

    /// <summary>
    /// If the floaty collides with the player start the bounce co routine.
    /// </summary>
    /// <param name="other">The other thing the floaty collided with.</param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("Player"))
        {
            this.attacking = false;
            StartCoroutine(this.bounceCoroutine(other.transform));
        }
    }

    /// <summary>
    /// Flashes the character model red over several frames when the ytake damage.
    /// </summary>
    /// <returns>Return null. Is a co-routine so returns at the end of each frame.</returns>
    protected override IEnumerator flashOnDamageTaken()
    {
        if (!flashing)
        {
            //Initializing colors.
            this.flashing = true;
            Transform bodyTransform = this.gameObject.transform.Find("Body");
            MeshRenderer body = bodyTransform.gameObject.GetComponent<MeshRenderer>();
            Material[] colorBackup = body.materials;
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    body.materials = new Material[] { this.damageMaterial, this.damageMaterial };
                }
                else if (i % 2 == 0)
                {
                    body.materials = new Material[] { colorBackup[0], colorBackup[1] };
                }
                else
                {
                    body.materials = new Material[] { this.damageMaterial, this.bodyMaterial };
                }
                yield return new WaitForSeconds(.15f);
            }
            body.materials = colorBackup;
            this.flashing = false;
        }
    }

 
} 