namespace WinCtp
{
    public class MainViewImpl
    {
        private IMainView _view;

        public MainViewImpl(IMainView view)
        {
            _view = view;
        }
    }
}