namespace IpJournal.UI.Contracts;

public sealed record FilterOptionsDTO(
    string TimeStart,
    string TimeEnd,
    string? AddressStart = null,
    int? AddressMask = null
);