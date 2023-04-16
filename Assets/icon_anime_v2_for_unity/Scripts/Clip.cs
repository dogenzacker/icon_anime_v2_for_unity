namespace icon_anime_v2
{
    using UnityEngine;
    using UnityEngine.U2D;
    public class Clip : ScriptableObject
    {
        [SerializeField]
        private string prefix;

        [SerializeField]
        private float fps;
        public float FPS => fps;

        [SerializeField]
        public bool isLooping = false;

        [SerializeField]
        private SpriteAtlas m_SpriteAtlas; // Sprite Atlasの参照

        public int frameCount
        {
            get
            {

                Debug.Log("m_SpriteAtlas?:" + m_SpriteAtlas);
                Debug.Log("m_SpriteAtlas.spriteCount?:" + m_SpriteAtlas.spriteCount);
                return m_SpriteAtlas.spriteCount;
            }
        }

        public Sprite GetFrame(int frameCount)
        {
            Sprite sprite = m_SpriteAtlas.GetSprite($"{m_SpriteAtlas.name}{prefix}_{frameCount:D5}");
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
        public static Clip Generate(SpriteAtlas spriteAtlas, string prefix, float fps, bool isLooping)
        {
            var icon_anime_v2 = ScriptableObject.CreateInstance<Clip>();
            icon_anime_v2.m_SpriteAtlas = spriteAtlas;
            icon_anime_v2.prefix = prefix;
            icon_anime_v2.isLooping = isLooping;
            icon_anime_v2.fps = fps;
            return icon_anime_v2;
        }
#endif
    }
}