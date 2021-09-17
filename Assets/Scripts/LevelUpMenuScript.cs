using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class LevelUpMenuScript : MonoBehaviour
{
    public TextMeshProUGUI txtHeader;
    public TextMeshProUGUI txtDescription;
    public GameObject btBuy;
    public GameObject btChoose;

    private GameManager gm;
    private Player player;
    private List<Image> buttons = new List<Image>();
    private Feature SelectedFeature;

    private string FeaturesSpritePath { get; set; } = "Features/";
    private string ButtonsSpritePath { get; set; } = "Buttons/";
    private string ChosenPrfx { get; set; } = "chosen_";
    private string UnchosenPrfx { get; set; } = "unchosen_";
    private string UnboughtPrfx { get; set; } = "unbought_";
    private string UnavailPrfx { get; set; } = "unavail_";
    private string BlockedPrfx { get; set; } = "blocked";
    private string SpritePstfx { get; set; } = ".png";

    public void OnButtonClick(Button sender)
    {
        if (sender == btBuy.GetComponent<Button>())
        {
            gm.PlayerBuyFeature(SelectedFeature);
            OnShowed();
        }
        else if (sender == btChoose.GetComponent<Button>())
        {

        }
        else
        {
            SelectedFeature = gm.AllFeatures.FirstOrDefault(x => x.Name == sender.name);
            SetSpritesToButtons();
            SetTextToDescription();
        }
    }

    private void OnShowed()
    {
        SelectedFeature = null;
        SetSpritesToAllFeatures();
        SetSpritesToButtons();
        SetTextToDescription();
    }

    private void SetAvailableToFeature(Feature feature)
    {
        feature.IsAvailable = player.MediatorsCount >= feature.Cost;
        if (feature.IsAvailable && feature.NeededFeature != "")
        {
            feature.IsAvailable = player.HasFeature(feature.NeededFeature);
        }
    }

    private void SetBlockedToFeature(Feature feature)
    {
        feature.IsBlocked = gm.CurrentWorld < feature.NeededWorld;
    }

    private void SetSpritesToAllFeatures()
    {
        try
        {
            for (int i = 0; i < gm.AllFeatures.Count; i++)
            {
                SetAvailableToFeature(gm.AllFeatures[i]);
                SetBlockedToFeature(gm.AllFeatures[i]);

                if (gm.AllFeatures[i].IsBlocked)
                {
                    var img = buttons.FirstOrDefault(x => x.name == gm.AllFeatures[i].Name);
                    img.sprite = Resources.Load<Sprite>(FeaturesSpritePath + BlockedPrfx);
                }
                else if (player.HasFeature(gm.AllFeatures[i].Name))
                {
                    var ftr = player.Features.FirstOrDefault(x => x.Name == gm.AllFeatures[i].Name);
                    if (ftr.IsChosen)
                    {
                        var img = buttons.FirstOrDefault(x => x.name == gm.AllFeatures[i].Name);
                        img.sprite = Resources.Load<Sprite>(FeaturesSpritePath + ChosenPrfx + img.name);
                    }
                    else
                    {
                        var img = buttons.FirstOrDefault(x => x.name == gm.AllFeatures[i].Name);
                        img.sprite = Resources.Load<Sprite>(FeaturesSpritePath + UnchosenPrfx + img.name);
                    }
                }
                else if (gm.AllFeatures[i].IsAvailable)
                {
                    var img = buttons.FirstOrDefault(x => x.name == gm.AllFeatures[i].Name);
                    img.sprite = Resources.Load<Sprite>(FeaturesSpritePath + UnboughtPrfx + img.name);
                }
                else
                {
                    var img = buttons.FirstOrDefault(x => x.name == gm.AllFeatures[i].Name);
                    img.sprite = Resources.Load<Sprite>(FeaturesSpritePath + UnavailPrfx + img.name);
                }
            }
        }
        catch
        {

        }
    }

    private void SetSpritesToButtons()
    {
        if (SelectedFeature == null)
        {
            btBuy.SetActive(false);
            btChoose.SetActive(false);
        }
        else
        {
            btBuy.SetActive(true);
            btChoose.SetActive(false);

            if (player.HasFeature(SelectedFeature.Name))
            {
                if (SelectedFeature.CanBeChosen)
                {
                    btChoose.SetActive(true);
                }
                else
                {
                    btChoose.SetActive(false);
                }

                var ftr = player.Features.FirstOrDefault(x => x.Name == SelectedFeature.Name);
                var imgBuy = btBuy.GetComponent<Image>();
                var imgChoose = btChoose.GetComponent<Image>();
                imgBuy.sprite = Resources.Load<Sprite>(ButtonsSpritePath + "buy_button_inactive");
                if (ftr.IsChosen)
                    imgChoose.sprite = Resources.Load<Sprite>(ButtonsSpritePath + "chosen_button");
                else
                    imgChoose.sprite = Resources.Load<Sprite>(ButtonsSpritePath + "unchosen_button");
            }
            else
            {
                var imgBuy = btBuy.GetComponent<Image>();
                var imgButton = btBuy.GetComponent<Button>();
                if (SelectedFeature.IsBlocked)
                {
                    imgBuy.sprite = Resources.Load<Sprite>(ButtonsSpritePath + "buy_button_inactive");
                    imgButton.interactable = false;
                }
                else if (SelectedFeature.IsAvailable)
                {
                    imgBuy.sprite = Resources.Load<Sprite>(ButtonsSpritePath + "buy_button");
                    imgButton.interactable = true;
                }
                else
                {
                    imgBuy.sprite = Resources.Load<Sprite>(ButtonsSpritePath + "buy_button_inactive");
                    imgButton.interactable = false;
                }
            }
        }
    }

    private void SetTextToDescription()
    {
        if (SelectedFeature != null)
        {
            if (SelectedFeature.IsBlocked)
            {
                txtHeader.text = "unknown yet";
                txtDescription.text = "unknown yet";
            }
            else
            {
                txtHeader.text = SelectedFeature.LabelName;
                txtDescription.text = SelectedFeature.Description;
            }
        }
        else
        {
            txtHeader.text = "";
            txtDescription.text = "";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        gm.LevelUpMenuShowedNotify += OnShowed;
        player = gm.Player;

        buttons = GameObject.FindObjectsOfType<Image>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
