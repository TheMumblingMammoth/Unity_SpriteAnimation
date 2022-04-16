using UnityEngine;
using System.Collections.Generic;

public class AnimatedSprite : MonoBehaviour{
    
    public SpriteRenderer sprite {get; private set;}
    [SerializeField] private Clip clip;
    private Clip futureClip = null;
    public bool Casual() { return futureClip == null; }
    byte step, stage;
    public byte GetStage(){ return stage; }
    public void SetStage(byte stage){ 
        this.stage = stage;
        if(clip.frames.Length <= stage) this.stage = 0;
    }
	
    public byte animspeed {get; private set;} = 3;
	bool forward = true;
    public bool ended = true;
    bool stop = false;
    public bool rng;
    List<DelayedClip> delayed_clips;
    bool playing_delayed = false;
    [SerializeField] bool pause;
    #region Options
        public void Pause(){ pause = true;}
        public void Resume(){ pause = false;}
        public void NormalSpeed(){ animspeed = 3; }
        public void DoubleSpeed(){ animspeed = 6; }
    #endregion Options
    public void Awake(){
        sprite = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        delayed_clips = new List<DelayedClip>(1);
        if(sprite == null)
            Debug.Log(" You forgot SpriteRenderer, you moron!");
        
    }
  
    void FixedUpdate(){
        if(ended || pause)
            return;

        if(clip==null){
            return;
        }

        UpdateSound();

        
        if(rng)
            step+=(byte)Random.Range(1,animspeed);    
        else   
            step+=animspeed;
        
        DelayedClip [] temp = delayed_clips.ToArray();
        for(int i = 0; i < temp.Length; i++){
            temp[i].delay-=animspeed;
            if(temp[i].delay <=0 ){
                PlayOnce(temp[i].clip);
                playing_delayed = true;
                delayed_clips.Remove(temp[i]);
            }
        }
        
        try{
            if(step >= clip.frames[stage].delay){
                step = 0;
                NextFrame();
            }
        }catch{
            Debug.Log(clip.name + " " + stage + " stage is not found");
        }
    }

    void NextFrame(){
        switch(clip.type){
            case AniType.Circle:
                stage++;
                if(stage == clip.frames.Length){ // last frame on circle
                    if(futureClip != null  || stop)  SwapFuture();
                    stage = 0;
                }
            break;
            case AniType.Pong:
            
                if(forward){
                    stage++;
                    if(stage == clip.frames.Length-1){
                        forward = false;
                    }
                }else{
                    stage--;
                    if(stage == 0){ // lastframe on pong
                        forward = true;
                        if(futureClip != null || stop)  SwapFuture();
                    }   
                }
            break;
        }
        
        if(! ended)
            sprite.sprite = clip.frames[stage].pic;        
    }

    public bool FirstFrame(){
        return stage == 0 && step == 0;
    }

    public bool LastFrame(){
        return stage == clip.frames.Length - 1 && step >= clip.frames[clip.frames.Length - 1].delay - animspeed;
    }
    public string CurrentClip(){
        if(clip != null)
            return clip.name;
        else
            return "";
    }
    public void ChangeClip(Clip newClip){
        playing_delayed = false;
        clip = newClip;
        stop = false;
        stage = 0;
        forward = true;
        if(clip == null)
            Debug.Log("Clip == null");
        try{
            sprite.sprite = clip.frames[0].pic;
        }catch{
            Debug.Log(clip.name);
            sprite.sprite = clip.frames[0].pic;
        }
        ended = false;
    }
    public void PlayOnce(Clip newClip){
        if(!playing_delayed)
            futureClip = clip;
        clip = newClip;
        stage = 0;
        forward = true;
        sprite.sprite = clip.frames[0].pic;
        ended = false;       
    }
    public void PlayOnceThenChange(Clip newClip, Clip nextClip){
        futureClip = nextClip;
        clip = newClip;
        stop = false;
        stage = 0;
        forward = true;
        sprite.sprite = clip.frames[0].pic;
        ended = false;
    }
    public void PlayOnceThenStop(Clip newClip){
        futureClip = clip;
        stop = true;
        clip = newClip;
        stage = 0;
        forward = true;
        sprite.sprite = clip.frames[0].pic;
        ended = false;
    }
    public void PlayClipLater(Clip newClip, int delay, bool stop){
        delayed_clips.Add(new DelayedClip(newClip, delay, stop));
    }
    private void SwapFuture(){
        if(stop){
            ended = true;
        }else{
            clip = futureClip;
            futureClip = null;
        }
        playing_delayed = false;
    }
    public void SetOrder(string layer, int order){
        sprite.sortingLayerName = layer;
        sprite.sortingOrder = order;
    }

    public void Left(){
        sprite.flipX = false;
    }
    public void Right(){
        sprite.flipX = true;
    }

    public void LastFrameOf(Clip newClip){
        clip = newClip;
        futureClip = null;
        stage = (byte)(clip.frames.Length - 1);
        forward = true;
        sprite.sprite = clip.frames[stage].pic;
        ended = true;
    }
    

    #region Sound
        [SerializeField] bool constantSound;
        [SerializeField] AudioSource sound;
        public void LoadSound(){
            sound = Instantiate<LocalSound>(Resources.Load<LocalSound>("Sound/Local Audio Source")).GetComponent<AudioSource>();
            sound.transform.parent = gameObject.transform;
            sound.transform.localPosition = new Vector3(0, 0, 0);
        }
        void UpdateSound(){
            if(sound == null) return;
            if(sound.clip != clip.sound){
                sound.clip = clip.sound;
                sound.Play();
            }
        }
    #endregion Sound

}
