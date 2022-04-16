public class DelayedClip{
    public int delay;
    public Clip clip;
    public bool stop;
    public DelayedClip(Clip newClip, int time, bool stop){
        delay = time;
        clip = newClip;
        this.stop = stop;
    }
}