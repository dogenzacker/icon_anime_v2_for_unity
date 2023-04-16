namespace icon_anime_v2
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class ImagePlayer : BasePlayer
    {
        private Image m_Image;
        protected override void __OnAwake__()
        {
            m_Image = GetComponent<Image>();
            SetAlpha(0);
        }

        protected override void __OnStop__()
        {
            SetAlpha(0);
            m_Image.sprite = null;
        }

        protected override void __OnPlay__()
        {
            SetAlpha(1);
        }

        protected override void __OnResume__()
        {
            SetAlpha(1);
        }

        protected override void __OnSetFrame__(Sprite sprite)
        {
            m_Image.sprite = sprite;
        }

        private void SetAlpha(float value)
        {
            var color = m_Image.color;
            color.a = value;
            m_Image.color = color;
        }

    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// [RequireComponent(typeof(Image))]
// public class icon_anime_v2_ImagePlayer : MonoBehaviour
// {

//     public float Speed { get; private set; } = 1;
//     public bool IsLoop { get; private set; } = false;
//     public bool IsPlaying { get; private set; } = false;

//     [SerializeField]
//     public bool m_PlayAutomatically;

//     [SerializeField]
//     private icon_anime_v2_clip m_icon_anime_v2_clip;

//     [SerializeField]
//     private Image m_Image;

//     private float m_ElapsedTime = 0;

//     private void Awake()
//     {
//         SetClip(m_icon_anime_v2_clip);
//         SetLoop(false);
//     }

//     private void Start()
//     {
//         if (m_PlayAutomatically) Play();
//     }

//     public void SetClip(icon_anime_v2_clip icon_anime_v2_clip)
//     {
//         this.m_icon_anime_v2_clip = icon_anime_v2_clip;
//     }

//     public void SetSpeed(float value)
//     {
//         this.Speed = value;
//     }

//     public void SetLoop(bool value)
//     {
//         this.IsLoop = value;
//     }

//     public void Play(int atFrameCount = 0)
//     {
//         var color = m_Image.color;
//         color.a = 1;
//         m_Image.color = color;

//         m_ElapsedTime = FrameToDruation(atFrameCount);
//         IsPlaying = true;
//         SetFrame(atFrameCount);
//     }

//     private float FrameToDruation(int frameCount)
//     {
//         return frameCount * (1.0F / icon_anime_v2_clip.FPS);
//     }

//     public void Resume()
//     {
//         IsPlaying = true;
//     }

//     public void Pause()
//     {
//         IsPlaying = false;
//     }

//     public void Stop()
//     {
//         IsPlaying = false;
//         m_ElapsedTime = 0;
//         m_Image.sprite = null;
//         var color = m_Image.color;
//         color.a = 0;
//         m_Image.color = color;

//     }


//     void Update()
//     {
//         /* 再生状態でなければ */
//         if (IsPlaying == false) return;

//         /* クリップが設定されていなければ */
//         if (m_icon_anime_v2_clip == null) return;

//         m_ElapsedTime += Time.deltaTime;
//         var totalFrameCount = Mathf.FloorToInt(m_ElapsedTime * icon_anime_v2_clip.FPS * Speed);

//         /* 再生終了判定 */
//         var overFrameCount = this.m_icon_anime_v2_clip.frameCount <= totalFrameCount;
//         if (overFrameCount && IsLoop == false)
//         {
//             IsPlaying = false;
//             return;
//         }

//         var frameCount = totalFrameCount % m_icon_anime_v2_clip.frameCount;
//         // 補間文字列を使用して0埋め後の文字列を生成
//         SetFrame(frameCount);
//     }

//     private void SetFrame(int frameCount)
//     {
//         var sprite = m_icon_anime_v2_clip.GetFrame(frameCount);
//         m_Image.sprite = sprite;
//         Debug.Log($"Change Sprite frameCount:{frameCount} sprite:{sprite}");
//     }
// }
