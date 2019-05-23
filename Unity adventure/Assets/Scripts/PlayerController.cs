using UnityEngine;
using Assets.Repository;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //movement
    public float attackTime; //Hur länge en "attack" tid ska gälla
    public bool playerMoving;
    public bool canMove;
    public string startPoint;
    public string currentMap;
    public Vector2 lastMove;
    public Rigidbody2D myRigidBody;
    public GameObject swing;
    public GameObject wandPrefab; //För staven
    public GameObject blueStaffPrefab;

    //Private fields
    float attackTimeCounter;
    float grabbTime;
    float grabbTimeCounter;
    float currentMovementSpeed;
    static bool playerExists; //Måste vara static, annars skapar det flera canvas om man byter till en karta med en canvas redan
    bool grabbing;
    bool attacking;

    Animator anim;
    BackpackController backpack;
    Vector2 movementOnPlayer;
    Wand wandClass;

    void Start()
    {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        backpack = FindObjectOfType<BackpackController>();

        if (!playerExists && currentMap != "mainMenu")
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);

            var playerContent = Generator.PlayerData();
            moveSpeed = (float)playerContent.Speed;
            attackTime = 0.5f;
            playerMoving = false;
            startPoint = playerContent.SpawnPoint;
            currentMap = playerContent.CurrentMap;
            lastMove = new Vector2(0, 0);
            canMove = true;

            var weapon = FindObjectOfType<HurtEnemy>();
            weapon.damage = playerContent.Damage;

            backpack.GetInventory();
        }

        else
        {
            Destroy(gameObject);
        }

        lastMove = new Vector2(0, -1f);  //annars buggar svärdet när man startar spelet
        canMove = true;
    }

    void Update()
    {
        playerMoving = false;

        if (!canMove)
        {
            playerMoving = false;
            myRigidBody.velocity = Vector2.zero;
            return;
        }

        //om vi inte attackerar så är vi tillåtna att göra detta:
        if (!attacking || !grabbing)
        {
            //Hanterar så att även om man går diagonalt så går man lika snabbt ändå
            movementOnPlayer = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            //Om man går
            if (movementOnPlayer != Vector2.zero)
            {
                myRigidBody.velocity = new Vector2(movementOnPlayer.x * moveSpeed, movementOnPlayer.y * moveSpeed);
                playerMoving = true;
                lastMove = movementOnPlayer;
            }

            else
            {
                myRigidBody.velocity = Vector2.zero;
            }

            //Om användaren  trycker E eller Vänster Musknapp - sätter värdet i animatorn till true -så Unity vet hur gubben ska röra sig
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E))
            {
                var hasWand = backpack.HasRequestedItem("Staff");
                var hasBlueWand = backpack.HasRequestedItem("BlueWand");

                if (hasWand)
                {
                    Instantiate(wandPrefab, myRigidBody.transform.position, myRigidBody.transform.rotation);
                }

                else if (hasBlueWand)
                {
                    Instantiate(blueStaffPrefab, myRigidBody.transform.position, myRigidBody.transform.rotation);
                }

                //Om enbart svärd
                else
                {
                    attackTimeCounter = attackTime;

                    myRigidBody.velocity = Vector2.zero; //x och y är noll
                    attacking = true;
                    anim.SetBool("Attack", true);
                }
            }

            else if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Q))
            {
                grabbTimeCounter = 1;
                myRigidBody.velocity = Vector2.zero; //x och y är noll
                grabbing = true;
                anim.SetBool("Grabbing", true);
            }
        }

        if (attacking)
        {
            if (attackTimeCounter > 0)
            {
                attackTimeCounter -= Time.deltaTime;
            }

            if (attackTimeCounter <= 0)
            {
                attacking = false;
                anim.SetBool("Attack", false);
            }
        }

        else if (grabbing)
        {
            if (grabbTimeCounter > 0)
            {
                grabbTimeCounter -= Time.deltaTime;
            }

            if (grabbTimeCounter <= 0)
            {
                grabbing = false;
                anim.SetBool("Grabbing", false);
            }
        }

        //Tar värdet från GetAxisRaw för att styra gubben åt rätt hål
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);


        if (currentMap == "mainMenu")
        {
            Destroy(gameObject);
        }
    }
}