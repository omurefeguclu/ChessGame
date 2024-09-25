namespace Omedya.Scripts.Core.Shared.ModuleSystem
{
    public interface IModule<TOwner>
        where TOwner : class
    {
        void Initialize(TOwner owner);
    }
}