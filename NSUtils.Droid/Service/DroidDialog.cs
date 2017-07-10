using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NSUtils.Interfaces;
using Android.Text;
using Plugin.CurrentActivity;
using System.Threading.Tasks;
using System.Threading;

namespace NSUtils.Droid.Service
{
    public class DroidDialog : IDialog
    {
        public void ShowAlert(string title, string message = "")
        {
            new AlertDialog.Builder(CrossCurrentActivity.Current.Activity)
                .SetTitle(title)
                .SetMessage(message)
                .Show();
        }

        public void ShowCalendar(string title, Action<DateTime> callbackDate, string buttonText = "OK")
        {
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

        public async Task ExecuteTask(Action waitedAction, int timeout = -1)
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
    }
}