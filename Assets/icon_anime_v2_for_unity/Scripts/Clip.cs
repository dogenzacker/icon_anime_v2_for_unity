namespace icon_anime_v2
{
    using UnityEngine;
    using UnityEngine.U2D;
    public class Clip : ScriptableObject
    {
        [SerializeField]
        private string m_Prefix;

        [SerializeField]
        private int m_FrameCount;

        [SerializeField]
        private float m_FPS;

        [SerializeField]
        private bool m_IsLooping;

        [SerializeField]
        private SpriteAtlas m_SpriteAtlas;

        public bool IsLoopng => m_IsLooping;
        public float FPS => m_FPS;
        public int FrameCount => m_FrameCount;

        public Sprite GetFrame(int frameCount)
        {
            Sprite sprite = m_SpriteAtlas.GetSprite($"{m_Prefix}_{frameCount:D5}");
            return sprite;
        }

        public int width
        {
            get
            {

                return (int)GetFrame(0).rect.width;
            }
        }

        public int height
        {
            get
            {
                return (int)GetFrame(0).rect.height;
            }
        }

        public float length
        {
            get { return m_SpriteAtlas.spriteCount / 30.0F; }
        }

#if UNITY_EDITOR
        public static Clip Generate(SpriteAtlas spriteAtlas, string prefix, float fps, bool isLooping, int frameCount)
        {
            var icon_anime_v2 = ScriptableObject.CreateInstance<Clip>();
            icon_anime_v2.m_SpriteAtlas = spriteAtlas;
            icon_anime_v2.m_Prefix = prefix;
            icon_anime_v2.m_IsLooping = isLooping;
            icon_anime_v2.m_FPS = fps;
            icon_anime_v2.m_FrameCount = frameCount;
            return icon_anime_v2;
        }
#endif
    }
}