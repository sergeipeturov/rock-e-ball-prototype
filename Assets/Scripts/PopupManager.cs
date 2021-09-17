using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject DamagePopup;

    public void ShowDamagePopup(float damage, Transform transformForPopupPosition, Transform transformToLookAt)
    {
        Vector3 popupPosition = new Vector3(transformForPopupPosition.position.x, transformForPopupPosition.position.y, transformForPopupPosition.position.z);
        var dp = Instantiate(DamagePopup, popupPosition, Quaternion.identity);
        dp.transform.LookAt(transformToLookAt);
        dp.transform.Rotate(new Vector3(0, 180, 0));
        dp.GetComponent<DamagePopup>().SetDamageForText(damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
