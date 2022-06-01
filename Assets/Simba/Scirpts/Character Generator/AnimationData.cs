using System;
using UnityEngine;
namespace CharacterGenerator{
    [Serializable]
    public struct AnimationData
    {
        [SerializeField] AnimationName _name;
        public AnimationName name { get { return _name; } }

        [SerializeField] int _line;
        public int line { get { return _line; } }

        [SerializeField] int _endCol;
        public int endCol { get { return _endCol; } }

        [SerializeField] Vector2Int _size;
        public Vector2Int size { get { return _size; } }

        [SerializeField] float _frameRate;
        public float frameRate { get { return _frameRate; } }


        public AnimationData(string name, int line, int endCol, int sizeX,int sizeY, float frameRate){
            AnimationName.TryParse(name, out _name);
            _line = line;
            _endCol = endCol;
            _size = new Vector2Int(sizeX, sizeY);
            _frameRate = frameRate;
        }
    }
}