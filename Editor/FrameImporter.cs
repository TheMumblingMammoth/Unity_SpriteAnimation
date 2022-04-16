using UnityEditor;
using UnityEngine;
using System.IO;
public class FrameImporter{
    public static Sprite[] ImportSprites(string path){
        string [] frameNames = Directory.GetFiles(path,"*.png");
        Sprite [] sprites = new Sprite[frameNames.Length];
        path = path.Remove(0, (Application.dataPath).Length);
        path = path +'/'+ path.Split('/')[path.Split('/').Length-1];
        for(int i =0; i< frameNames.Length;i++){
            //Debug.Log("Assets"+path+i+".png");
            sprites[i] =  (Sprite) AssetDatabase.LoadAssetAtPath("Assets"+path+(i+1)+".png",typeof(Sprite));
            //Debug.Log("Assets"+path+(i+1)+".png");
        }
        return sprites;
    }
}