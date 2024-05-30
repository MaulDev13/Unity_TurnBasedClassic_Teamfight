using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModType 
{
    OnRevive,
    FirstTurn,
    StartTurn,
    Heal,
    Heal_ChangeValue,
    Shield,
    Shield_ChangeValue,
    Attack,
    Attack_ChangeValue,
    TakeDamage,
    TakDamage_ChangeValue,
    EndTurn,
    OnDead,
}

[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifier/Default")]
public class Mod : ScriptableObject
{
    [SerializeField] public AnimatorOverrideController _animator; // A target
    [SerializeField] public GameObject hit_Effect;

    public string modName;

    public bool isStackable;
    public float stack;

    public ModType modType;

    [HideInInspector] BattleUnit myUnit;

    public virtual void Init(BattleUnit user) { }

    public virtual void Init(BattleUnit user, float value) 
    {
        myUnit = user;

        stack = value;
    }

    public virtual void Active() { }

    public virtual void Active(float value) { }

    public virtual void Active(BattleUnit user) { }

    public virtual float Active_ReturnFloat() { return 0f; }

    public virtual float Active_ReturnFloat(float value) { return 0f; }

    public virtual float Active_ReturnFloat(BattleUnit user) { return 0f; }

    public virtual bool Comparer(List<Mod> mods)
    {
        if (!isStackable)
            return false;

        foreach(Mod m in mods)
        {
            if (string.Equals(m.modName, modName, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    public virtual Mod Comparer_ReturnMod(List<Mod> mods)
    {
        foreach (Mod m in mods)
        {
            if (string.Equals(m.modName, modName, System.StringComparison.OrdinalIgnoreCase))
                return m;
        }

        return null;
    }

    public string GetDesc() { return "Empty"; }
}
