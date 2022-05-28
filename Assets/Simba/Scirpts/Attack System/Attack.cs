using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using CustomAttributes;

namespace AttackSystem{
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
        [SerializeField] Vector2 _effectRelativePose;
        public Vector2 effectRelativePosition{ get {return _effectRelativePose; } } 
        

        // Proprety
        public GameObject prefabEffect{ get { return _prefabEffect; } }
        public AttackSettings Settings{ get { return _attackSettings; } }
        public GameObject prefabBullet{ get { return _prefabBullet; } }

        
        // Serialization 
        bool distanceAttack; /*!< Utiliser CustomAttributes.ShowIf */

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// ISerializationCallbackReceiver Abstract Method
        /// </summary>
        public void OnAfterDeserialize()
        {
            _attackSettings = new AttackSettings(_canCharge, _maxChargeTime, _minChargeTime, _power, _attackType, _range, _timeOfHit, _bulletRelativePosition);
        }
        
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// ISerializationCallbackReceiver Abstract Method
        /// </summary>
        public void OnBeforeSerialize(){}

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// MonoBehaviour Abstract Method
        /// </summary>
        public void OnValidate(){
            distanceAttack = _attackType == AttackType.Distance;
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-

    }
}