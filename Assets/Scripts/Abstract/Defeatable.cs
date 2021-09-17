using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Defeatable : MonoBehaviour
{
    public float Health { get; set; } = 100;
    public float DamageMultiplier { get; set; } = 1;

    private bool haveAnimation = false;
    private bool isDefaultAnimationRuning = false;
    private float defaultAnimationTime = 0.0f;
    private MeshRenderer meshRenderer;
    private Color firstMaterialDefaultColor;
    private PopupManager pm;

    private const float defaultAnimationTimeMax = 0.5f;

    public virtual float GetDamage(float damage = 0, Transform transformForPopupPosition = null, Transform transformForLookAt = null)
    {
        //animation
        if (!haveAnimation)
        {
            StartDefaultAnimation();
        }

        //counting
        var gottenDamage = damage * DamageMultiplier;
        Health -= gottenDamage;

        //showing popup
        if (transformForPopupPosition != null && transformForLookAt != null)
            pm.ShowDamagePopup(gottenDamage, transformForPopupPosition, transformForLookAt);

        if (Health <= 0)
            Defeat();

        return gottenDamage;
    }

    public virtual void Defeat()
    {
        Destroy(gameObject);
    }

    private void StartDefaultAnimation()
    {
        isDefaultAnimationRuning = true;
    }

    private void RunDefaultAnimation()
    {
        defaultAnimationTime += Time.deltaTime;
        if (meshRenderer.material.color != Color.white)
        {
            meshRenderer.material.color = Color.white;
        }
        else
        {
            meshRenderer.material.color = firstMaterialDefaultColor;
        }

        if (defaultAnimationTime >= defaultAnimationTimeMax)
        {
            isDefaultAnimationRuning = false;
            defaultAnimationTime = 0.0f;
            meshRenderer.material.color = firstMaterialDefaultColor;
        }
    }

    public void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        firstMaterialDefaultColor = meshRenderer.material.color;
        pm = GameObject.Find("PopupManager").GetComponent<PopupManager>();
    }

    public void Update()
    {
        if (isDefaultAnimationRuning)
        {
            RunDefaultAnimation();
        }
    }
}
