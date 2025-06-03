namespace Domain.Common
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        // Métodos para obtener las propiedades que componen el objeto de valor.
        // Las clases que hereden de ValueObject deben implementar este método.
        protected abstract IEnumerable<object> GetEqualityComponents();

        // Implementación de la igualdad basada en los componentes (propiedades) del objeto de valor.
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            var valueObject = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            // Un buen hashCode para objetos de valor es una combinación de los hash de sus componentes.
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y); // Usa XOR para combinar los hash codes.
        }

        // Implementación de la interfaz IEquatable<ValueObject> para tipado fuerte.
        public bool Equals(ValueObject? other)
        {
            return Equals((object?)other);
        }

        // Sobrecarga de operadores de igualdad y desigualdad.
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
