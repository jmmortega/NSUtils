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

namespace NSUtils.Droid.Service
{
    class DroidDialog : IDialog
    {
        public void ShowAlert(string title, string message = "")
        {
            new AlertDialog.Builder(CrossCurrentActivity.Current.Activity)
                .SetTitle(title)
                .SetMessage(message)
                .Show();
        }

        public void ShowCalendar(string title, Action<DateTime> callbackDate)
        {
            throw new NotImplementedException();
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

        public void ShowLoading(string title, Action waitedAction)
        {
            throw new NotImplementedException();
        }

        public void ShowSelection(string title, string[] options, Action<string> callbackSelection)
        {
            throw new NotImplementedException();
        }

        public void ShowSelection<T>(string title, T[] options, string[] optionsShowed, Action<T> callbackSelection)
        {
            throw new NotImplementedException();
        }
    }
}