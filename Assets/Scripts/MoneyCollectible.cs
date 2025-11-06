using UnityEngine;

public class MoneyCollectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private int moneyValue = 10;
    [SerializeField] private bool destroyOnCollect = true;
    
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem collectFX;
    
    private bool _hasBeenCollected;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasBeenCollected) return;
        
        CharacterCurrency currency = other.GetComponent<CharacterCurrency>();
        if (currency != null)
        {
            OnCollect(currency);
        }
    }

    private void OnCollect(CharacterCurrency currency)
    {
        _hasBeenCollected = true;
        
        currency.AddCurrency(moneyValue);
        
        PlayCollectFX();
        
        if (destroyOnCollect)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void PlayCollectFX()
    {
        if (collectFX != null)
        {
            collectFX.transform.SetParent(null);
            collectFX.Play();
            Destroy(collectFX.gameObject, collectFX.main.duration);
        }
    }

    public void ResetCollectible()
    {
        _hasBeenCollected = false;
        gameObject.SetActive(true);
    }
}

