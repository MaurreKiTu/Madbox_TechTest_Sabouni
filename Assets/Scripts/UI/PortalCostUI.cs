using TMPro;
using UnityEngine;
using System.Collections;

public class PortalCostUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI costText;
    
    [Header("Portal Reference")]
    [SerializeField] private Portal portal;
    
    [Header("Text Settings")]
    [SerializeField] private string costFormat = "${0}";
    [SerializeField] private string freeText = "FREE";
    
    [Header("Flash Settings")]
    [SerializeField] private Color insufficientFundsColor = Color.red;
    [SerializeField] private Color paymentSuccessColor = Color.green;
    [SerializeField] private float flashDuration = 0.5f;
    
    private Color _originalColor;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        if (costText == null)
        {
            costText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (portal == null)
        {
            portal = GetComponentInParent<Portal>();
        }
        
        if (costText != null)
        {
            _originalColor = costText.color;
        }
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (costText == null || portal == null) return;
        
        int portalCost = portal.Cost;
        
        if (portalCost <= 0)
        {
            costText.text = freeText;
        }
        else
        {
            costText.text = string.Format(costFormat, portalCost);
        }
    }
    
    public void FlashRed()
    {
        Flash(insufficientFundsColor);
    }
    
    public void FlashGreen()
    {
        Flash(paymentSuccessColor);
    }
    
    private void Flash(Color color)
    {
        if (costText == null) return;
        
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }
        
        _flashCoroutine = StartCoroutine(FlashCoroutine(color));
    }
    
    private IEnumerator FlashCoroutine(Color flashColor)
    {
        costText.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        costText.color = _originalColor;
        _flashCoroutine = null;
    }
}






