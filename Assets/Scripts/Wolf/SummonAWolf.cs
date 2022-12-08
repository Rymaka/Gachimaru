using UnityEngine;

public class SummonAWolf : MonoBehaviour
{
    private Rigidbody wolfRigidbody;

    [Header("WolfMovement")] 
    
    [SerializeField] private float speed;

    private void Awake()
    {
        wolfRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        wolfRigidbody.velocity = transform.forward * speed;
    }
}
