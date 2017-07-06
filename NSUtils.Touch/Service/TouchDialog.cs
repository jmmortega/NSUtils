using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSUtils.Interfaces;
using Foundation;
using UIKit;

namespace NSUtils.Touch.Service
{
    class TouchDialog : IDialog
    {
        public void ShowAlert(string title, string message = "")
        {
            throw new NotImplementedException();
        }

        public void ShowCalendar(string title, Action<DateTime> callbackDate)
        {
            throw new NotImplementedException();
        }

        public void ShowInput(string title, Action<string> callbackInput, string buttonText = "OK")
        {
            throw new NotImplementedException();
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