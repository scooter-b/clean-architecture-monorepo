namespace Shared.Core.Abstractions
{
    /// <summary>
    /// </summary>
    public interface IReadWriteRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class
    {

    }
}
