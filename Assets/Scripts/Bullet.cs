using UnityEngine;

public class Bullet : MonoBehaviour{
    [SerializeField] private float bulletSpeed = 20f;
    private Rigidbody2D _myRigidbody;
    private PlayerMovement _player;
    private float _xSpeed;

    private void Start(){
        _myRigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();
        _xSpeed = _player.transform.localScale.x * bulletSpeed;
    }

    private void Update(){
        _myRigidbody.velocity = new Vector2(_xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other != null && other.CompareTag("Enemy")){
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other){
        Destroy(gameObject);
    }
}