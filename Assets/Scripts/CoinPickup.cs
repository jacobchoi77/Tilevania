using UnityEngine;

public class CoinPickup : MonoBehaviour{
    [SerializeField] private AudioClip coinPickupSfx;
    [SerializeField] private int pointsForCoinPickup = 100;

    private bool _wasCollected;

    private void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player") || _wasCollected) return;
        _wasCollected = true;
        FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
        if (Camera.main != null) AudioSource.PlayClipAtPoint(coinPickupSfx, Camera.main.transform.position);
        GameObject o;
        (o = gameObject).SetActive(false);
        Destroy(o);
    }
}