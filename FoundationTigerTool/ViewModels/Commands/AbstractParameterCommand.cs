using System;
using System.ComponentModel;

namespace FoundationTigerTool.ViewModels.Commands
{
    abstract internal class AbstractParameterCommand : AbstractWorkerCommand
    {
        protected OneTimeRunViewModel OneTimeRunViewModel;

        public override void Execute(object parameter)
        {
            OneTimeRunViewModel = (OneTimeRunViewModel)parameter;
            Worker.RunWorkerAsync();
        }

        protected override void DoWork(object sender, DoWorkEventArgs e)
        {
            DoSpecificWork();
            if (!OneTimeRunViewModel.HasError)
            {
                OneTimeRunViewModel.OnRequestClose(new EventArgs());
            }
        }

        protected abstract void DoSpecificWork();
    }
}