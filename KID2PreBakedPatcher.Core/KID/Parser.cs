namespace KID2PreBakedPatcher.Core.KID;

using KID2PreBakedPatcher.Core.KID.LineColumns;
using KID2PreBakedPatcher.Core.KID.Enums;

public interface ILineParser
{
    Line? ParseLine(string line);
}

public interface IFileParser
{
    IEnumerable<Line> ParseFile(string filePath);
}


public class LineParser : ILineParser
{
    public Line? ParseLine(string line)
    {
        line = line.Trim();

        if (string.IsNullOrWhiteSpace(line)) return null;
        if (line.StartsWith(';')) return null;

        var equalParts = line.Split('=');

        if (equalParts.Length != 2)
        {
            throw new FormatException($"Line must have exactly one '=' character: {line}");
        }

        equalParts[0] = equalParts[0].Trim();
        equalParts[1] = equalParts[1].Trim();

        if (equalParts[0].Length == 0)
        {
            throw new FormatException($"Line must have a non-empty type column: {line}");
        }

        if (equalParts[1].Length == 0)
        {
            throw new FormatException($"Line must have a non-empty formID column: {line}");
        }

        if (equalParts[0].ToLower() == "keyword")
        {
            return ParseKeywordLine(equalParts[1]);
        }
        else
        {
            Console.WriteLine($"Unknown line type: {equalParts[0]}");
            return null;
        }
    }

    private Line ParseKeywordLine(string line)
    {
        var columns = line.Split('|').Select(c => c.Trim()).Where(c => !string.IsNullOrEmpty(c)).ToList();

        if (columns.Count == 0)
        {
            throw new FormatException($"Keyword line must have at least one column: {line}");
        }

        var keywordColumn = columns[0].ToKeywordColumn();
        var typeEnumValue = columns[1].ToTypeColumn() ?? throw new FormatException($"Invalid type column value: {columns[1]}");
        var typeColumn = new RecordType(typeEnumValue);
        var filterColumn = columns[2].ToColumnFilter(typeEnumValue);

        Trait? traitColumn = null;
        Chance? chanceColumn = null;

        // TEMPORARY, just want to test weapons.
        if (typeEnumValue != Type.Weapon)
        {
            return new Line
            {
                Keyword = keywordColumn,
                RecordType = typeColumn,
                Filter = filterColumn
            };
        }

        if (columns.Count > 3)
        {
            traitColumn = columns[3].ToTraitColumn(typeEnumValue);
        }

        if (columns.Count > 4)
        {
            chanceColumn = new Chance(decimal.Parse(columns[4]));
        }

        return new Line
        {
            Keyword = keywordColumn,
            RecordType = typeColumn,
            Filter = filterColumn,
            Trait = traitColumn,
            Chance = chanceColumn
        };
    }
}

public class FileParser(
    ILineParser _lineParser
) : IFileParser
{

    public IEnumerable<Line> ParseFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parsedLine = _lineParser.ParseLine(line);
            if (parsedLine != null)
            {
                yield return parsedLine;
            }
        }
    }
}