namespace MiF.MySqlQueryValidator.Listeners;

internal class TableDetectionListener : MySqlParserBaseListener
{
    public HashSet<string> Tables { get; } = [];

    public override void EnterTableName(MySqlParser.TableNameContext context)
    {
        string table = context.GetText();

        if (!string.IsNullOrWhiteSpace(table))
            Tables.Add(table);
    }
}
