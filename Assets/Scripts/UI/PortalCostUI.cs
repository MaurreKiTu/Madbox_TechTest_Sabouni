using TMPro;
using UnityEngine;

public class PortalCostUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI costText;
    
    [Header("Portal Reference")]
    [SerializeField] private Portal portal;
    
    [Header("Text Settings")]
    [SerializeField] private string costFormat = "${0}";
    [SerializeField] private string freeText = "FREE";

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
}





