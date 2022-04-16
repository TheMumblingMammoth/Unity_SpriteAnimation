using UnityEngine;
public class DestroyOnStop : MonoBehaviour{
    AnimatedSprite sprite;
    void Awake(){
        sprite = GetComponent<AnimatedSprite>();
    }
    void FixedUpdate(){
        if(sprite.ended)
            DestroyImmediate(gameObject);
    }
}