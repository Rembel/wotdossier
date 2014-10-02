namespace WotDossier.Applications.ViewModel
{
    public interface IReporter
    {
        void Report(int percentProgress, string format, params object[] arg);
    }
}