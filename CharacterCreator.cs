using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.UI;
using System.IO;

public class CharacterCreator : MonoBehaviour {

    public DynamicCharacterAvatar avatar;

    private Dictionary<string, DnaSetter> dna;
    public Slider heightSlider;
    public Slider bellySlider;

    public List<string> hairModels = new List<string>();
    private int currentHair;

    public string MyCharacter;

    void OnEnable()
    {
        avatar.CharacterUpdated.AddListener(Updated);
        heightSlider.onValueChanged.AddListener(HeightSizeChange);
        bellySlider.onValueChanged.AddListener(BellySizeChange);

    }
    void OnDisable()
    {
        avatar.CharacterUpdated.RemoveListener(Updated);
        heightSlider.onValueChanged.RemoveListener(HeightSizeChange);
        bellySlider.onValueChanged.RemoveListener(BellySizeChange);
    }

    public void SwitchGender(bool male)
    {
        if(male && avatar.activeRace.name !="HumanMaleDCS")
        {
            avatar.ChangeRace("HumanMaleDCS");
        }
        if(!male && avatar.activeRace.name != "HumanFemaleDCS")
        {
            avatar.ChangeRace("HumanFemaleDCS");
        }
    }

    void Updated(UMAData data)
    {
        dna = avatar.GetDNA();
        heightSlider.value = dna["height"].Get();
        bellySlider.value = dna["belly"].Get();
    }

    public void HeightSizeChange(float val)
    {
        dna["height"].Set(val);
        avatar.BuildCharacter();
    }
    public void BellySizeChange(float val)
    {
        dna["belly"].Set(val);
        avatar.BuildCharacter();
    }

    public void ChangeSkinColor(Color col)
    {
        avatar.SetColor("Skin", col);
        avatar.UpdateColors(true);
    }

    public void ChangeHairColor(Color col)
    {
        avatar.SetColor("Hair", col);
        avatar.UpdateColors(true);
    }

    public void ChangeHair(bool plus)
    {
        if(plus)
        {
            currentHair++;
        }
        else
        {
            currentHair--;
        }

        currentHair = Mathf.Clamp(currentHair, 0, hairModels.Count - 1);

        if(hairModels[currentHair]=="None")
        {
            avatar.ClearSlot("Hair");
        }
        else
        {
            avatar.SetSlot("Hair", hairModels[currentHair]);
        }
        avatar.BuildCharacter();

       
    }

    public void SaveChar()
    {
        MyCharacter = avatar.GetCurrentRecipe();
        File.WriteAllText(Application.persistentDataPath+"/dude.txt", MyCharacter);
    }
    public void LoadChar()
    {
        MyCharacter = File.ReadAllText(Application.persistentDataPath + "/dude.txt");
        avatar.ClearSlots();
        avatar.LoadFromRecipeString(MyCharacter);
       
        
    }
}
