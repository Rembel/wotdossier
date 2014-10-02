using System.ComponentModel;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    public class Reporter : IReporter
    {
        private readonly BackgroundWorker _worker;
        private readonly ProgressControlViewModel _progressView;

        public Reporter(BackgroundWorker worker, ProgressControlViewModel progressView)
        {
            _worker = worker;
            _progressView = progressView;
        }

        public void Report(int percentProgress, string format, params object[] arg)
        {
            _progressView.Report(_worker, percentProgress, Resources.Resources.Progress_DataLoadCompleted);
        }
    }
}