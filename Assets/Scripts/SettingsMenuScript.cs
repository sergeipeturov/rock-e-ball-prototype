using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenuScript : MonoBehaviour
{
    public TextMeshProUGUI txtHeader;
    public GameObject MouseSensitivityX;
    public GameObject MouseSensitivityY;
    public GameObject Aim;

    public float MouseSensitivityXValue { get; set; }
    public float MouseSensitivityYValue { get; set; }
    public bool ShowAim { get { return cbAim != null ? cbAim.isOn : false; } }

    private TextMeshProUGUI txtMouseSensitivityX;
    private Slider slidMouseSensitivityX;
    private TMP_InputField editMouseSensitivityX;
    private TextMeshProUGUI txtMouseSensitivityY;
    private Slider slidMouseSensitivityY;
    private TMP_InputField editMouseSensitivityY;
    private Toggle cbAim;
    private TextMeshProUGUI txtAim;

    public delegate void SettingsMenuShow();
    public event SettingsMenuShow SettingsMenuShowNotify;
    public event SettingsMenuShow SettingsApplyNotify;

    public void OnOkClick()
    {
        SettingsApplyNotify?.Invoke();
    }

    public void OnCancelClick()
    {
        SettingsMenuShowNotify?.Invoke();
    }

    private void OnSliderXValueChanged()
    {
        MouseSensitivityXValue = slidMouseSensitivityX.value;
        editMouseSensitivityX.text = MouseSensitivityXValue.ToString();
    }

    private void OnSliderYValueChanged()
    {
        MouseSensitivityYValue = slidMouseSensitivityY.value;
        editMouseSensitivityY.text = MouseSensitivityYValue.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        txtHeader.text = "Settings";
        txtMouseSensitivityX = MouseSensitivityX.transform.Find("MouseSensitivityXText").gameObject.GetComponent<TextMeshProUGUI>();
        slidMouseSensitivityX = MouseSensitivityX.transform.Find("MouseSensitivityXSlider").gameObject.GetComponent<Slider>();
        slidMouseSensitivityX.onValueChanged.AddListener(delegate { OnSliderXValueChanged(); });
        editMouseSensitivityX = MouseSensitivityX.transform.Find("MouseSensitivityXEdit").gameObject.GetComponent<TMP_InputField>();
        txtMouseSensitivityY = MouseSensitivityY.transform.Find("MouseSensitivityYText").gameObject.GetComponent<TextMeshProUGUI>();
        slidMouseSensitivityY = MouseSensitivityY.transform.Find("MouseSensitivityYSlider").gameObject.GetComponent<Slider>();
        slidMouseSensitivityY.onValueChanged.AddListener(delegate { OnSliderYValueChanged(); });
        editMouseSensitivityY = MouseSensitivityY.transform.Find("MouseSensitivityYEdit").gameObject.GetComponent<TMP_InputField>();
        cbAim = Aim.GetComponent<Toggle>();
        txtAim = Aim.transform.Find("Label").GetComponent<TextMeshProUGUI>();
        txtMouseSensitivityX.text = "Mouse Sensitivity X";
        txtMouseSensitivityY.text = "Mouse Sensitivity Y";
        txtAim.text = "Show Aim";
        OnSliderXValueChanged(); //calling to change text in input field
        OnSliderYValueChanged(); //calling to change text in input field
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
