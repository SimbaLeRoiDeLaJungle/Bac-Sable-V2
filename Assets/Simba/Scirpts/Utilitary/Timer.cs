using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utilitary{
    /// <summary>
    /// Petit miniteur
    /// </summary>
    /// TimerMode :  <br/> 
    /// LOCK : le temps reste bloqué à maxTime  <br/>
    /// RESET : le temps se réintialise automatiquement <br/>
    /// IGNORE : le temps continue de croitre
    public class Timer
    {
        TimerMode timerMode; 
        public float time{ get; private set; } /*!< temps écouler. */
        public float maxTime{ get; private set; } /*!< temps limite. */

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Constructor.
        /// </summary>
        public Timer(float maxTime, TimerMode timerMode = TimerMode.RESET){
            this.time = 0;
            this.maxTime = maxTime;
            this.timerMode = timerMode;
        }

        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Ajoute dt à time, revoi vrai si time > maxTime.
        /// </summary>
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
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        // #-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
        /// <summary>
        /// Met time à 0.
        /// </summary>
        public void Reset(){
            time = 0;
        }
    }
}