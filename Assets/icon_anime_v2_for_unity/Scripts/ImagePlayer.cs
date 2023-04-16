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