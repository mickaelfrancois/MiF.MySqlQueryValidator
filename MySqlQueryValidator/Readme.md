# MySqlQueryValidator

MySqlQueryValidator is a C# library that validates MySQL queries against a grammar.
It uses ANTLR to parse the queries and provides a simple API to check if a query is valid.

## Getting Started

To validate a MySQL query, use the `MySqlQueryValidator` class.

```csharp
using MySqlQueryValidator;
using System;
using System.Threading.Tasks;

class Program
{
	static async Task Main(string[] args)
	{
		MySqlQueryAnalyzer queryAnalyzer = new();
		string query = "SELECT * FROM users WHERE id = 1";
		QueryInformations result = queryAnalyzer.AnalyseQuery(request.Sql);		
	}
}
```

## How to generate C# classes from MySQL grammar files using ANTLR
Ensure you have Docker installed and running

```
curl -O https://www.antlr.org/download/antlr-4.13.1-complete.jar
curl -O https://raw.githubusercontent.com/antlr/grammars-v4/master/sql/mysql/MySqlLexer.g4
curl -O https://raw.githubusercontent.com/antlr/grammars-v4/master/sql/mysql/MySqlParser.g4

docker run -v $(pwd)/Grammar:/antlr -w /antlr openjdk:17 java -jar antlr-4.13.1-complete.jar -Dlanguage=CSharp MySqlLexer.g4 MySqlParser.g4
```

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests to improve the library.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Librairies

- [ANTLR](https://www.antlr.org/) - A powerful parser generator for reading, processing, executing, or translating structured text or binary files.

