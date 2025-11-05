using UnityEngine;

public class CharacterEjection : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] private TrailRenderer ejectionTrail;
    
    [Header("References")]
    private Rigidbody _rigidBody;
    
    private CharacterMover _characterMover;
    private bool _wasKinematic;
    private bool _isEjected;

    private void Awake()
    {
        if (_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        
        _characterMover = GetComponent<CharacterMover>();
        
        if (ejectionTrail != null)
        {
            ejectionTrail.enabled = false;
        }
    }

    public void Eject(Vector3 direction, float force, float upward)
    {
        if (_isEjected) return;
        
        _isEjected = true;
        
        if (_characterMover != null)
        {
            _characterMover.enabled = false;
        }
        
        if (_rigidBody != null)
        {
            _wasKinematic = _rigidBody.isKinematic;
            _rigidBody.isKinematic = false;
            
            Vector3 ejectionDirection = direction.normalized;
            ejectionDirection.y = 0;
            
            Vector3 forceVector = ejectionDirection * force + Vector3.up * upward;
            _rigidBody.AddForce(forceVector, ForceMode.Impulse);
        }
        
        if (ejectionTrail != null)
        {
            ejectionTrail.enabled = true;
        }
    }

    public void ResetEjection()
    {
        _isEjected = false;
        
        if (_rigidBody != null)
        {
            _rigidBody.isKinematic = _wasKinematic;
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }
        
        if (ejectionTrail != null)
        {
            ejectionTrail.enabled = false;
            ejectionTrail.Clear();
        }
        
        if (_characterMover != null)
        {
            _characterMover.enabled = true;
        }
    }

    public bool IsEjected => _isEjected;
}

