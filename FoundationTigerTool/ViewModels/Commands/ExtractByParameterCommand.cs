namespace FoundationTigerTool.ViewModels.Commands
{
    internal class ExtractByParameterCommand : AbstractParameterCommand
    {
        protected override void DoSpecificWork()
        {
            OneTimeRunViewModel.Extract();
        }
    }
}