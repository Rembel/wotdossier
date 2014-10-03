using System.ComponentModel;
using WotDossier.Framework.Forms.ProgressDialog;

namespace WotDossier.Applications.ViewModel
{
    public class Reporter : IReporter
    {
        private readonly BackgroundWorker _worker;
        private readonly DoWorkEventArgs _eventArgs;
        private readonly ProgressControlViewModel _progressView;

        public Reporter(BackgroundWorker worker, DoWorkEventArgs eventArgs, ProgressControlViewModel progressView)
        {
            _worker = worker;
            _eventArgs = eventArgs;
            _progressView = progressView;
        }

        public void Report(int percentProgress, string format, params object[] arg)
        {
            _progressView.ReportWithCancellationCheck(_worker, _eventArgs, percentProgress, string.Format(format, arg));
        }
    }
}