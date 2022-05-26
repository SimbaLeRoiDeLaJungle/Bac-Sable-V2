using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Simba{
    // Un petit cronomètre
    public class Timer
    {
        float time;
        float maxTime;
        TimerMode timerMode;
        public float Time{ get { return time; } }
        public float MaxTime{ get { return maxTime; } }


        public Timer(float maxTime, TimerMode timerMode = TimerMode.RESET){
            this.time = 0;
            this.maxTime = maxTime;
            this.timerMode = timerMode;
        }

        public bool Update(float dt){
            time += dt;
            bool result = time >= maxTime;
            if(result){
                if(timerMode == TimerMode.RESET){
                    time = 0;
                }
                else if(timerMode == TimerMode.LOCK){
                    time = maxTime;
                }
            }
            return result;
        }

        public void Reset(){
            time = 0;
        }
    }

    public enum TimerMode{
        RESET = 0,
        LOCK = 1,
        IGNORE = 2
    }
}