namespace ED.Assistant.Data.Types;

public sealed class BodyType : IEquatable<BodyType>, IEquatable<string>
{
	private readonly string _value;

	public BodyType(string value) => _value = value;

	public static bool operator ==(BodyType? left, BodyType? right)
	{
		if (left is null)
			return right is null;

		return left.Equals(right);
	}

	public static bool operator !=(BodyType? left, BodyType? right) => !(left == right);

	public static bool operator ==(BodyType? left, string? right) => left?.Equals(right) ?? right is null;

	public static bool operator !=(BodyType? left, string? right) => !(left == right);

	public static bool operator ==(string? left, BodyType? right) => right == left;

	public static bool operator !=(string? left, BodyType? right) => !(left == right);

	public override string ToString() => _value;

	public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_value);

	public override bool Equals(object? obj) => obj switch
	{
		BodyType other => Equals(other),
		string str => Equals(str),
		_ => false
	};

	public bool Equals(BodyType? other)
	{
		if (other is null)
			return false;

		if (ReferenceEquals(this, other))
			return true;

		return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
	}

	public bool Equals(string? other) => string.Equals(_value, other, StringComparison.Ordinal);
}
