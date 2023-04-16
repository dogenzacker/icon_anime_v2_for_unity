namespace icon_anime_v2
{
    using System;
    using UnityEngine;
    public abstract class BasePlayer : MonoBehaviour
    {

        [SerializeField]
        private bool m_PlayAutomatically;

        [SerializeField]
        private Clip m_icon_anime_v2_clip;

        public float Speed => m_Speed;
        public bool IsPlaying => m_IsPlaying;
        public float NormalizeTime
        {
            get
            {
                if (m_icon_anime_v2_clip == null) return 0;
                return (float)ClampFrameCount(DurationToTotalFrameCount(m_ElapsedTime)) / (float)m_icon_anime_v2_clip.FrameCount;
            }
        }

        private float m_ElapsedTime = 0;
        private bool m_IsPlaying = false;
        private float m_Speed = 1;

        private void Awake()
        {
            __OnAwake__();
        }

        private void Start()
        {
            if (m_PlayAutomatically) Play();
            __OnStart__();
        }

        private void Update()
        {
            if (IsPlaying == false) return;
            if (m_icon_anime_v2_clip == null) return;
            m_ElapsedTime += Time.deltaTime * Speed;
            var totalFrameCount = DurationToTotalFrameCount(m_ElapsedTime);
            var clampFrameCount = ClampFrameCount(totalFrameCount);

            if (this.m_icon_anime_v2_clip.FrameCount <= totalFrameCount && this.m_icon_anime_v2_clip.IsLooping == false)
            {
                Stop();
                return;
            }
            SetFrame(clampFrameCount);
        }

        private void OnDestroy()
        {
            __OnDestroy__();
        }

        public void SetClip(Clip icon_anime_v2_clip) => this.m_icon_anime_v2_clip = icon_anime_v2_clip;
        public void SetSpeed(float speed) => this.m_Speed = speed;
        public void Play(int atFrameCount = 0)
        {
            m_ElapsedTime = FrameToDruation(atFrameCount);
            m_IsPlaying = true;
            SetFrame(atFrameCount);
            __OnPlay__();
        }

        public void Resume()
        {
            m_IsPlaying = true;
            __OnResume__();
        }

        public void Pause()
        {
            m_IsPlaying = false;
            __OnPause__();
        }

        public void Stop()
        {
            m_IsPlaying = false;
            m_ElapsedTime = 0;
            __OnStop__();
        }

        protected virtual void __OnAwake__() { }

        protected virtual void __OnStart__() { }

        protected virtual void __OnResume__() { }

        protected virtual void __OnPause__() { }

        protected virtual void __OnStop__() { }

        protected virtual void __OnPlay__() { }

        protected virtual void __OnSetFrame__(Sprite sprite) { }

        protected virtual void __OnDestroy__() { }

        private int DurationToTotalFrameCount(float duration)
        {
            return Mathf.FloorToInt(duration * this.m_icon_anime_v2_clip.FPS);
        }

        private int ClampFrameCount(int frameCount)
        {
            return frameCount % this.m_icon_anime_v2_clip.FrameCount;
        }

        private float FrameToDruation(int frameCount)
        {
            return frameCount * (1.0F / this.m_icon_anime_v2_clip.FPS);
        }

        private void SetFrame(int frameCount)
        {
            var sprite = m_icon_anime_v2_clip.GetFrame(frameCount);
            __OnSetFrame__(sprite);
        }

    }
}