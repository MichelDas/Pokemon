using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{

    [SerializeField]
    Text messageText;
    PartyMemberUI[] membersSlots;
    List<Pokemon> pokemons;

    public void Init()
    {
        membersSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    // This will be called from Battle system
    // Battle system will provide the list of pokemons that I have
    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;
        for(int i=0; i<membersSlots.Length; i++)
        {
            if(i < pokemons.Count)
            {
                membersSlots[i].SetData(pokemons[i]);
            }
            else
            {
                membersSlots[i].gameObject.SetActive(false);
            }
        }

        messageText.text = "Select a pokemon";
    }

    
    public void UpdateMemberSelection(int selectedMember)
    {
        // change the color of the selected member
        for(int i=0; i<pokemons.Count; i++)
        {
            if(i == selectedMember)
            {
                // change the color
                membersSlots[i].SetSelected(true);
            }
            else
            {
                membersSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }
}
