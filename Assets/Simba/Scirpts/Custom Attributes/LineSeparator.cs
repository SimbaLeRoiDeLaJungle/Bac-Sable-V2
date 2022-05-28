using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace CustomAttributes
{
    /// <summary>
    /// Permet de tracer une ligne dans l'inspecteur
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited= true)]
    public class LineSeparator : PropertyAttribute
    {
        public const float DefaultHeight = 0.5f;

        public float Height { get; private set; }

        public LineSeparator(float height = DefaultHeight){
            Height = height;
        }
    }

    [CustomPropertyDrawer(typeof(LineSeparator))]
    public class LineSeparatorDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight(){
            LineSeparator lineAttr = (LineSeparator)attribute;
            return EditorGUIUtility.singleLineHeight + lineAttr.Height;
        }

        public override void OnGUI(Rect position){
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight/ 3.0f;
            LineSeparator lineAttr = (LineSeparator)attribute;
            rect.height = lineAttr.Height;
            EditorGUI.DrawRect(rect, new Color32(0,0,0,255));
        }
    }
}