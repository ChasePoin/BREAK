using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    static public AudioController a;
    public AudioSource source;
    public List<AudioClip> clips;
    void Awake() {
        a = this;
    }
    static public void PlayClip(string clipName) {
        AudioClip clip = a.clips.Where(c => c.name == clipName).ElementAt(0);
        a.source.PlayOneShot(clip);
    }
    static public void RepeatClip(string clipName) {
        AudioClip clip = a.clips.Where(c => c.name == clipName).ElementAt(0);
        if (a.source.clip == clip) return;
        a.source.clip = clip;
        a.source.loop = true;
        a.source.Play();
    }
    static public void StopClip() {
        a.source.Stop();
        a.source.loop = false;
        a.source.clip = null;
    }
}