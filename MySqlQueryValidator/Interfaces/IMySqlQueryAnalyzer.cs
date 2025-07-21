using MiF.MySqlQueryValidator.Models;

public interface IMySqlQueryAnalyzer
{
    QueryInformations AnalyseQuery(string sql);
}