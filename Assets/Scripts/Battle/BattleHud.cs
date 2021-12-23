using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text StatusText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color poisonColor;
    [SerializeField] Color burnColor;
    [SerializeField] Color sleepColor;
    [SerializeField] Color paralysisColor;
    [SerializeField] Color freezeColor;


    Pokemon _pokemon;

    Dictionary<ConditionID, Color> statusColor;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        levelText.text = "LV " +pokemon.Level;
        hpBar.SetHP((float) pokemon.HP / pokemon.MaxHP);
        statusColor = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.Poison, poisonColor },
            {ConditionID.Burn, burnColor},
            {ConditionID.Sleep, sleepColor },
            {ConditionID.Paralysis, paralysisColor },
            {ConditionID.Freeze, freezeColor },
        };
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }

    // if Status is changed this will call
    void SetStatusText()
    {
        //set status of _pokemon
        if(_pokemon.Status == null)
        {
            StatusText.text = "";
        }
        else
        {
            StatusText.text = _pokemon.Status.HUDName;
            StatusText.color = statusColor[_pokemon.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
        if (_pokemon.HpChanged)
        {
            yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHP);
            _pokemon.HpChanged = false;
        }
    }
}
