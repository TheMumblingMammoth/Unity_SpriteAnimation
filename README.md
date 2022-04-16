# Unity_SpriteAnimation
an ongoing assets with scripts for convinient sprite-based animation

the files in the "Editor" folder should be placed in the "Editor" folder in your project. They work with the Unity editor itself, and such placement is necessary for correct operation.

the scripts are mostly self-explained. the main ones are AnimatedSprite and AnimationImage for animation itself, and Clip/ClipSet for storing clips. Animation is based on FixedUpdate(), Clips can be reversed, changed, played with delay or played once then changed to another clip.

you can create Clips via drag-drop, but it's not any better than Unity's Animation scripts. I recommend to auto-create Clips and ClipSets via correct naming and integrated interface. I recommend you to create "Clip" folder and store clips there.

For example, to create a clipset for character with multiple animations, you need to place all the pictures in folders according to the animations and all these folders in a folder named **Сharacter_Name**, somewhere in **Assets/Sprites** folder. Then you create a GameObject named **Сharacter_NameClipSet** and place it inside **Сharacter_Name** folder somewhere inside **Assets/Resources/Clips** folder. Then just click "Create from folder".


You'll probably need to adjust naming orientation in Editor scripts. And private/public access in AnimatedSprite and AnimatedImage.
