using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;


//сделать более резкий jump
//улетает на трамплине при движении...  Мб нужен все таки драг? Но все еще хз как работает... Сошлись на том что разберем в пн/вт
    public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float RotationSpeed;
    [SerializeField]  private Transform orientation;
    //delete this after a test
    [SerializeField]  private Transform _dummy;
    private Vector3 _velocity;
    //private bool _isMoving;
    private Vector3 _moveDirection;
    [SerializeField] private AnimationCurve SpeedUpCurve;
    [SerializeField] private float SpeedUpTimer = 0;
    [SerializeField] private float MaxAirAttacks = 3;
     private bool CanAirAttack = true;
     private float AirAttacks;
     
     [Header("Gravity")]
     [SerializeField] private float Gravirty;
     private float StartingGravirty;


    [Header("Jump")]
    [SerializeField] private float JumpForce;
    [SerializeField] private float FallingMultiplier;
    [SerializeField] private float FlightDuration;
    [SerializeField] private float XAirSlowDown;
    [SerializeField] private float ZAirSlowDown;
    private Vector3 _FallingVelocity;
    private bool IsJumpAttack;
    private Vector3 LastPosition;
    private bool Falling;

    [Header("Attack")]
    [SerializeField] private float TimeInTheAir;
    [SerializeField] private Transform pfSpiritWolf;
    [SerializeField] private Transform SpawnWolfPosition;
    
    [Header("Keybinds")] 
    [SerializeField] private KeyCode JumpKey = KeyCode.Space;
    [SerializeField] private KeyCode FirstSkill = KeyCode.F1;
    
    [Header("Ground Check")] 
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;
    //private bool _ReadyToJump;
    Rigidbody rb;

    private void Start()
    {
        StartingGravirty = Gravirty;
        //_isMoving = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
       // _ReadyToJump = true;
        AirAttacks = MaxAirAttacks;
        LastPosition = transform.position;
        Falling = false;
    }

    private void Update()
    {
        MyInput(); 
        CheckFalling();
        SummonAWolf();
        var dist = Vector3.Distance(_dummy.position, transform.position);
        Debug.Log(dist);
    }


    private void FixedUpdate()
{
       //IsMoving();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MovePlayer();
        PlayerGravity();
}

    //Move player by wasd + cameras direction. Jump with a space button
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput == 0 && verticalInput == 0)
        {
            SpeedUpTimer = 0;
        }
        else
        {
            SpeedUpTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(JumpKey) && grounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Should be a default attack but now I`ve got only an Air Attack 
            JumpAttack();
        }
       
        
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //If character is on the ground and used vertical or horizontal input, character will accelerate by time until he reaches max speed 
        _velocity = _moveDirection.normalized * (MaxSpeed * SpeedUpCurve.Evaluate(SpeedUpTimer));
        _velocity.y = rb.velocity.y;
        //freeze character movement while in AirAttack
        if (IsJumpAttack)
            {
                _velocity.y = 0;
                _velocity.x = 0;
                _velocity.z = 0;
            }
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //rotate character
        if (inputDirection != Vector3.zero)
        {
            var rotationVelocity = new Vector3(_velocity.x, 0, _velocity.z);
            transform.forward = Vector3.Slerp(transform.forward, rotationVelocity,Time.deltaTime * RotationSpeed);
        }
        if (grounded)
        {
                _velocity = _moveDirection.normalized * (MaxSpeed * SpeedUpCurve.Evaluate(SpeedUpTimer));
                IsJumpAttack = false;
                FlightDuration = 0;
                CanAirAttack = true;
                MaxAirAttacks = AirAttacks;
        }

        //Slowdown the x,y speed at the air
        if (!grounded)
        {
            _velocity.x /= XAirSlowDown;
            _velocity.z /= ZAirSlowDown;
        }
        
        //Increase the falling speed after air attack
        if (!grounded && Falling && MaxAirAttacks < AirAttacks)
        {
            _velocity.y *= FallingMultiplier/10f;
        }
        
        rb.velocity = _velocity;

    }
    
  //  private void IsMoving()
   // {
    //    if (horizontalInput != 0 || verticalInput != 0)
    //    {
    //        _isMoving = true;
     //   }
     //   else
      //  {
      //      _isMoving = false;
     //   }
            
   // }

   
    private void Jump()
    {
        rb.AddForce(Vector3.up * (JumpForce * 10f));
    }

    private void JumpAttack()
    {
        if (CanAirAttack && !grounded)
        {
            IsJumpAttack = true;
            StartCoroutine(DelayedSlowDown());

        }
    }

    private IEnumerator DelayedSlowDown()
    {
        if (Input.GetMouseButtonDown(0) && MaxAirAttacks > 0)
        {
            MaxAirAttacks -= 1f;
            FlightDuration += TimeInTheAir;
            Gravirty = 0;
        }

        while (FlightDuration > 0)
        {
            FlightDuration -= (1f * Time.deltaTime);
            yield return null;
        }

        if (MaxAirAttacks <= 0)
        {
            CanAirAttack = false;
        }
        yield return new WaitForSeconds(0);
        Gravirty = StartingGravirty;
        IsJumpAttack = false;
    }


    private void CheckFalling()
    {   
       var currentPosition = transform.position;

        if (currentPosition.y < LastPosition.y && !grounded)
        {
            if (IsJumpAttack)
            {
                Falling = false;
            }
            else
            {
                Falling = true;
            }
        }
        else
        {
            Falling = false;
            
        }
        LastPosition = transform.position;

    }


    private void SummonAWolf()
    {
        if (Input.GetKeyDown(FirstSkill)) //ToDO добавить условие что кд <= 0
        {
            Vector3 inputDirection = orientation.forward;
            Vector3 aimDir = (inputDirection - SpawnWolfPosition.position).normalized;
            Instantiate(pfSpiritWolf, SpawnWolfPosition.position, Quaternion.LookRotation(aimDir, Vector3.zero));
            
        }
    }
    
    private void PlayerGravity()
    {
        rb.AddForce(Vector3.down * Gravirty * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}
