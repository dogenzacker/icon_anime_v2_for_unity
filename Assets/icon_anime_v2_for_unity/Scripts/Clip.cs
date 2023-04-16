namespace icon_anime_v2
{
    using UnityEngine;
    using UnityEngine.U2D;
    public class Clip : ScriptableObject
    {
        [SerializeField]
        private SpriteAtlas m_SpriteAtlas;

        [SerializeField]
        private string m_Prefix;

        [SerializeField]
        private int m_FrameCount;

        [SerializeField]
        private float m_FPS;

        [SerializeField]
        private bool m_IsLooping;

        [SerializeField]
        private float m_Width;

        [SerializeField]
        private float m_Height;

        [SerializeField]
        private float m_Length;

        public bool IsLooping => m_IsLooping;
        public float FPS => m_FPS;
        public int FrameCount => m_FrameCount;
        public float Width => m_Width;
        public float Height => m_Height;
        public float Length => m_Length;

        private string[] m_ListKeys;

        void Awake()
        {
            m_ListKeys = new string[FrameCount];
            for (var frameCount = 0; frameCount < FrameCount; frameCount++) m_ListKeys[frameCount] = $"{m_Prefix}_{frameCount:D5}";
        }

        void OnDestroy()
        {
            m_ListKeys = null;
        }

        public Sprite GetFrame(int frameCount) => m_SpriteAtlas.GetSprite(m_ListKeys[frameCount]);

#if UNITY_EDITOR
        public static Clip Generate(SpriteAtlas spriteAtlas, string prefix, float fps, bool isLooping, int frameCount, float width, float height, float length)
        {
            var icon_anime_v2 = ScriptableObject.CreateInstance<Clip>();
            icon_anime_v2.m_SpriteAtlas = spriteAtlas;
            icon_anime_v2.m_Prefix = prefix;
            icon_anime_v2.m_IsLooping = isLooping;
            icon_anime_v2.m_FPS = fps;
            icon_anime_v2.m_FrameCount = frameCount;
            icon_anime_v2.m_Width = width;
            icon_anime_v2.m_Height = height;
            icon_anime_v2.m_Length = length;
            return icon_anime_v2;
        }
#endif
    }
}