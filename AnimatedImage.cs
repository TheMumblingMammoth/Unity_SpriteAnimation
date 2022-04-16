using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class AnimatedImage : MonoBehaviour{
    
    public Image image {get; private set;}
    [SerializeField] private Clip clip;
    private Clip futureClip = null;
    byte step, stage; 
	[SerializeField] byte animspeed = 3, fStage;
	bool forward = true;
    [SerializeField] bool rng;
    [SerializeField] bool cicle = true;
    public bool ended = true;
    bool stop = false;
    List<DelayedClip> delayed_clips;
    bool playing_delayed;
    
    void Awake(){
        delayed_clips = new List<DelayedClip>(1);
        try{
            image = GetComponentInChildren<Image>();
        }catch{
            Debug.Log("No Image to Animate in " + gameObject.name);
        }
        if(hasSound)
            LoadSound();
    }
    
    public void SetColor(Color color){
        image.color = color;
    }

    void FixedUpdate(){
        if(clip==null)
            return;
        if(rng)
            step+=(byte)Random.Range(0,animspeed);    
        else
            step+=animspeed;

        UpdateSound();

        DelayedClip [] temp = delayed_clips.ToArray();
        for(int i = 0; i < temp.Length; i++){
            temp[i].delay-=animspeed;
            if(temp[i].delay <=0 ){
                PlayOnce(temp[i].clip);
                playing_delayed = true;
                delayed_clips.Remove(temp[i]);
            }
        }


        if(step >= clip.frames[stage].delay){
            step = 0;
            NextFrame();
        }
    }

    void NextFrame(){
        switch(clip.type){
            case AniType.Circle:
                stage++;
                if(stage == clip.frames.Length){ // last frame on circle
                    if(futureClip != null)  SwapFuture();
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
                        if(futureClip != null)  SwapFuture();
                    }   
                }
            break;
        }
        
        if(! ended)
            image.sprite = clip.frames[stage].pic;      
    }

    public void ChangeClip(string clipPath){
        ended = false;
        playing_delayed = false;
        Clip temp = Resources.Load<Clip>(clipPath);
        if(clip != temp) clip = temp;
        if(clip == null)    Debug.Log(clipPath);
        step = 0;
        stage = 0;
        if(image == null) Awake();
        image.sprite = clip.frames[0].pic;
    }
    public void ChangeClip(Clip new_clip){
        ended = false;
        playing_delayed = false;
        clip = new_clip;
        if(clip == null)    Debug.Log("AnimatedImage missuse");
        step = 0;
        stage = 0;
        if(image == null) Awake();
        image.sprite = clip.frames[0].pic;
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

    public void PlayOnce(Clip newClip){
        if(!playing_delayed)
            futureClip = clip;
        clip = newClip;
        stage = 0;
        forward = true;
        image.sprite = clip.frames[0].pic;
        ended = false;       
    }

    public void PlayOnceThenChange(Clip newClip, Clip nextClip){
        futureClip = nextClip;
        clip = newClip;
        stage = 0;
        forward = true;
        image.sprite = clip.frames[0].pic;
        ended = false;
    }

    public void PlayOnceThenStop(Clip newClip){
        futureClip = clip;
        stop = true;
        clip = newClip;
        stage = 0;
        forward = true;
        image.sprite = clip.frames[0].pic;
        ended = false;
    }

    public void PlayClipLater(Clip newClip, int delay, bool stop){
        delayed_clips.Add(new DelayedClip(newClip, delay, stop));
    }

    #region Sound
        [SerializeField] bool hasSound;
        [SerializeField] LocalSound sound;
        public void LoadSound(){
            sound = Instantiate<LocalSound>(Resources.Load<LocalSound>("Sound/UI Audio Source"));
            sound.transform.parent = gameObject.transform;
            sound.transform.localPosition = new Vector3(0, 0, 0);
        }
        void UpdateSound(){
            if(sound == null) return;
            if(sound.source.clip != clip.sound){
                sound.source.clip = clip.sound;
                sound.source.Play();
            }
        }
    #endregion Sound
}
