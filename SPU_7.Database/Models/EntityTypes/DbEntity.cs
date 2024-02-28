namespace SPU_7.Database.Models.EntityTypes;

public abstract class DbEntity<TIdType>
{
    public TIdType Id { get; set; }

    public DateTime CreateTime { get; set; }

    public bool Deleted { get; set; }

    private object Actual => this;

    protected abstract bool IsEmpty(TIdType id);

    public override bool Equals(object obj)
    {
        if (obj is not DbEntity<TIdType> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Actual.GetType() != other.Actual.GetType()) return false;
        if (IsEmpty(Id) || IsEmpty(other.Id)) return false;
        return Id.Equals(other.Id);
    }

    public static bool operator ==(DbEntity<TIdType> a, DbEntity<TIdType> b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(DbEntity<TIdType> a, DbEntity<TIdType> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (Actual.GetType().ToString() + Id).GetHashCode();
    }
}