using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace NSUtils.Mvx.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public virtual ICommand CommandOnResume
        {
            get
            {
                return new MvxCommand(() =>
                {
                    //Nothing to do
                });
            }
        }

        public virtual ICommand CommandOnPause
        {
            get
            {
                return new MvxCommand(() =>
                {

                });
            }
        }

        public virtual ICommand CommandOnStop
        {
            get
            {
                return new MvxCommand(() =>
                {
                    //Nothing to do
                });
            }
        }

        public virtual ICommand CommandClose
        {
            get
            {
                return new MvxCommand(() =>
                {
                    this.Close(this);
                });
            }
        }
    }
}
