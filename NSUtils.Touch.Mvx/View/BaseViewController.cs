using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using NSUtils.Mvx.ViewModels;

namespace NSUtils.Touch.Mvx.View
{
    [MvxViewFor(typeof(BaseViewModel))]
    public class BaseViewController : MvxViewController
    {
        public BaseViewModel BaseVM
        {
            get
            {
                return (BaseViewModel)ViewModel;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            BaseVM.CommandOnResume.Execute();
            base.ViewDidAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            BaseVM.CommandOnPause.Execute();
            base.ViewDidDisappear(animated);
        }

        public override void ViewDidUnload()
        {
            BaseVM.CommandOnStop.Execute();
            base.ViewDidUnload();
        }
    }
}
