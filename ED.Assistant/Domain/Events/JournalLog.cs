namespace ED.Assistant.Domain.Events;

public sealed record JournalLogEntry(long Sequence, string RawLine);

/// <summary>
/// Raw journal lines in file order. Bounded, so loading every historical
/// journal on startup doesn't keep hundreds of MB of text in memory.
/// Written by the loader / watcher threads, read by the UI.
/// </summary>
public sealed class JournalLog
{
    public const int DefaultCapacity = 5000;

    private readonly Lock _lock = new();
    private readonly Queue<JournalLogEntry> _entries;

    private long _lastSequence;

    public JournalLog(int capacity = DefaultCapacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);

        Capacity = capacity;
        _entries = new Queue<JournalLogEntry>(capacity);
    }

    public int Capacity { get; }

    public void Append(string rawLine)
    {
        lock (_lock)
        {
            _entries.Enqueue(new JournalLogEntry(++_lastSequence, rawLine));

            if (_entries.Count > Capacity)
                _entries.Dequeue();
        }
    }

    /// <summary>Returns retained entries with <c>Sequence &gt; afterSequence</c>, oldest first.</summary>
    public IReadOnlyList<JournalLogEntry> GetSince(long afterSequence)
    {
        lock (_lock)
        {
            if (_entries.Count == 0 || _lastSequence <= afterSequence)
                return [];

            var firstSequence = _entries.Peek().Sequence;
            var skip = (int)Math.Max(0, afterSequence - firstSequence + 1);

            return [.._entries.Skip(skip)];
        }
    }
}