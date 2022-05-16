using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///Developed by Indie Games Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]

public class MusicPlayer : MonoBehaviour
{      
	/// <summary>
	/// The audio source reference.
	/// </summary>
	public AudioSource audioSource;
	
	/// <summary>
	/// The total time of the audio clip.
	/// </summary>
	private string totalTime = "00:00";
	
	/// <summary>
	/// The index of the current clip.
	/// </summary>
	[HideInInspector]
	public int currentClipIndex;
	
	/// <summary>
	/// Whether a click down on the music player .
	/// </summary>
	private bool musicPlayerClickDown;
	
	/// <summary>
	/// Whether the music player is interrupted.
	/// </summary>
	private bool interrupted;
	
	/// <summary>
	/// Whether the music player is  muted.
	/// </summary>
	private bool muted;
	
	/// <summary>
	/// The music time.
	/// </summary>
	public MusicTime musicTime;
	
	/// <summary>
	/// The sound level slider reference.
	/// </summary>
	public Slider soundLevelSlider;
	
	/// <summary>
	/// The music slider reference.
	/// </summary>
	public Slider musicSlider;
	
	/// <summary>
	/// The audio clips references.
	/// </summary>
	public AudioClip[] audioClips;
	
	/// <summary>
	/// The current audio clip.
	/// </summary>
	private AudioClip currentAudioClip;
	
	/// <summary>
	/// The sound icons references.
	/// </summary>
	public Sprite[]soundIcons;
	
	/// <summary>
	/// The repeat icons references.
	/// </summary>
	public Sprite[]repeatIcons;
	
	/// <summary>
	/// The play icons references.
	/// </summary>
	public Sprite playIcons;
	
	/// <summary>
	/// The pause icons references.
	/// </summary>
	public Sprite pauseIcons;
	
	/// <summary>
	/// The repeat button image reference.
	/// </summary>
	public Image repeatButtonImage;
	
	/// <summary>
	/// The repeat button image reference.
	/// </summary>
	public Image soundButtonImage;
	
	/// <summary>
	/// The play button image reference.
	/// </summary>
	public Image playButtonImage;
	
	/// <summary>
	/// Whether to play the first audio clip on start.
	/// </summary>
	public bool playOnStart = true;

	/// <summary>
	/// Whether to skip audio playing
	/// </summary>
	private bool skipPlay;

	/// <summary>
	/// The music info animator.
	/// </summary>
	public Animator musicInfoAnimator;
	
	/// <summary>
	/// Whether the click began on music slider or not.
	/// </summary>
	[HideInInspector]
	public bool clickBeganOnMusicSlider;
	
	// Use this for initialization
	void Start ()
	{
		///Setting up the references
		if (audioSource == null) {
			audioSource = GetComponent<AudioSource> ();
		}
		
		if (soundLevelSlider == null) {
			soundLevelSlider = GameObject.FindGameObjectWithTag ("SoundLevelSlider").GetComponent<Slider> ();
		}
		
		if (musicSlider == null) {
			musicSlider = GameObject.FindGameObjectWithTag ("MusicSlider").GetComponent<Slider> ();
		}
		
		if (musicTime == null) {
			musicTime = GameObject.FindGameObjectWithTag ("MusicTime").GetComponent<MusicTime> ();
		}
		
		if (musicInfoAnimator == null) {
			GameObject musicInfo = GameObject.Find ("Music Info");
			if (musicInfo != null)
				musicInfoAnimator = musicInfo.GetComponent<Animator> ();
		}
	
		///Set sound level slider boundary
		soundLevelSlider.minValue = 0;
		soundLevelSlider.maxValue = 1;
		SetRepeatIcon ();

		///Set the initial audio clip
		SetUpAudioClip (0,playOnStart);
		audioSource.clip = currentAudioClip;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!audioSource.isPlaying && !interrupted) {
			StopAudioClip ();
		}
		
		if (audioSource.isPlaying) {
			if (!clickBeganOnMusicSlider) {
				SetMusicSliderValue ();
			}
		}
		
		///Set time of the audio clip
		musicTime.SetTime (musicSlider.value, totalTime);
		///Set the sound volume
		audioSource.volume = soundLevelSlider.value;
	}
	
	/// <summary>
	/// Set the value of music slider.
	/// </summary>
	private void SetMusicSliderValue ()
	{
		musicSlider.value = audioSource.time;
	}
	
	/// <summary>
	/// Set the audio clip.
	/// </summary>
	/// <param name="audioClip">The Audio clip.</param>
	public void SetAudioClip (AudioClip audioClip)
	{
		this.currentAudioClip = audioClip;
	}
	
	/// <summary>
	/// Play the audio clip at time.
	/// </summary>
	/// <param name="time">time in seconds.</param>
	public void PlayAudioClipAtTime (float time)
	{
		 skipPlay = false;
		//Avoid error execution result 
		if (!(time >= 0 && time < currentAudioClip.length)) {
			time = 0;
			skipPlay = true;
		}
		
		playButtonImage.sprite = pauseIcons;
		interrupted = false;
		audioSource.time = time;
		
		if (skipPlay && !audioSource.loop) {
			audioSource.Stop ();
		} else {
			audioSource.Play ();
		}
	}
	
	/// <summary>
	/// Play the audio clip.
	/// </summary>
	public void PlayAudioClip ()
	{
		playButtonImage.sprite = pauseIcons;
		interrupted = false;
		 skipPlay = false;
		//Avoid error execution result 
		if (!(musicSlider.value >= 0 && musicSlider.value < currentAudioClip.length)) {
			musicSlider.value = 0;
			skipPlay = true;
		}
		audioSource.time = musicSlider.value;

		if (skipPlay && !audioSource.loop) {
			audioSource.Stop ();
		} else {
			audioSource.Play ();
		}

	}
	
	/// <summary>
	/// Pause the audio clip.
	/// </summary>
	public void PauseAudioClip ()
	{
		playButtonImage.sprite = playIcons;
		interrupted = true;
		audioSource.Pause ();
	}
	
	/// <summary>
	/// Stop the audio clip.
	/// </summary>
	public void StopAudioClip ()
	{
		interrupted = true;
		playButtonImage.sprite = playIcons;
		audioSource.Stop ();
		audioSource.time = 0;
		SetMusicSliderValue ();
	}
	
	/// <summary>
	/// Mute the audio clip.
	/// </summary>
	public void MuteAudioClip ()
	{
		soundButtonImage.sprite = soundIcons [3];
		muted = true;
		audioSource.mute = true;
	}
	
	/// <summary>
	/// Unmute the audio clip.
	/// </summary>
	public void UnMuteAudioClip ()
	{
		soundButtonImage.sprite = soundIcons [0];
		muted = false;
		audioSource.mute = false;
	}
	
	/// <summary>
	/// Toggles the audio source loop.
	/// </summary>
	public void ToggleLoop ()
	{
		audioSource.loop = !audioSource.loop;
		SetRepeatIcon ();
	}
	
	/// <summary>
	/// Set the repeat icon.
	/// </summary>
	private void SetRepeatIcon ()
	{
		if (audioSource.loop) {
			repeatButtonImage.sprite = repeatIcons [1];
		} else {
			repeatButtonImage.sprite = repeatIcons [0];
		}
	}
	
	/// <summary>
	///Play the next audio clip
	/// </summary>
	public void NextAudioClip ()
	{
		if (currentClipIndex + 1 > 0 && currentClipIndex + 1 < audioClips.Length) {
			if (musicInfoAnimator != null)
				musicInfoAnimator.SetTrigger ("Toggle");
			SetUpAudioClip (currentClipIndex + 1,!interrupted);
		}
	}
	
	/// <summary>
	/// Play the previous audio clip.
	/// </summary>
	public void PreviousAudioClip ()
	{
		if (currentClipIndex - 1 >= 0 && currentClipIndex - 1 < audioClips.Length) {
			musicInfoAnimator.SetTrigger ("Toggle");
			SetUpAudioClip (currentClipIndex - 1,!interrupted);
		}
	}
	
	/// <summary>
	/// Set up the audio clip for the audio source.
	/// </summary>
	/// <param name="index">Index.</param>
	/// <param name="playClip">If set to true , play the clip.</param>
	private void SetUpAudioClip (int index,bool playClip)
	{
		if (!(index >= 0 && index < audioClips.Length)) {
			return;
		}
		
		currentClipIndex = index;
		currentAudioClip = audioClips [index];
		if (currentAudioClip != null) {
			musicSlider.minValue = 0;
			musicSlider.maxValue = currentAudioClip.length;
			totalTime = MusicTime.TimeToString (currentAudioClip.length);
		} else {
			Debug.Log ("AudioClip is undefined");
		}
		audioSource.clip = currentAudioClip;
		StopAudioClip ();
		if (playClip) {
			PlayAudioClip ();
		}
	}
	
	public bool Interrupted {
		get { return this.interrupted;}
	}
	
	public bool Playing {
		get{ return this.audioSource.isPlaying;}
	}
	
	public bool Muted {
		get{ return this.muted;}
	}
	
	public bool isLoop {
		get{ return this.audioSource.loop;}
	}
}