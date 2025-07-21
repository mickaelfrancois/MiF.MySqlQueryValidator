namespace MiF.MySqlQueryValidator.Models;

public class SqlSegment
{
    public string OriginalText { get; set; } = "";

    public string QueryType { get; set; } = "";

    public string QueryCategory { get; set; } = "";

    public List<string> Tables { get; set; } = [];

    public bool IsDDLQuery { get; set; }

    public bool IsNonQuery
    {
        get
        {
            return QueryType.StartsWith("INSERT") || QueryType.StartsWith("UPDATE") || QueryType.StartsWith("DELETE");
        }
    }
}