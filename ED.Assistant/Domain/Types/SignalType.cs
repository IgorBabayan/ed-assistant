namespace ED.Assistant.Domain.Types;

public sealed class SignalType : IEquatable<SignalType>, IEquatable<string>
{
	public static readonly SignalType Biological = new("$SAA_SignalType_Biological;");
	public static readonly SignalType Geological = new("$SAA_SignalType_Geological;");
	public static readonly SignalType Human = new("$SAA_SignalType_Human;");
	public static readonly SignalType Xeno = new("Xeno");

	private readonly string? _value;

	public SignalType() { }

	private SignalType(string value) => _value = value;

	public static bool operator ==(SignalType? left, SignalType? right)
	{
		if (left is null)
			return right is null;

		return left.Equals(right);
	}

	public static bool operator !=(SignalType? left, SignalType? right) => !(left == right);

	public static bool operator ==(SignalType? left, string? right) => left?.Equals(right) ?? right is null;

	public static bool operator !=(SignalType? left, string? right) => !(left == right);

	public static bool operator ==(string? left, SignalType? right) => right == left;

	public static bool operator !=(string? left, SignalType? right) => !(left == right);

	public static explicit operator string(SignalType scanType) => scanType._value!;

	public override string ToString() => _value!;

	public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_value!);

	public override bool Equals(object? obj) => obj switch
	{
		SignalType other => Equals(other),
		string str => Equals(str),
		_ => false
	};

	public bool Equals(SignalType? other)
	{
		if (other is null)
			return false;

		if (ReferenceEquals(this, other))
			return true;

		return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
	}

	public bool Equals(string? other) => string.Equals(_value, other, StringComparison.Ordinal);
}
