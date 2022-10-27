# S<sup>4</sup> Super Simple Sound System
`v5.0.0`

So S<sup>4</sup> is as the name implies, a super simple way to go about adding sounds to your game. You can still have some control but when playing a sound, but it can be as simple as `_soundSource.PlaySound("Sparks");`. The system is not super robust, it does extend unity's sound system but not in any major ways. It only adds common functionality such as crossfade, global sounds, sound positioning, etc. It's meant to be simple to get a game's sound up and running. Perfect for game Jams and smaller games. 

There is also a subtitle/closed captioning engine in here. That bit hasn't had a lot of work done on it and honestly I'd have loved to do a larger update before doing the public release but a work project needs this tool and doing a public release makes it easier to license it for that. 


## Setup

Setup is fairly easy, there is a prefab for `S4SoundManager` that just needs to be added to your scenes. This is a global object and can really just be added to your first scene if you don't want to add it to them all. If you want subtitles or closed captions you can drag subs in with it. Just whatever works best for your project. Sound Manager helps keep track of what sounds are playing, what sounds need to play and also takes care of spawning the new sounds. 

Next up, any object in the scene that needs to make sounds just needs to have an `S4SoundSource` added to them. This is where the sound configurations will be made and it's all done within the unity UI. These are all well documented within the tooltips but you just add your sounds to this object.

These sounds can be triggered like so: 

```csharp
using TylerCode.SoundSystem;

...

private S4SoundSource _soundSource;

private void Start()
{
    _soundSource = GetComponent<S4SoundSource>();
}

private void SomeMethodYouHave()
{
    ...
    _soundSource.PlaySound("SoundName");
    ...
}
```

It's that easy. All the options are documented in the tooltips and this documentation will expand as the tool matures. 

