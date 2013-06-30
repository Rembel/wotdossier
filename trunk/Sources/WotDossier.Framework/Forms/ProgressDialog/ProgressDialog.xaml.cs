//
// Parago Media GmbH & Co. KG, Jürgen Bäurle (jbaurle@parago.de)
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WotDossier.Framework.Forms.ProgressDialog
{
	public partial class ProgressDialog : Window
	{
		volatile bool _isBusy;
		BackgroundWorker _worker;

		public string Label
		{
			get { return TextLabel.Text; }
			set { TextLabel.Text = value; }
		}

		public string SubLabel
		{
			get { return SubTextLabel.Text; }
			set { SubTextLabel.Text = value; }
		}

		internal ProgressDialogResult Result { get; private set; }

		public ProgressDialog(ProgressDialogSettings settings)
		{
			InitializeComponent();

			double top;
			double right;

			if(settings == null)
				settings = ProgressDialogSettings.WithLabelOnly;

			if(settings.ShowSubLabel)
			{
				top = 38;
				Height = 110;
				SubTextLabel.Visibility = Visibility.Visible;
			}
			else
			{
				top = 22;
				Height = 100;
				SubTextLabel.Visibility = Visibility.Collapsed;
			}

			if(settings.ShowCancelButton)
			{
				right = 74;
				CancelButton.Visibility = Visibility.Visible;
			}
			else
			{
				right = 0;
				CancelButton.Visibility = Visibility.Collapsed;
			}

			ProgressBar.Margin = new Thickness(0, top, right, 0);
			ProgressBar.IsIndeterminate = settings.ShowProgressBarIndeterminate;
		}

		public ProgressDialogResult Execute(object operation)
		{
			if(operation == null)
				throw new ArgumentNullException("operation");

			ProgressDialogResult result = null;

			_isBusy = true;

			_worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.WorkerSupportsCancellation = true;

			_worker.DoWork +=
				(s, e) => {
					if(operation is Action)
						((Action)operation)();
					else if(operation is Action<BackgroundWorker>)
						((Action<BackgroundWorker>)operation)(s as BackgroundWorker);
					else if(operation is Action<BackgroundWorker, DoWorkEventArgs>)
						((Action<BackgroundWorker, DoWorkEventArgs>)operation)(s as BackgroundWorker, e);
					else if(operation is Func<object>)
						e.Result = ((Func<object>)operation)();
					else if(operation is Func<BackgroundWorker, object>)
						e.Result = ((Func<BackgroundWorker, object>)operation)(s as BackgroundWorker);
					else if(operation is Func<BackgroundWorker, DoWorkEventArgs, object>)
						e.Result = ((Func<BackgroundWorker, DoWorkEventArgs, object>)operation)(s as BackgroundWorker, e);
					else
						throw new InvalidOperationException("Operation type is not supoorted");
				};

			_worker.RunWorkerCompleted +=
				(s, e) => {
					result = new ProgressDialogResult(e);
					Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate {
						_isBusy = false;
						Close();
					}, null);
				};

			_worker.ProgressChanged +=
				(s, e) => {
					if(!_worker.CancellationPending)
					{
						SubLabel = (e.UserState as string) ?? string.Empty;
						ProgressBar.Value = e.ProgressPercentage;
					}
				};

			_worker.RunWorkerAsync();

			ShowDialog();

			return result;
		}

		void OnCancelButtonClick(object sender, RoutedEventArgs e)
		{
			if(_worker != null && _worker.WorkerSupportsCancellation)
			{
				SubLabel = "Please wait while process will be cancelled...";
				CancelButton.IsEnabled = false;
				_worker.CancelAsync();
			}
		}

		void OnClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = _isBusy;
		}

        public static ProgressDialogResult Execute(Window owner, string label, Action operation)
		{
			return ExecuteInternal(owner, label, (object)operation, null);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Action operation, ProgressDialogSettings settings)
		{
			return ExecuteInternal(owner, label, (object)operation, settings);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker> operation)
		{
			return ExecuteInternal(owner, label, (object)operation, null);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker> operation, ProgressDialogSettings settings)
		{
			return ExecuteInternal(owner, label, (object)operation, settings);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker, DoWorkEventArgs> operation)
		{
			return ExecuteInternal(owner, label, (object)operation, null);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker, DoWorkEventArgs> operation, ProgressDialogSettings settings)
		{
			return ExecuteInternal(owner, label, (object)operation, settings);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Func<object> operationWithResult)
		{
			return ExecuteInternal(owner, label, (object)operationWithResult, null);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Func<object> operationWithResult, ProgressDialogSettings settings)
		{
			return ExecuteInternal(owner, label, (object)operationWithResult, settings);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, object> operationWithResult)
		{
			return ExecuteInternal(owner, label, (object)operationWithResult, null);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, object> operationWithResult, ProgressDialogSettings settings)
		{
			return ExecuteInternal(owner, label, (object)operationWithResult, settings);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, DoWorkEventArgs, object> operationWithResult)
		{
			return ExecuteInternal(owner, label, (object)operationWithResult, null);
		}

        public static ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, DoWorkEventArgs, object> operationWithResult, ProgressDialogSettings settings)
		{
			return ExecuteInternal(owner, label, (object)operationWithResult, settings);
		}

        public static void Execute(Window owner, string label, Action operation, Action<ProgressDialogResult> successOperation, Action<ProgressDialogResult> failureOperation = null, Action<ProgressDialogResult> cancelledOperation = null)
		{
			ProgressDialogResult result = ExecuteInternal(owner, label, operation, null);

			if(result.Cancelled && cancelledOperation != null)
				cancelledOperation(result);
			else if(result.OperationFailed && failureOperation != null)
				failureOperation(result);
			else if(successOperation != null)
				successOperation(result);
		}

		internal static ProgressDialogResult ExecuteInternal(Window owner, string label, object operation, ProgressDialogSettings settings)
		{
			ProgressDialog dialog = new ProgressDialog(settings);
			dialog.Owner = owner;

			if(!string.IsNullOrEmpty(label))
				dialog.Label = label;

			return dialog.Execute(operation);
		}

        public static bool CheckForPendingCancellation(BackgroundWorker worker, DoWorkEventArgs e)
		{
			if(worker.WorkerSupportsCancellation && worker.CancellationPending)
				e.Cancel = true;

			return e.Cancel;
		}

        public static void Report(BackgroundWorker worker, string message)
		{
			if(worker.WorkerReportsProgress)
				worker.ReportProgress(0, message);
		}

        public static void Report(BackgroundWorker worker, string format, params object[] arg)
		{
			if(worker.WorkerReportsProgress)
				worker.ReportProgress(0, string.Format(format, arg));
		}

        public static void Report(BackgroundWorker worker, int percentProgress, string message)
		{
			if(worker.WorkerReportsProgress)
				worker.ReportProgress(percentProgress, message);
		}

        public static void Report(BackgroundWorker worker, int percentProgress, string format, params object[] arg)
		{
			if(worker.WorkerReportsProgress)
				worker.ReportProgress(percentProgress, string.Format(format, arg));
		}

        public static bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, string message)
		{
			if(CheckForPendingCancellation(worker, e))
				return true;

			if(worker.WorkerReportsProgress)
				worker.ReportProgress(0, message);

			return false;
		}

        public static bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, string format, params object[] arg)
		{
			if(CheckForPendingCancellation(worker, e))
				return true;

			if(worker.WorkerReportsProgress)
				worker.ReportProgress(0, string.Format(format, arg));

			return false;
		}

        public static bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, int percentProgress, string message)
		{
			if(CheckForPendingCancellation(worker, e))
				return true;

			if(worker.WorkerReportsProgress)
				worker.ReportProgress(percentProgress, message);

			return false;
		}

        public static bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, int percentProgress, string format, params object[] arg)
		{
			if(CheckForPendingCancellation(worker, e))
				return true;

			if(worker.WorkerReportsProgress)
				worker.ReportProgress(percentProgress, string.Format(format, arg));

			return false;
		}
	}
}
