using UnityEngine;

public class CoinFlyManager : MonoBehaviour
{
    private static CoinFlyManager _instance;
    public static CoinFlyManager Instance => _instance;

    [Header("Prefab")]
    [SerializeField] private GameObject coinFlyingPrefab;
    
    [Header("Target")]
    [SerializeField] private RectTransform currencyUITarget;
    
    [Header("Canvas")]
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }

    public static void SpawnFlyingCoin(Vector3 worldPosition, int amount)
    {
        if (Instance == null || Instance.coinFlyingPrefab == null || Instance.currencyUITarget == null)
        {
            return;
        }

        Transform parent = Instance.canvas != null ? Instance.canvas.transform : Instance.transform;
        GameObject coinObj = Instantiate(Instance.coinFlyingPrefab, parent);
        
        CoinFlyingUI coinUI = coinObj.GetComponent<CoinFlyingUI>();
        
        if (coinUI != null)
        {
            coinUI.FlyToTarget(worldPosition, Instance.currencyUITarget.position, amount);
        }
    }
}

