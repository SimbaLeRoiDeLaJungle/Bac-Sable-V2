using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEditor.Animations;
using UnityEditorInternal;
using CharacterGenerator;

public class MyWindow : EditorWindow, IHasCustomMenu
{
    private XDocument xmlDoc;
    private Texture2D texture;
    private int pixelsPerUnit = 64;
    private string name = "name";
    [MenuItem("Window/Characters Creator")]
    private static void ShowWindow()
    {
        GetWindow<MyWindow>().Show();
    }
    
    // This interface implementation is automatically called by Unity.
    void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
    {
        GUIContent content = new GUIContent("My Custom Entry");
        menu.AddItem(content, false, MyCallback);
    }
    
    private void MyCallback()
    {
        Debug.Log("My Callback was called.");
    }

    private void OnGUI(){
        // Basic info
        GUILayout.Label ("Information", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("CharacterName");
        GUIStyle style = EditorStyles.textField;
        style.fixedWidth = 150f;
        name = GUILayout.TextField(name, 25, style);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        // 
        GUILayout.Label ("Animation Settings", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label ("Spritesheet texture");
        texture = EditorGUILayout.ObjectField(texture, typeof(Texture2D), false) as Texture2D;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //
        GUILayout.BeginHorizontal();
        GUILayout.Label ("pixels per units");
        pixelsPerUnit = EditorGUILayout.IntField(pixelsPerUnit);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Generate")) {
            var animationsData = GetAnimationDataFromXML();
            AnimatorControllerGenerator.Create(name, animationsData, texture, pixelsPerUnit);
        }
    }

    private void OnEnable(){
        GetAnimationDataFromXML();
    }

    private void OnInspectorUpdate(){
        Repaint();
    }

    private AnimationsData GetAnimationDataFromXML(){
        xmlDoc = XDocument.Load("Assets/animation_data.xml");
        var items = xmlDoc.Descendants("animationData");
        List<AnimationData> datas = new List<AnimationData>();
        foreach(var item in items){
            string name = "";
            int line = -1;
            int endCol = -1;
            int sizeX = -1;
            int sizeY = -1;
            float frameRate = -1f;
            foreach(var prop in item.Elements()){
                string itemName = prop.Name.ToString();
                if(itemName == "name"){
                    name = (string)prop;
                }
                else if(itemName == "line"){
                    line = (int)prop;
                }
                else if(itemName == "endCol"){
                    endCol = (int)prop;
                }
                else if(itemName == "sizeX"){
                    sizeX = (int)prop;
                }
                else if(itemName == "sizeY"){
                    sizeY = (int)prop;
                }
                else if(itemName == "frameRate"){
                    frameRate = (float)prop;
                }
            }
            AnimationData aData = new AnimationData(name,line,endCol,sizeX,sizeY,frameRate);
            datas.Add(aData);
        }
        AnimationsData animationsData = new AnimationsData(datas);
        return animationsData;
        
    }
}
