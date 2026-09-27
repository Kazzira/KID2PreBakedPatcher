namespace KID2PreBakedPatcher.Core.KID;




public class Line
{
    public required LineColumns.Keyword    Keyword     { get; init; }
    public required LineColumns.RecordType RecordType  { get; init; }
    public required LineColumns.Filter     Filter      { get; init; }
    public          LineColumns.Trait?     Trait       { get; init; }
    public          LineColumns.Chance?    Chance      { get; init; }
}