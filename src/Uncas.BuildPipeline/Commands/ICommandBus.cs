namespace Uncas.BuildPipeline.Commands
{
    public interface ICommandBus
    {
        void Publish(ICommand updateGitMirrors);
    }
}