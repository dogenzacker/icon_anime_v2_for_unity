namespace icon_anime_v2
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpritePlayer : BasePlayer
    {
        private SpriteRenderer m_SpriteRenderer;
        protected override void __OnAwake__()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        protected override void __OnStop__()
        {
            m_SpriteRenderer.sprite = null;
        }
        protected override void __OnSetFrame__(Sprite sprite)
        {
            m_SpriteRenderer.sprite = sprite;
        }
    }
}