using UnityEngine;

public class EnemyMovement : MonoBehaviour{
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D _rigidbody;

    private void Start(){
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        _rigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other){
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    private void FlipEnemyFacing(){
        transform.localScale = new Vector2(-(Mathf.Sign(_rigidbody.velocity.x)), 1f);
    }
}