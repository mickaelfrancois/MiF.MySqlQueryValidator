using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MiF.MySqlQueryValidator.Listeners;
using MiF.MySqlQueryValidator.Models;

namespace MiF.MySqlQueryValidator.Analysers;

internal class SqlSegmentExtractor(string _rawText)
{
    public List<SqlSegment> ExtractSegments()
    {
        AntlrInputStream input = new(_rawText);
        MySqlLexer lexer = new(input);
        CommonTokenStream tokens = new(lexer);
        MySqlParser parser = new(tokens);
        MySqlParser.SqlStatementsContext tree = parser.sqlStatements();

        bool isSyntaxValid = parser.NumberOfSyntaxErrors == 0;
        if (isSyntaxValid == false)
            throw new ArgumentException("The provided SQL query has syntax errors.");

        List<SqlSegment> list = ParseSegments(tree);

        return list;
    }

    private List<SqlSegment> ParseSegments(MySqlParser.SqlStatementsContext tree)
    {
        List<SqlSegment> list = [];

        foreach (MySqlParser.SqlStatementContext? statement in tree.sqlStatement())
        {
            SqlSegment segment = new();

            int start = statement.Start.StartIndex;
            int stop = statement.Stop.StopIndex;
            segment.OriginalText = _rawText.Substring(start, stop - start + 1).Trim();

            MySqlParser.DmlStatementContext dml = statement.dmlStatement();
            MySqlParser.DdlStatementContext ddl = statement.ddlStatement();
            MySqlParser.AdministrationStatementContext administration = statement.administrationStatement();
            //MySqlParser.TransactionStatementContext transaction = statement.transactionStatement();
            //MySqlParser.UtilityStatementContext utility = statement.utilityStatement();
            //MySqlParser.PreparedStatementContext prepared = statement.preparedStatement();


            if (dml != null)
            {
                segment.QueryType = DmlStatement(dml);
                segment.QueryCategory = "DML";
            }
            else if (ddl != null)
            {
                segment.QueryType = DdlStatement(ddl);
                segment.QueryCategory = "DDL";
                segment.IsDDLQuery = true;
            }
            else if (administration != null)
            {
                segment.QueryType = SetStatement(administration);
                segment.QueryCategory = "Administration";
            }

            segment.Tables.AddRange(TableDetection(statement));

            list.Add(segment);
        }

        return list;
    }


    private string DmlStatement(MySqlParser.DmlStatementContext dml)
    {
        if (dml.selectStatement() != null) return "SELECT";
        else if (dml.insertStatement() != null) return "INSERT";
        else if (dml.updateStatement() != null) return "UPDATE";
        else if (dml.deleteStatement() != null) return "DELETE";

        return "Unknown";
    }


    private string DdlStatement(MySqlParser.DdlStatementContext ddl)
    {
        if (ddl?.alterTable() != null)
            return "ALTER TABLE";
        else if (ddl?.dropTable() != null)
            return "DROP TABLE";
        else if (ddl?.truncateTable() != null)
            return "TRUNCATE TABLE";
        else if (ddl?.createTable() != null)
            return "CREATE TABLE";
        else if (ddl?.createDatabase() != null)
            return "CREATE DATABASE";
        else if (ddl?.createView() != null)
            return "CREATE VIEW";
        else if (ddl?.dropTablespace() != null)
            return "DROP TABLESPACE";
        else if (ddl?.dropView() != null)
            return "DROP VIEW";
        else if (ddl?.dropDatabase() != null)
            return "DROP DATABASE";

        return "Unknown";
    }


    private string SetStatement(MySqlParser.AdministrationStatementContext administration)
    {
        if (administration?.setStatement() != null)
            return "SET";
        else if (administration?.analyzeTable() != null)
            return "ANALYSE TABLE";
        else if (administration?.optimizeTable() != null)
            return "OPTIMIZE TABLE";
        else if (administration?.showStatement() != null)
            return "SHOW";
        else if (administration?.repairTable() != null)
            return "REPAIR TABLE";

        return "Unknown";
    }


    private HashSet<string> TableDetection(MySqlParser.SqlStatementContext statement)
    {
        TableDetectionListener tableCollector = new();
        ParseTreeWalker walker = new();
        walker.Walk(tableCollector, statement);

        return tableCollector.Tables;
    }
}
