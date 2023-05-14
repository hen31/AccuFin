namespace AccuFin.Data.Entities
{
    public abstract class BaseEntity<T>
    {
        internal BaseEntity()
        {

        }

        public T Id { get; set; }

    }

    public abstract class BaseEntityNumericId : BaseEntity<int>
    {

    }

    public abstract class BaseEntityGuidId : BaseEntity<Guid>
    {

    }
}