using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSUtils.Interfaces
{
    public interface IDialog
    {
        void ShowAlert(string title, string message = "");
        void ShowInput(string title, Action<string> callbackInput, string buttonText = "OK");
        void ShowSelection(string title, string[] options, Action<string> callbackSelection);
        void ShowSelection<T>(string title, T[] options, string[] optionsShowed, Action<T> callbackSelection);
        void ShowLoading(Action waitedAction, int timeout = -1);
        void ShowCalendar(string title, Action<DateTime> callbackDate, string buttonText = "OK");
    }
}
