using System.ComponentModel.DataAnnotations;

namespace MySqlQueryValidatorApi;

public class QueryRequest
{
    [Required(ErrorMessage = "SQL query is required.")]
    public string Sql { get; set; } = string.Empty;
}
