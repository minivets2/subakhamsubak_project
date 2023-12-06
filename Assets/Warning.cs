using System;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public delegate void WarningEvent(bool isWarning);
    public static WarningEvent warningEvent;
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag($"Bread"))
        {
            if (col.GetComponent<Bread>().IsDrop) warningEvent?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag($"Bread"))
        {
            if (other.GetComponent<Bread>().IsDrop) warningEvent?.Invoke(false);
        }
    }
}
