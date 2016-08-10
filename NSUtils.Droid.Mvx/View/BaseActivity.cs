
using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using MvvmCross.Core.ViewModels;
using NSUtils.Mvx.ViewModels;

namespace NSUtils.Droid.Mvx.View
{
    [Activity()]
    [MvxViewFor(typeof(BaseViewModel))]
    public class BaseActivity : MvxActivity
    {
        public BaseViewModel BaseVM
        {
            get
            {
                return (BaseViewModel)ViewModel;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
        }

        protected override void OnResume()
        {
            BaseVM.CommandOnResume.Execute();
            base.OnResume();
        }

        protected override void OnPause()
        {
            BaseVM.CommandOnPause.Execute();
            base.OnPause();
        }

        protected override void OnStop()
        {
            BaseVM.CommandOnStop.Execute();
            base.OnStop();
        }
    }


}