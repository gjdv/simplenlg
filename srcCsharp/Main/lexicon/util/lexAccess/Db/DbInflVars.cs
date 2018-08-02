using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
//using java.sql;
using SimpleNLG.Main.lexicon.util.lexAccess.Lib;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Db
{
    using Configuration = Configuration;

    public class DbInflVars
    {
        public static List<string> GetEuis(object conn, XMLRealiser.LexiconType lexiconType, string inflVar)
        {
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    string query = "SELECT eui FROM INFL_VARS where (inflVarLc = '" +
                                   DbBase.FormatSqlStr(inflVar.ToLower()) + "')";
                    //Statement statement = ((Connection) conn).createStatement();
                    //ResultSet rs = statement.executeQuery(query);
                    //List<string> outs = new List<string>();
                    //while (rs.next())
                    //{
                    //    string eui = rs.getString("eui");
                    //    outs.Add(eui);
                    //}

                    //rs.close();
                    //statement.close();
                    //return outs;
                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    List<string> euis = new List<string>();
                    SQLiteCommand command = new SQLiteCommand("SELECT eui FROM INFL_VARS where (inflVarLc = @INFLVAR)",
                        (SQLiteConnection) conn);
                    command.Parameters.AddWithValue("@INFLVAR", DbBase.FormatSqlStr(inflVar.ToLower()));
                    SqliteAccess.ProcessDataReader((SQLiteConnection) conn, command,
                        reader => euis.Add(reader["eui"] as string));
                    return euis;
            }

            return null;
        }

        public static List<string> GetUniqueEuisByInflVar(object conn, XMLRealiser.LexiconType lexiconType,
            string inflVar)
        {
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    //string query = "SELECT eui FROM INFL_VARS where (inflVarLc = '" +
                    //               DbBase.FormatSqlStr(inflVar.ToLower()) + "')";
                    //Statement statement = ((Connection) conn).createStatement();
                    //ResultSet rs = statement.executeQuery(query);
                    //List<string> outs = new List<string>();
                    //while (rs.next())
                    //{
                    //    string eui = rs.getString("eui");
                    //    if (!outs.Contains(eui))
                    //    {
                    //        outs.Add(eui);
                    //    }
                    //}

                    //rs.close();
                    //statement.close();
                    //return outs;
                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    HashSet<string> euis = new HashSet<string>();
                    SQLiteCommand command = new SQLiteCommand("SELECT eui FROM INFL_VARS where (inflVarLc = @INFLVAR)",
                        (SQLiteConnection) conn);
                    command.Parameters.AddWithValue("@INFLVAR", DbBase.FormatSqlStr(inflVar.ToLower()));
                    SqliteAccess.ProcessDataReader((SQLiteConnection) conn, command,
                        reader => euis.Add(reader["eui"] as string));
                    return euis.ToList();
            }

            return null;
        }

        public static List<string> GetBasesByEui(object conn, XMLRealiser.LexiconType lexiconType, string eui)
        {
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    //string query = "SELECT inflVar FROM INFL_VARS where (eui = '" + eui + "' and inflection = 1)";
                    //Statement statement = ((Connection) conn).createStatement();
                    //ResultSet rs = statement.executeQuery(query);
                    //List<string> outs = new List<string>();
                    //while (rs.next())
                    //{
                    //    string @base = rs.getString("inflVar");
                    //    outs.Add(@base);
                    //}

                    //rs.close();
                    //statement.close();
                    //return outs;
                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    List<string> recordStringList = new List<string>();
                    SQLiteCommand command =
                        new SQLiteCommand("SELECT inflVar FROM INFL_VARS where (eui = @EUI and inflection = 1)",
                            (SQLiteConnection) conn);
                    command.Parameters.AddWithValue("@EUI", eui);
                    SqliteAccess.ProcessDataReader((SQLiteConnection) conn, command,
                        reader => recordStringList.Add(reader["inflVar"] as string));
                    return recordStringList;
            }

            return null;
        }

        //public static void Main(string[] args)
        //{
        //    string inflVar = "books";
        //    if (args.Length == 1)
        //    {
        //        inflVar = args[0];
        //    }

        //    Configuration conf = new Configuration("data.config.la", true);
        //    try
        //    {
        //        Connection conn = DbBase.OpenConnection(conf);
        //        if (conn != null)
        //        {
        //            List<string> euis = GetUniqueEuisByInflVar(conn, XMLRealiser.LexiconType.NIHDB_HSQL, inflVar);
        //            Console.WriteLine("-- inflVar: " + inflVar + " ---");
        //            for (int i = 0; i < euis.Count; i++)
        //            {
        //                Console.WriteLine((string) euis[i]);
        //            }

        //            euis = GetEuis(conn, XMLRealiser.LexiconType.NIHDB_HSQL, inflVar);
        //            DbBase.CloseConnection(conn);
        //            Console.WriteLine("-- inflVar: " + inflVar + " ---");
        //            for (int i = 0; i < euis.Count; i++)
        //            {
        //                Console.WriteLine((string) euis[i]);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("** Error: " + e.Message);
        //    }
        //}
    }
}