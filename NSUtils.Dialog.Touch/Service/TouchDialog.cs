using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSUtils.Interfaces;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Threading;

namespace NSUtils.Touch.Service
{
    class TouchDialog : IDialog
    {
        public void ShowAlert(string title, string message = "")
        {
            var dialog = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            dialog.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            ActualController().PresentViewController(dialog, true, null);
        }

        public void ShowCalendar(string title, Action<DateTime> callbackDate)
        {
            throw new NotImplementedException();
        }

        public void ShowInput(string title, Action<string> callbackInput, string buttonText = "OK")
        {
            var dialog = UIAlertController.Create(title, "", UIAlertControllerStyle.Alert);
            UITextField field = null;
            dialog.AddTextField(textField => 
            {
                field = textField;
            });
            dialog.AddAction(UIAlertAction.Create(buttonText, UIAlertActionStyle.Default, (actionOk) 
                => callbackInput.Invoke(field.Text)));
            ActualController().PresentViewController(dialog, true, null);
        }

        public void ShowLoading(Action waitedAction, int timeout = -1)
        {
            Task task = ExecuteTask(waitedAction, timeout);
            var dialog = UIAlertController.Create("", "", UIAlertControllerStyle.Alert);
            var spinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge)
            {
                HidesWhenStopped = true,
                Color = UIColor.Black,
                Center = new CoreGraphics.CGPoint(130.5, 65.5)
            };
            spinner.StartAnimating();
            dialog.Add(spinner);
            ActualController().PresentViewController(dialog, true, null);
            while (!(task.IsCompleted | task.IsFaulted)) ;
            spinner.StopAnimating();
            if (task.IsFaulted)
                throw new TimeoutException();
        }

        public void ShowSelection(string title, string[] options, Action<string> callbackSelection)
        {
            UIAlertController dialog = UIAlertController.Create(title, "", UIAlertControllerStyle.ActionSheet);

            foreach (var option in options)
                dialog.AddAction(UIAlertAction.Create(option,UIAlertActionStyle.Default, (action) => callbackSelection.Invoke(option)));

            UIPopoverPresentationController iPadDialog = dialog.PopoverPresentationController;
            if (iPadDialog != null)
            {
                iPadDialog.SourceView = ActualController().View;
                iPadDialog.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }
            ActualController().PresentViewController(dialog, true, null);
        }

        public void ShowSelection<T>(string title, T[] options, string[] optionsShowed, Action<T> callbackSelection)
        {
            UIAlertController dialog = UIAlertController.Create(title, "", UIAlertControllerStyle.ActionSheet);
            var t = options.GetEnumerator();
            foreach(var option in options.Zip(optionsShowed, Tuple.Create))
                dialog.AddAction(UIAlertAction.Create(option.Item2, UIAlertActionStyle.Default, (action) => callbackSelection.Invoke(option.Item1)));

            UIPopoverPresentationController iPadDialog = dialog.PopoverPresentationController;
            if (iPadDialog != null)
            {
                iPadDialog.SourceView = ActualController().View;
                iPadDialog.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }
            ActualController().PresentViewController(dialog, true, null);
        }

        private UIViewController ActualController()
        {
            var controller = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (controller.PresentedViewController != null)
                controller = controller.PresentedViewController;
            return controller;
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
    }
}