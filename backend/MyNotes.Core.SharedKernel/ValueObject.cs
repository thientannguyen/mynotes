using System.Diagnostics.CodeAnalysis;

namespace MyNotes.Core.SharedKernel
{
    using System.Collections.Generic;
    using System.Linq;

    [ExcludeFromCodeCoverage]
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;

            return !ReferenceEquals(valueObject, null) && EqualsCore(valueObject);
        }

        private bool EqualsCore(ValueObject<T> other) =>
            GetEqualityComponents()
                .SequenceEqual(other.GetEqualityComponents());

        public override int GetHashCode() =>
            GetEqualityComponents()
                .Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));

        protected abstract IEnumerable<object> GetEqualityComponents();

#pragma warning disable S3875 // All subclasses are intended to have value semantics and thus can be compared by == operator.
        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }
    }
}
