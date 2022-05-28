using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using CustomAttributes;

[CreateAssetMenu(fileName = "attack_name", menuName = "ScriptableObjects/Attack", order = 2)]
public class Attack : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Settings")]
    [SerializeField] bool _canCharge;
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(_canCharge))]
    [SerializeField] float _maxChargeTime;    
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(_canCharge))]
    [SerializeField] float _minChargeTime;
    [SerializeField] int _power;  
    [SerializeField] float _range;
    [SerializeField] float _timeOfHit;
    [SerializeField] AttackType _attackType;
    
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(distanceAttack))]
    [SerializeField] GameObject _prefabBullet;
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(distanceAttack))]
    [SerializeField] Vector2 _bulletRelativePosition;
    AttackSettings _attackSettings;
    
    [LineSeparator]
    [Header("GFX")]
    [SerializeField] GameObject _prefabEffectCharge;
    [SerializeField] GameObject _prefabEffect;
    
    
    

    // Proprety
    public GameObject prefabEffect{ get { return _prefabEffect; } }
    public AttackSettings Settings{ get { return _attackSettings; } }
    public GameObject prefabBullet{ get { return _prefabBullet; } }

    
    
    /// #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
    /// #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
    /// #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
    /// Serialization
    bool distanceAttack; // pour utiliser ShowIf
    public void OnAfterDeserialize()
    {
        _attackSettings = new AttackSettings(_canCharge, _maxChargeTime, _minChargeTime, _power, _attackType, _range, _timeOfHit, _bulletRelativePosition);
    }
    public void OnBeforeSerialize(){}

    public void OnValidate(){
        distanceAttack = _attackType == AttackType.Distance;
    }


}

[Serializable]
public struct AttackSettings
{

    public bool canCharge { get; private set; }
    public float maxChargeTime { get; private set; }
    public float minChargeTime { get; private set; }
    public int power { get; private set; }
    public AttackType attackType{ get; private set; }
    public float range { get; private set; }
    public float timeOfHit {get; private set;}
    public Vector2 bulletRelativePosition {get; private set;}
    public AttackSettings(bool p_canCharge, float p_maxChargeTime, float p_minChargeTime, int p_power, AttackType p_attackType, float p_range, float p_timeOfHit, Vector2 p_bulletRelativePosition){
        canCharge = p_canCharge;
        maxChargeTime = p_maxChargeTime;
        minChargeTime = p_minChargeTime;
        power = p_power;
        attackType = p_attackType;
        range = p_range;
        timeOfHit = p_timeOfHit;
        bulletRelativePosition = p_bulletRelativePosition;
    }
}

[Serializable]
public struct ActionPad
{
    public Attack first;
    public Attack second;
}

public enum AttackType{
    Contact,
    Distance
}