using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Reflection;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace CoreDMS.DBCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            #region log4net
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
               typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            #endregion
            var logger = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly(),
               typeof(log4net.Repository.Hierarchy.Hierarchy));

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

                logger.Info($"DbFile: {DbFile}");
                logger.Info($"SqlFilePath: {SqlFilePath}");
                BuildDb(DbFile, SqlFilePath);
                return 0;
            });

            app.Execute(args);

            logger.Info("done");
            logger.Info("Press any key to exit");
            Console.ReadKey();
        }

        static void BuildDb(string dbfile, string sqlFilesPath)
        {
            var logger = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly(),
               typeof(log4net.Repository.Hierarchy.Hierarchy));

            Console.WriteLine("Creating database");
            var currentDir = System.Environment.CurrentDirectory;
            Console.WriteLine($"current directory: {currentDir}");
            var dbExists = File.Exists(dbfile);
            Console.WriteLine($"Checking existence of {dbfile}: {dbExists}");
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
                    if (!dbExists)
                    {
                        db.Database.EnsureCreated();
                        var sqlFile = new SqlFile(sqlFilesPath + Path.DirectorySeparatorChar + "000-LogTable.sql");
                        RunScript(db, sqlFile);
                    }
                    logger.Debug($"looking for sql files in '{sqlFilesPath}'");
                    List<SqlFile> sqlFileList = new List<SqlFile>();
                    foreach (var file in sqlFiles)
                    {
                        sqlFileList.Add(new SqlFile(file));
                    }
                    foreach (var sqlFile in sqlFileList.OrderBy(s => s.Order))
                    {
                        RunScript(db, sqlFile);
                    }
                }
            }
            else
            {
                Console.WriteLine($"'{sqlFilesPath}' does not exist");
            }
        }

        static void RunScript(DbCreationContext db, SqlFile sqlFile)
        {
            var logger = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly(),
               typeof(log4net.Repository.Hierarchy.Hierarchy));
            logger.Debug($"checking script '{sqlFile.FilePath}'");
            logger.Debug($"scriptname: '{sqlFile.FileName}'");
            
            int checkResult = 0;
            checkResult = db.LogTable.Where(l => l.ScriptName == sqlFile.FileName).Count();

            logger.Debug($"checkResult: {checkResult}");
            if (checkResult == 0)
            {
                var command = File.ReadAllText(sqlFile.FilePath);
                db.Database.ExecuteSqlCommand(command);
                string sqlInsert = "INSERT INTO LogTable (ScriptOrder, ScriptName, createdAt) VALUES ({0}, '{1}', '{2}');";
                var cmd = string.Format(sqlInsert, sqlFile.Order, sqlFile.FileName, DateTime.UtcNow.ToString());
                db.Database.ExecuteSqlCommand(cmd);
            }
            else
            {
                logger.Info($"script '{sqlFile.Order}' has been already installed");
            }
        }
    }
}
