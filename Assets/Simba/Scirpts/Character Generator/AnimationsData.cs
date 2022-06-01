using System;
using UnityEngine;
using System.Collections.Generic;
namespace CharacterGenerator{
    [Serializable]
    public class AnimationsData{

        [SerializeField] List<AnimationData> _datas;

        public Dictionary<AnimationName, AnimationData> DatasToDictionary(){
            var animNames = Enum.GetValues(typeof(AnimationName));
            Dictionary<AnimationName, AnimationData> result = new Dictionary<AnimationName, AnimationData>();
            foreach(var animName in animNames){
                AnimationName aname = (AnimationName) animName;
                result.Add(aname, GetAnimationData(aname));
            }
            return result;
        }

        public AnimationData GetAnimationData(AnimationName animationName){
            foreach(var data in _datas){
                if(data.name == animationName){
                    return data;
                }
            }
            Debug.Log("Unknow animation name");
            throw new Exception();
        }

        public AnimationsData(List<AnimationData> datas){
            this._datas = datas;
        }
    }
}