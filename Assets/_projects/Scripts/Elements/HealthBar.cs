using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform fillBarPatent;

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void SetHealthBar(float ratio)
    {
        fillBarPatent.localScale = new Vector3(ratio, 1, 1);
        if (ratio >= 1)
        {
            gameObject.SetActive(false);
        }
        else if (ratio <= 0)
        {
            gameObject.SetActive(false);
        } 
        else
        {
            gameObject.SetActive(true);
        }

    }
}
