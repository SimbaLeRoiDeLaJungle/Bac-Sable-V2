using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEditor.Animations;
using UnityEditorInternal;
using CharacterSystem;

namespace CharacterGenerator{
    public class AnimatorControllerGenerator
    {
        static string directory_path = "Assets/Simba";

        public static void Create(string characterName, AnimationsData animationsData, Texture2D texture, int pixelsPerUnit)
        {
            string text = EditorUtility.SaveFilePanelInProject("Create New Character Assets", characterName, "", "message", directory_path);
            if (string.IsNullOrEmpty(text)){
                return;
            }
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            else{
                return;
            }

            // #-#-#-#-#-#-#-#-#-#-#-#-#-#
            // créations de l'animator  -#
            // controller               -#
            // #-#-#-#-#-#-#-#-#-#-#-#-#-#
            string pathToController = text + "/" + characterName + ".controller";
            
            UnityEditor.Animations.AnimatorController controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(pathToController);
            controller.AddParameter("direction", UnityEngine.AnimatorControllerParameterType.Bool);
            controller.AddParameter("walk", UnityEngine.AnimatorControllerParameterType.Bool); 
            controller.AddParameter("grounded", UnityEngine.AnimatorControllerParameterType.Bool); 

            // #-#-#-#-#-#-#-#-#-#-#-#-#-#
            // création des             -#
            // SpriteMetaData           -#
            // #-#-#-#-#-#-#-#-#-#-#-#-#-#

            // On recupère quelques données 
            var datas = animationsData.DatasToDictionary();
            string texPath = UnityEditor.AssetDatabase.GetAssetPath(texture);
            
            // On met les paramètres de la textures
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(texPath);
            importer.isReadable = true;
            importer.filterMode = FilterMode.Bilinear;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = pixelsPerUnit;
            
            // Creation des méta data
            List<SpriteMetaData> metas = new List<SpriteMetaData>();
            Dictionary<AnimationName, List<int>> metadict = new Dictionary<AnimationName, List<int>>(); // la liste c'est les numéro des sprites
            int spriteId = 0;
            foreach(KeyValuePair<AnimationName, AnimationData> data in datas){
                int col = 0;
                int line = (int)(texture.height/(data.Value.size.y*pixelsPerUnit))-data.Value.line;
                
                int sizeX = data.Value.size.x*pixelsPerUnit;
                int sizeY = data.Value.size.y*pixelsPerUnit;
                
                int maxCol = (int)(texture.width/(data.Value.size.x*pixelsPerUnit))-1;
                if(maxCol > data.Value.endCol){
                    maxCol = data.Value.endCol;
                }

                List<int> lsmd = new List<int>();
                while(col != maxCol){
                    SpriteMetaData meta = new SpriteMetaData();
                    meta.name = "Sprite" + spriteId.ToString();
                    meta.rect = new Rect(col*sizeX, line*sizeY, sizeX, sizeX);
                    
                    lsmd.Add(spriteId);
                    metas.Add(meta);
                    
                    col++;
                    spriteId++;
                }
                
                metadict.Add(data.Key, lsmd);
            }
            //Sauvegarde des meta data
            importer.spritesheet = metas.ToArray();
            AssetDatabase.ForceReserializeAssets(new List<string>() { texPath });
            AssetDatabase.ImportAsset(texPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();

            // #-#-#-#-#-#-#-#-#-#-#-#-#-#
            // créations des annimation -#
            // #-#-#-#-#-#-#-#-#-#-#-#-#-#

            //On recharge les sprites que l'on vien de créer
            Sprite[] spritesArray = AssetDatabase.LoadAllAssetsAtPath(texPath).OfType<Sprite>().ToArray();
            Dictionary<AnimationName, Sprite[]> sprites = new Dictionary<AnimationName, Sprite[]>();
            var animNames = Enum.GetValues(typeof(AnimationName));
            foreach(var animNameInt in animNames){
                AnimationName an = (AnimationName)animNameInt;
                List<Sprite> ss = new List<Sprite>();
                foreach(var sprite in spritesArray){
                    string stringId = sprite.name.Replace("Sprite", "");
                    int id = int.Parse(stringId);
                    if(metadict[an].Contains(id)){
                        ss.Add(sprite);
                    }
                }
                sprites.Add(an, ss.ToArray());
            }
            //On créer les animations
            var conditions = GetConditions();
            var rootStateMachine = controller.layers[0].stateMachine;
            foreach(KeyValuePair<AnimationName, Sprite[]> item in sprites){
                var frames = item.Value;
                AnimationClip animationClip = new AnimationClip();
                string pathToAnimation = text + "/" + item.Key.ToString() + ".anim";
                AssetDatabase.CreateAsset(animationClip, pathToAnimation);
                animationClip.frameRate = animationsData.GetAnimationData(item.Key).frameRate;
                ObjectReferenceKeyframe[] array = new ObjectReferenceKeyframe[frames.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = default(ObjectReferenceKeyframe);
                    array[i].value = frames[i];
                    array[i].time = (float)i / animationClip.frameRate;
                }
                EditorCurveBinding binding = EditorCurveBinding.PPtrCurve(string.Empty, typeof(SpriteRenderer), "m_Sprite");
                AnimationUtility.SetObjectReferenceCurve(animationClip, binding, array);
                // #-#-#-#-#-#-#-#-#-#-#-#-#-#
                // connection avec le       -#
                // controller               -#
                // #-#-#-#-#-#-#-#-#-#-#-#-#-#
                var state = rootStateMachine.AddState(item.Key.ToString());
                state.motion = animationClip;
                var transition = rootStateMachine.AddAnyStateTransition(state);
                transition.conditions = conditions[item.Key];
            }

            Character asset = ScriptableObject.CreateInstance<Character>();
            asset.SetAnimatorController(controller);
            asset.SetName(characterName);
            string pathToCharacter = text + "/" + characterName + ".asset";
            AssetDatabase.CreateAsset(asset, pathToCharacter);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        public static Dictionary<AnimationName, AnimatorCondition[]> GetConditions(){
            var conditionWL = new[] {
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "walk"
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "direction" 
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "grounded" 
                }
            };
            var conditionWR = new[] {
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "walk"
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "direction" 
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "grounded" 
                }
            };
            var conditionIR = new[] {
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "walk"
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "direction" 
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "grounded" 
                }
            };
            var conditionIL = new[] {
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "walk"
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "direction" 
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.If,
                    parameter = "grounded" 
                }
            };
            var conditionJL = new[] {
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "walk"
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "direction" 
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "grounded" 
                }
            };
            var conditionJR = new[] {
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "walk"
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "direction" 
                },
                new AnimatorCondition {
                    mode = AnimatorConditionMode.IfNot,
                    parameter = "grounded" 
                }
            };
            return new Dictionary<AnimationName, AnimatorCondition[]>{
                {AnimationName.WalkRight, conditionWR },
                {AnimationName.WalkLeft, conditionWL },
                {AnimationName.IdleRight, conditionIR },
                {AnimationName.IdleLeft, conditionIL },
                {AnimationName.JumpLeft, conditionJL },
                {AnimationName.JumpRight, conditionJR }
            };
        }

    }

}




