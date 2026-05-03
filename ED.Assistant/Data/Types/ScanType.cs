namespace ED.Assistant.Data.Types;

public sealed class ScanType : IEquatable<ScanType>, IEquatable<string>
{
	public static readonly ScanType AutoScan = new("AutoScan");
	public static readonly ScanType Detailed = new("Detailed");

	private readonly string _value;

	private ScanType(string value) => _value = value;

	public static bool operator ==(ScanType? left, ScanType? right)
	{
		if (left is null)
			return right is null;

		return left.Equals(right);
	}

	public static bool operator !=(ScanType? left, ScanType? right) => !(left == right);

	public static bool operator ==(ScanType? left, string? right) => left?.Equals(right) ?? right is null;

	public static bool operator !=(ScanType? left, string? right) => !(left == right);

	public static bool operator ==(string? left, ScanType? right) => right == left;

	public static bool operator !=(string? left, ScanType? right) => !(left == right);

	public static explicit operator string(ScanType scanType) => scanType._value;

	public override string ToString() => _value;

	public override int GetHashCode()  => StringComparer.OrdinalIgnoreCase.GetHashCode(_value);

	public override bool Equals(object? obj) => obj switch
	{
		ScanType other => Equals(other),
		string str => Equals(str),
		_ => false
	};

	public bool Equals(ScanType? other)
	{
		if (other is null)
			return false;

		if (ReferenceEquals(this, other))
			return true;

		return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
	}

	public bool Equals(string? other) => string.Equals(_value, other, StringComparison.Ordinal);
}
