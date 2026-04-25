using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform fillBarPatent;
    public Transform fillWhiteBarPatent;
    public SpriteRenderer fillBarSpriteRenderer;
    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void SetHealthBar(float ratio)
    {

        fillBarPatent.transform.localScale = new Vector3(ratio, 1, 1);

        fillWhiteBarPatent.DOKill();
        fillWhiteBarPatent.DOScale(new Vector3(ratio, 1, 1), .2f).SetDelay(.1f);
        fillBarSpriteRenderer.DOKill();
       fillBarSpriteRenderer.color = Color.red;
        fillBarSpriteRenderer.DOColor(Color.yellow, .1f).SetLoops(2, LoopType.Yoyo);

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

    private void OnDestroy()
    {
        fillBarPatent.DOKill();
        fillBarSpriteRenderer.DOKill();
        fillWhiteBarPatent.DOKill();
    }
}
