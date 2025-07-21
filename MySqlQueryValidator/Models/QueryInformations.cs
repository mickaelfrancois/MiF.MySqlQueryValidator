namespace MiF.MySqlQueryValidator.Models;

public class QueryInformations
{
    public bool IsValid { get; set; } = true;

    public bool IsSingleQuery { get { return QueryStatements.Count == 1; } }

    public List<SqlSegment> QueryStatements { get; set; } = [];
}
