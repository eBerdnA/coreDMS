using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;

namespace coreDMS.DBCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            app.HelpOption("-h|--help");

            var optionMessage = app.Option("-s|--sql <sqlfiles>", "Required. path to sql files", CommandOptionType.SingleValue)
            .IsRequired();

            var optionMessage2 = app.Option("-f|--file <dbfile>", "Required. db file", CommandOptionType.SingleValue)
            .IsRequired();

            app.OnExecute(() =>
            {
                var SqlFilePath = optionMessage.HasValue()
                    ? optionMessage.Value()
                    : "";

                var DbFile = optionMessage2.HasValue()
                    ? optionMessage2.Value()
                    : "";

                Console.WriteLine($"DbFile: {DbFile}");
                Console.WriteLine($"SqlFilePath: {SqlFilePath}");
                BuildDb(DbFile, SqlFilePath);
                return 0;
            });

            app.Execute(args);

            Console.WriteLine("done");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void BuildDb(string dbfile, string sqlFilesPath)
        {
            Console.WriteLine("Creating database");
            var currentDir = System.Environment.CurrentDirectory;
            Console.WriteLine($"current directory: {currentDir}");
            Console.WriteLine($"Checking existence of {dbfile}: {File.Exists(dbfile)}");
            if (!File.Exists(dbfile))
            {
                if (Directory.Exists(sqlFilesPath))
                {
                    var sqlFiles = Directory.GetFiles(sqlFilesPath, "*.sql").Where(name => !name.EndsWith("000-LogTable.sql", StringComparison.InvariantCultureIgnoreCase)).ToArray();
                    if (sqlFiles.Length == 0)
                    {
                        Console.WriteLine("no sql files found, therefore doing nothing");
                        return;
                    }
                    var optionsBuilder = new DbContextOptionsBuilder<DbCreationContext>();
                    optionsBuilder.UseSqlite($"Data Source={dbfile}");
                    using (var db = new DbCreationContext(optionsBuilder.Options))
                    {
                        db.Database.EnsureCreated();
                        RunScript(db, sqlFilesPath + "\\" + "000-LogTable.sql");
                        foreach (var file in sqlFiles)
                        {
                            RunScript(db, file);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"'{sqlFilesPath}' does not exist");
                }
            }
            else
            {
                Console.WriteLine("db file already exists, therefore doing nothing");
            }
        }

        static void RunScript(DbCreationContext db, string filePath)
        {
            var scrriptName = filePath.Substring(filePath.LastIndexOf("\\")+1);
            var scriptOrder = int.Parse(scrriptName.Substring(0, 3));
            var command = File.ReadAllText(filePath);
            db.Database.ExecuteSqlCommand(command);
            var cmd = string.Format("INSERT INTO LogTable (ScriptOrder, ScriptName, createdAt) VALUES ({0}, '{1}', '{2}');", scriptOrder, scrriptName, DateTime.UtcNow.ToString());
            db.Database.ExecuteSqlCommand(cmd);
        }
    }
}
