using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AttackSystem{
    
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
}