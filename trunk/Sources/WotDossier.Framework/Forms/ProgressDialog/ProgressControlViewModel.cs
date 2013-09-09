﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WotDossier.Framework.Forms.ProgressDialog
{
    public class ProgressControlViewModel : INotifyPropertyChanged
    {
        volatile bool _isBusy;
        BackgroundWorker _worker;

        private string _subLabel;
        private bool _isCancelEnabled;
        private double _progressBarValue;

        public bool IsCancelEnabled
        {
            get { return _isCancelEnabled; }
            set
            {
                _isCancelEnabled = value;
                OnPropertyChanged("IsCancelEnabled");
            }
        }

        public string SubLabel
        {
            get { return _subLabel; }
            set
            {
                _subLabel = value;
                OnPropertyChanged("SubLabel");
            }
        }

        public double ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
            }
        }

        public ProgressDialogResult Execute(object operation)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");

            ProgressDialogResult result = null;

            _isBusy = true;

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;

            _worker.DoWork +=
                (s, e) =>
                {
                    if (operation is Action)
                        ((Action)operation)();
                    else if (operation is Action<BackgroundWorker>)
                        ((Action<BackgroundWorker>)operation)(s as BackgroundWorker);
                    else if (operation is Action<BackgroundWorker, DoWorkEventArgs>)
                        ((Action<BackgroundWorker, DoWorkEventArgs>)operation)(s as BackgroundWorker, e);
                    else if (operation is Func<object>)
                        e.Result = ((Func<object>)operation)();
                    else if (operation is Func<BackgroundWorker, object>)
                        e.Result = ((Func<BackgroundWorker, object>)operation)(s as BackgroundWorker);
                    else if (operation is Func<BackgroundWorker, DoWorkEventArgs, object>)
                        e.Result = ((Func<BackgroundWorker, DoWorkEventArgs, object>)operation)(s as BackgroundWorker, e);
                    else
                        throw new InvalidOperationException("Operation type is not supoorted");
                };

            _worker.RunWorkerCompleted +=
                (s, e) =>
                {
                    result = new ProgressDialogResult(e);
                    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                    {
                        _isBusy = false;
                        //Close();
                    }, null);
                };

            _worker.ProgressChanged +=
                (s, e) =>
                {
                    if (!_worker.CancellationPending)
                    {
                        SubLabel = (e.UserState as string) ?? string.Empty;
                        ProgressBarValue = e.ProgressPercentage;
                    }
                };

            _worker.RunWorkerAsync();

            //ShowDialog();

            return result;
        }

        void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (_worker != null && _worker.WorkerSupportsCancellation)
            {
                SubLabel = "Please wait while process will be cancelled...";
                IsCancelEnabled = false;
                _worker.CancelAsync();
            }
        }

        void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = _isBusy;
        }

        public ProgressDialogResult Execute(Window owner, string label, Action operation)
        {
            return ExecuteInternal(owner, label, (object)operation, null);
        }

        public ProgressDialogResult Execute(Window owner, string label, Action operation, ProgressDialogSettings settings)
        {
            return ExecuteInternal(owner, label, (object)operation, settings);
        }

        public ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker> operation)
        {
            return ExecuteInternal(owner, label, (object)operation, null);
        }

        public ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker> operation, ProgressDialogSettings settings)
        {
            return ExecuteInternal(owner, label, (object)operation, settings);
        }

        public ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker, DoWorkEventArgs> operation)
        {
            return ExecuteInternal(owner, label, (object)operation, null);
        }

        public ProgressDialogResult Execute(Window owner, string label, Action<BackgroundWorker, DoWorkEventArgs> operation, ProgressDialogSettings settings)
        {
            return ExecuteInternal(owner, label, (object)operation, settings);
        }

        public ProgressDialogResult Execute(Window owner, string label, Func<object> operationWithResult)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, null);
        }

        public ProgressDialogResult Execute(Window owner, string label, Func<object> operationWithResult, ProgressDialogSettings settings)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, settings);
        }

        public ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, object> operationWithResult)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, null);
        }

        public ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, object> operationWithResult, ProgressDialogSettings settings)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, settings);
        }

        public ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, DoWorkEventArgs, object> operationWithResult)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, null);
        }

        public ProgressDialogResult Execute(Window owner, string label, Func<BackgroundWorker, DoWorkEventArgs, object> operationWithResult, ProgressDialogSettings settings)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, settings);
        }

        public void Execute(Window owner, string label, Action operation, Action<ProgressDialogResult> successOperation, Action<ProgressDialogResult> failureOperation = null, Action<ProgressDialogResult> cancelledOperation = null)
        {
            ProgressDialogResult result = ExecuteInternal(owner, label, operation, null);

            if (result.Cancelled && cancelledOperation != null)
                cancelledOperation(result);
            else if (result.OperationFailed && failureOperation != null)
                failureOperation(result);
            else if (successOperation != null)
                successOperation(result);
        }

        internal ProgressDialogResult ExecuteInternal(Window owner, string label, object operation, ProgressDialogSettings settings)
        {
            //ProgressDialog dialog = new ProgressDialog(settings);
            //dialog.Owner = owner;

            //if (!string.IsNullOrEmpty(label))
            //    dialog.Label = label;

            return Execute(operation);
        }

        public bool CheckForPendingCancellation(BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (worker.WorkerSupportsCancellation && worker.CancellationPending)
                e.Cancel = true;

            return e.Cancel;
        }

        public void Report(BackgroundWorker worker, string message)
        {
            if (worker.WorkerReportsProgress)
                worker.ReportProgress(0, message);
        }

        public void Report(BackgroundWorker worker, string format, params object[] arg)
        {
            if (worker.WorkerReportsProgress)
                worker.ReportProgress(0, string.Format(format, arg));
        }

        public void Report(BackgroundWorker worker, int percentProgress, string message)
        {
            if (worker.WorkerReportsProgress)
                worker.ReportProgress(percentProgress, message);
        }

        public void Report(BackgroundWorker worker, int percentProgress, string format, params object[] arg)
        {
            if (worker.WorkerReportsProgress)
                worker.ReportProgress(percentProgress, string.Format(format, arg));
        }

        public bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, string message)
        {
            if (CheckForPendingCancellation(worker, e))
                return true;

            if (worker.WorkerReportsProgress)
                worker.ReportProgress(0, message);

            return false;
        }

        public bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, string format, params object[] arg)
        {
            if (CheckForPendingCancellation(worker, e))
                return true;

            if (worker.WorkerReportsProgress)
                worker.ReportProgress(0, string.Format(format, arg));

            return false;
        }

        public bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, int percentProgress, string message)
        {
            if (CheckForPendingCancellation(worker, e))
                return true;

            if (worker.WorkerReportsProgress)
                worker.ReportProgress(percentProgress, message);

            return false;
        }

        public bool ReportWithCancellationCheck(BackgroundWorker worker, DoWorkEventArgs e, int percentProgress, string format, params object[] arg)
        {
            if (CheckForPendingCancellation(worker, e))
                return true;

            if (worker.WorkerReportsProgress)
                worker.ReportProgress(percentProgress, string.Format(format, arg));

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}