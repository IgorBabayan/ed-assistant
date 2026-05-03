namespace ED.Assistant.Data.Types;

public sealed class ParentType : IEquatable<ParentType>, IEquatable<string>
{
	public static readonly ParentType Null = new("Null");

	private readonly string _value;

	public ParentType(string value) => _value = value;

	public static bool operator ==(ParentType? left, ParentType? right)
	{
		if (left is null)
			return right is null;

		return left.Equals(right);
	}

	public static bool operator !=(ParentType? left, ParentType? right) => !(left == right);

	public static bool operator ==(ParentType? left, string? right) => left?.Equals(right) ?? right is null;

	public static bool operator !=(ParentType? left, string? right) => !(left == right);

	public static bool operator ==(string? left, ParentType? right) => right == left;

	public static bool operator !=(string? left, ParentType? right) => !(left == right);

	public override string ToString() => _value;

	public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_value);

	public override bool Equals(object? obj) => obj switch
	{
		ParentType other => Equals(other),
		string str => Equals(str),
		_ => false
	};

	public bool Equals(ParentType? other)
	{
		if (other is null)
			return false;

		if (ReferenceEquals(this, other))
			return true;

		return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
	}

	public bool Equals(string? other) => string.Equals(_value, other, StringComparison.Ordinal);
}
