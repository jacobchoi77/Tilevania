using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;

    private Vector2 _moveInput;
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;
    private CapsuleCollider2D _myBodyCollider;
    private BoxCollider2D _myFeetCollider;
    private float _gravityScaleAtStart;
    private bool _isAlive = true;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
    private static readonly int Dying = Animator.StringToHash("Dying");

    private void Start(){
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _myBodyCollider = GetComponent<CapsuleCollider2D>();
        _myFeetCollider = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _myRigidbody.gravityScale;
    }

    private void Update(){
        if (!_isAlive){
            return;
        }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void OnFire(InputValue value){
        if (!_isAlive) return;

        Instantiate(bullet, gun.position, transform.rotation);
    }

    private void OnMove(InputValue value){
        if (!_isAlive) return;

        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value){
        if (!_isAlive) return;

        if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            return;
        }

        if (value.isPressed){
            _myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void Run(){
        var velocity = _myRigidbody.velocity;
        var playerVelocity = new Vector2(_moveInput.x * runSpeed, velocity.y);
        velocity = playerVelocity;
        _myRigidbody.velocity = velocity;

        var playerHasHorizontalSpeed = Mathf.Abs(velocity.x) > Mathf.Epsilon;
        _myAnimator.SetBool(IsRunning, playerHasHorizontalSpeed);
    }

    private void FlipSprite(){
        var playerHasHorizontalSpeed = Mathf.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed){
            transform.localScale = new Vector2(Mathf.Sign(_myRigidbody.velocity.x), 1f);
        }
    }

    private void ClimbLadder(){
        if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            _myRigidbody.gravityScale = _gravityScaleAtStart;
            _myAnimator.SetBool(IsClimbing, false);
            return;
        }

        var climbVelocity = new Vector2(_myRigidbody.velocity.x, _moveInput.y * climbSpeed);
        _myRigidbody.velocity = climbVelocity;
        _myRigidbody.gravityScale = 0f;

        var playerHasVerticalSpeed = Mathf.Abs(_myRigidbody.velocity.y) > Mathf.Epsilon;
        _myAnimator.SetBool(IsClimbing, playerHasVerticalSpeed);
    }

    private void Die(){
        if (!_myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) return;
        _isAlive = false;
        _myAnimator.SetTrigger(Dying);
        _myRigidbody.velocity = deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}