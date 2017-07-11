﻿using System;
using Android.App;
using Android.Content;
using Android.Widget;
using NSUtils.Interfaces;
using Android.Text;
using Plugin.CurrentActivity;
using System.Threading.Tasks;
using System.Threading;

namespace NSUtils.Droid.Service
{
    public class DroidDialog : Java.Lang.Object, IDialog, DatePickerDialog.IOnDateSetListener
    {
        private Action<DateTime> _onDateSelect = delegate { };
        public void ShowAlert(string title, string message = "")
        {
            new AlertDialog.Builder(CrossCurrentActivity.Current.Activity)
                .SetTitle(title)
                .SetMessage(message)
                .Show();
        }

        public void ShowCalendar(string title, Action<DateTime> callbackDate)
        {
            _onDateSelect = callbackDate;
            var currently = DateTime.Now;
            var dialog = new DatePickerDialog(CrossCurrentActivity.Current.Activity, this, 
                currently.Year, currently.Month, currently.Day);
            dialog.SetTitle(title);
            dialog.Show();
        }

        public void ShowInput(string title, Action<string> callbackInput, string buttonText = "OK")
        {
            var builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
            builder.SetTitle(title);

            var inputText = new EditText(CrossCurrentActivity.Current.Activity)
            {
                InputType = InputTypes.ClassText | InputTypes.TextVariationPassword
            };
            builder.SetView(inputText);

            builder.SetPositiveButton(buttonText, (args, index) =>
            {
                callbackInput.Invoke(inputText.Text);
            });

            builder.Show();
        }

        public void ShowLoading(Action waitedAction, int timeout = -1)
        {
            Task task = ExecuteTask(waitedAction, timeout);
            var dialog = new ProgressDialog(CrossCurrentActivity.Current.Activity)
            {
                Indeterminate = true
            };
            dialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            dialog.Show();
            while (!(task.IsCompleted | task.IsFaulted));
            dialog.Hide();
            if (task.IsFaulted)
                throw new TimeoutException();
        }

        public void ShowSelection(string title, string[] options, Action<string> callbackSelection)
        {
            new AlertDialog.Builder(CrossCurrentActivity.Current.Activity)
                .SetTitle(title)
                .SetItems(options, new EventHandler<DialogClickEventArgs>((s, e) =>
                {
                    callbackSelection.Invoke(options[e.Which]);
                })).Show();
        }

        public void ShowSelection<T>(string title, T[] options, string[] optionsShowed, Action<T> callbackSelection)
        {
            new AlertDialog.Builder(CrossCurrentActivity.Current.Activity)
               .SetTitle(title)
               .SetItems(optionsShowed, new EventHandler<DialogClickEventArgs>((s, e) =>
               {
                   callbackSelection.Invoke(options[e.Which]);
               })).Show();
        }

        private async Task ExecuteTask(Action waitedAction, int timeout = -1)
        {
            var token = new CancellationTokenSource();
            var task = Task.Run(waitedAction, token.Token);
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                await task;
            }
            else
            {
                token.Cancel();
                throw new TimeoutException();
            }
        }

        public void OnDateSet(DatePicker view, int year, int month, int day)
        {
            DateTime selectedDate = new DateTime(year, month + 1, day);
            _onDateSelect(selectedDate);
        }
    }
}