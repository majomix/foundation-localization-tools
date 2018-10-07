namespace FoundationTigerTool.ViewModels.Commands
{
    internal class ImportByParameterCommand : AbstractParameterCommand
    {
        protected override void DoSpecificWork()
        {
            OneTimeRunViewModel.Import();
        }
    }
}