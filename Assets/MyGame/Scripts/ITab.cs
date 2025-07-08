namespace MyGame.Scripts
{
    public interface ITab
    {
        TabType TabType { get; }
        void OnShow();
        void OnHide();
    }
}