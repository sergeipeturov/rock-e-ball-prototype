using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMesh txtDamage;
    private float currentPopupTime = 0.0f;
    private float damageFloat;

    private const float maxPopupTime = 0.5f;

    public void SetDamageForText(float damage)
    {
        damageFloat = damage;
        txtDamage.text = damageFloat.ToString();
    }

    private void Awake()
    {
        txtDamage = GetComponent<TextMesh>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 5f, 0) * Time.deltaTime);
        currentPopupTime += Time.deltaTime;
        if (currentPopupTime > maxPopupTime)
        {
            Destroy(gameObject);
        }
    }
}
