using MiF.MySqlQueryValidator.Models;

namespace MiF.MySqlQueryValidator.Analysers;

/// <summary>
/// Analyzes SQL queries to extract and provide information about their structure and validity.
/// </summary>
/// <remarks>This class implements the <see cref="IMySqlQueryAnalyzer"/> interface to analyze SQL queries and
/// return structured information about the query statements. It handles invalid SQL by returning a result indicating
/// the query is not valid.</remarks>
public class MySqlQueryAnalyzer : IMySqlQueryAnalyzer
{
    /// <summary>
    /// Analyzes the provided SQL query and extracts its segments.
    /// </summary>
    /// <param name="sql">The SQL query string to be analyzed. Cannot be null or empty.</param>
    /// <returns>A <see cref="QueryInformations"/> object containing the extracted query segments. If the SQL query is invalid,
    /// the <see cref="QueryInformations.IsValid"/> property will be <see langword="false"/>.</returns>
    public QueryInformations AnalyseQuery(string sql)
    {
        ArgumentException.ThrowIfNullOrEmpty(sql, nameof(sql));

        SqlSegmentExtractor extractor = new(sql);

        try
        {
            return new()
            {
                QueryStatements = extractor.ExtractSegments()
            };
        }
        catch (ArgumentException)
        {
            return new QueryInformations
            {
                IsValid = false
            };
        }
    }
}
