namespace KID2PreBakedPatcher.Core.KID;


public class Database
{
    private IFileParser         FileParser { get; init; }
    private IEnumerable<string> FilePaths  { get; init; }
    private IEnumerable<Line>   Lines      { get; set; } = Enumerable.Empty<Line>();

    public Dictionary<Enums.Type, List<Line>> LinesByType => Lines.GroupBy(line => line.RecordType.Type)
                                                .ToDictionary(group => group.Key, group => group.ToList());

    public Database(IFileParser fileParser, IEnumerable<string> filePaths)
    {
        FileParser = fileParser;
        FilePaths  = filePaths;

        // This constructor can throw.

        foreach (var filePath in FilePaths)
        {
            var lines = FileParser.ParseFile(filePath);
            Lines = Lines.Concat(lines);
        }
    }


}