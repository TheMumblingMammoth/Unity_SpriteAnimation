using UnityEngine;
public class Clip : MonoBehaviour{
    public AniType type;
    [SerializeField] public AnimationFrame [] frames;
    [SerializeField] public AudioClip sound;
    
    public int Length(){
        int sum = 0;
        for(int i = 0; i < frames.Length; i++)
            sum += frames[i].delay;
        return sum;
    }
    public void LoadFrames(Sprite [] sprites){
        frames = new AnimationFrame[sprites.Length];
        for(int i = 0; i< sprites.Length; i++){
            frames[i] = new AnimationFrame(sprites[i]);
        }  
    }   

    public Sprite LastFrame(){
        return frames[frames.Length-1].pic;
    }

    public bool hasSound(){
        return sound != null;
    }

    public void SetDelay(byte delay){
        foreach(AnimationFrame frame in frames)
            frame.delay = delay;
    }
}
