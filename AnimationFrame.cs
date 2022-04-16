using System;
using UnityEngine;

[Serializable]
public class AnimationFrame{
    public Sprite pic;
    public byte delay=20;
    public AnimationFrame(Sprite sprite){
        pic = sprite;
        delay = 20;
    }
}
