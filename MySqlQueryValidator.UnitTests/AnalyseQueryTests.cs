using MiF.MySqlQueryValidator.Analysers;
using MiF.MySqlQueryValidator.Models;

namespace MySqlQueryValidator.UnitTests;

public class AnalyseQueryTests
{
    [Fact]
    public void AnalyseQuery_ShouldReturnValidSingleSelectQueryInformations()
    {
        // Arrange
        IMySqlQueryAnalyzer queryAnalyzer = new MySqlQueryAnalyzer();
        string sql = "SELECT * FROM users";

        // Act
        QueryInformations result = queryAnalyzer.AnalyseQuery(sql);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSingleQuery);
        Assert.Equal("SELECT", result.QueryStatements.First().QueryType);
        Assert.False(result.QueryStatements.First().IsDDLQuery);
        Assert.False(result.QueryStatements.First().IsNonQuery);
        Assert.Equal("users", result.QueryStatements.First().Tables.First());
        Assert.Equal(sql, result.QueryStatements.First().OriginalText);
    }


    [Fact]
    public void AnalyseQuery_WithDeleteQuery_ShouldReturnValidSingleNonQueryInformations()
    {
        // Arrange
        IMySqlQueryAnalyzer queryAnalyzer = new MySqlQueryAnalyzer();
        string sql = "DELETE FROM users";

        // Act
        QueryInformations result = queryAnalyzer.AnalyseQuery(sql);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSingleQuery);
        Assert.Equal("DELETE", result.QueryStatements.First().QueryType);
        Assert.False(result.QueryStatements.First().IsDDLQuery);
        Assert.True(result.QueryStatements.First().IsNonQuery);
        Assert.Equal("users", result.QueryStatements.First().Tables.First());
        Assert.Equal(sql, result.QueryStatements.First().OriginalText);
    }


    [Fact]
    public void AnalyseQuery_WithInsertQuery_ShouldReturnValidSingleNonQueryInformations()
    {
        // Arrange
        IMySqlQueryAnalyzer queryAnalyzer = new MySqlQueryAnalyzer();
        string sql = "INSERT INTO users (id) VALUES (1)";

        // Act
        QueryInformations result = queryAnalyzer.AnalyseQuery(sql);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSingleQuery);
        Assert.Equal("INSERT", result.QueryStatements.First().QueryType);
        Assert.False(result.QueryStatements.First().IsDDLQuery);
        Assert.True(result.QueryStatements.First().IsNonQuery);
        Assert.Equal("users", result.QueryStatements.First().Tables.First());
        Assert.Equal(sql, result.QueryStatements.First().OriginalText);
    }


    [Fact]
    public void AnalyseQuery_WithUpdateQuery_ShouldReturnValidSingleNonQueryInformations()
    {
        // Arrange
        IMySqlQueryAnalyzer queryAnalyzer = new MySqlQueryAnalyzer();
        string sql = "UPDATE users SET id = 1 WHERE name IN (SELECT name FROM contacts)"; // "UDPATE users SET id = 1";

        // Act
        QueryInformations result = queryAnalyzer.AnalyseQuery(sql);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSingleQuery);
        Assert.Equal("UPDATE", result.QueryStatements.First().QueryType);
        Assert.False(result.QueryStatements.First().IsDDLQuery);
        Assert.True(result.QueryStatements.First().IsNonQuery);
        Assert.Equal("users", result.QueryStatements.First().Tables.First());
        Assert.Equal(sql, result.QueryStatements.First().OriginalText);
    }


    [Fact]
    public void AnalyseQuery_WithAlterShouldReturnValidSingleSelectQueryInformations()
    {
        // Arrange
        IMySqlQueryAnalyzer queryAnalyzer = new MySqlQueryAnalyzer();
        string sql = "ALTER TABLE users ADD COLUMN id INT";

        // Act
        QueryInformations result = queryAnalyzer.AnalyseQuery(sql);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSingleQuery);
        Assert.Equal("ALTER TABLE", result.QueryStatements.First().QueryType);
        Assert.True(result.QueryStatements.First().IsDDLQuery);
        Assert.False(result.QueryStatements.First().IsNonQuery);
        Assert.Equal("users", result.QueryStatements.First().Tables.First());
        Assert.Equal(sql, result.QueryStatements.First().OriginalText);
    }
}
