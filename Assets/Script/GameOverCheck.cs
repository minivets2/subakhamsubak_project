using UnityEngine;

public class GameOverCheck : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag($"Bread"))
        {
            if (col.GetComponent<Bread>().IsDrop)
            {
                col.GetComponent<Bread>().Explode(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag($"Bread"))
        {
            if (other.GetComponent<Bread>().IsDrop)
            {
                other.GetComponent<Bread>().Explode(false);
            }
        }
    }
}
