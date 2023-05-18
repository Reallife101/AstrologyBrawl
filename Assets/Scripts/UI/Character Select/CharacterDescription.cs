using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDescription : MonoBehaviour
{
    //Reference to TMP text objects for changing their text during the game
    public TMP_Text characterName;
    public TMP_Text characterTitle;
    public TMP_Text descriptionSection;

    //The text objects of the keybinds section. Assuming an order of Light, Heavy, Mixup, and Iconic
    public List<TMP_Text> abilitiesKeyBinds; 

    //Variables for controlling input
    private static PlayerControllerInputAsset input;
    private InputAction lightAttack;
    private InputAction heavyAttack;
    private InputAction mixup;
    private InputAction iconic;

    private Names currentName;

    [Serializable]
    public struct Description
    {
        public Names name;
        public Abilities ability;
        public string description;
    }

    [Serializable]
    public struct Title
    {
        public Names name;
        public string title;
    }

    //Abilities of each character    
    public enum Abilities
    { 
        LightAttack,
        HeavyAttack, 
        Mixup,
        Iconic,
        Desc
    }

    //Names of the characters
    public enum Names
    { 
        Aquarius,
        Scorpio,
        Aries,
        Taurus,
        Libra,
        Cancer,
        Leo,
        Pisces,
        Gemini,
        Capricorn,
        Sagittarius,
        Virgo    
    }

    //List of character Names.
    //  Could throw an error or show wrong name if this list doesn't match the list of characters in CharacterSelect
    public List<Names> characterNames;

    //This list is to be able to add the descriptions in the editor
    public List<Title> titles; 
    public List<Description> descriptions;
    
    //Dictionary that will hold the information for each character
    private Dictionary<Names, Dictionary<Abilities, string>> abilityDescription = new Dictionary<Names, Dictionary<Abilities, string>>();
    private Dictionary<Names, string> characterTitles = new Dictionary<Names, string>(); 

    private void Start()
    {
        //Setting up ability description dictionary 
        foreach (Description des in descriptions)
        {
            // If statement block for adding keys without throwing exceptions in case of duplicate keys
            if (!abilityDescription.ContainsKey(des.name))
                abilityDescription.Add(des.name, new Dictionary<Abilities, string> { { des.ability, des.description } });
            else 
            {
                if (!abilityDescription[des.name].ContainsKey(des.ability))
                    abilityDescription[des.name].Add(des.ability, des.description);
                else
                    abilityDescription[des.name][des.ability] = des.description;
            }
                
        }

        //Setting up character titles dictionary
        foreach (Title t in titles)
        {
            if (!characterTitles.ContainsKey(t.name))
                characterTitles.Add(t.name, t.title);
            else
                characterTitles[t.name] = t.title;
        }
 
        //Setting up the inputs 
        input = LobbyManager.input;
        lightAttack = input.UI.LightAttack;
        heavyAttack = input.UI.HeavyAttack;
        mixup = input.UI.Mixup;
        iconic = input.UI.Iconic;

        //Adding methods to the inputs
        lightAttack.started += GetDescription;
        heavyAttack.started += GetDescription;
        mixup.started += GetDescription;
        iconic.started += GetDescription;

        lightAttack.Enable();
        heavyAttack.Enable();
        mixup.Enable();
        iconic.Enable();
    }

    private void Update()
    {
        if (Input.GetJoystickNames().Length >= 1 && Input.GetJoystickNames()[0] != "")
            ChangeKeyBindsNames(false);
        else
            ChangeKeyBindsNames(true);
    }

    public void ChangeCharacter(int i)
    { 
        characterName.text = characterNames[i].ToString();
        currentName = characterNames[i];
        ShowTitle(currentName);
        ShowDescription(Abilities.Desc);
    }

    public void showLight()
    {
        ShowDescription(Abilities.LightAttack);
    }

    public void showHeavy()
    {
        ShowDescription(Abilities.HeavyAttack);
    }

    public void showMixup()
    {
        ShowDescription(Abilities.Mixup);
    }
    public void showIconic()
    {
        ShowDescription(Abilities.Iconic);
    }

    public void showDesc()
    {
        ShowDescription(Abilities.Desc);
    }

    private void GetDescription(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "LightAttack":
                ShowDescription(Abilities.LightAttack);
                break;
            case "HeavyAttack":
                ShowDescription(Abilities.HeavyAttack);
                break;
            case "Mixup":
                ShowDescription(Abilities.Mixup);
                break;
            case "Iconic":
                ShowDescription(Abilities.Iconic);
                break;
            case "Desc":
                ShowDescription(Abilities.Desc);
                break;
            default:
                Debug.Log("Action name does not match any of the preset action names");
                break;
        }
    }

    private void ShowDescription(Abilities ability)
    {
        if (abilityDescription.ContainsKey(currentName) && abilityDescription[currentName].ContainsKey(ability))
            descriptionSection.text = abilityDescription[currentName][ability];
        else
            descriptionSection.text = "THIS DESCRIPTION HASN'T BEEN SET UP YET";
    }

    private void ShowTitle(Names name)
    {
        if (characterTitles.ContainsKey(name))
        {
            characterTitle.text = characterTitles[name];
        }            
        else
            characterTitle.text = "THIS TITLE HASN'T BEEN SET UP YET";
    }

    public void ChangeKeyBindsNames(bool isKeyboard)
    {
        if (isKeyboard)
        {
            abilitiesKeyBinds[0].text = "J";
            abilitiesKeyBinds[1].text = "K";
            abilitiesKeyBinds[2].text = "U";
            abilitiesKeyBinds[3].text = "I";
        }
        else
        {
            abilitiesKeyBinds[0].text = "A";
            abilitiesKeyBinds[1].text = "B";
            abilitiesKeyBinds[2].text = "X";
            abilitiesKeyBinds[3].text = "Y";
        }
    }

    public void OnDisable()
    {
        lightAttack.started -= GetDescription;
        heavyAttack.started -= GetDescription;
        mixup.started -= GetDescription;
        iconic.started -= GetDescription;

        lightAttack.Disable();
        heavyAttack.Disable();
        mixup.Disable();
        iconic.Disable();
    }
}
