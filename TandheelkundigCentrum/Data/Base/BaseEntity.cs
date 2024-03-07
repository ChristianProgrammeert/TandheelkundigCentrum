namespace Pravis.Data.Base;

public interface IBaseEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    public TIdentifier Id { get; set; }
}