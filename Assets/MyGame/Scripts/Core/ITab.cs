namespace MyGame.Scripts.Core
{
    public interface ITab
    {
        TabType TabType { get; }
        void OnShow();
        void OnHide();
    }
}