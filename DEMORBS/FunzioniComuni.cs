using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using com.ibm.as400.access;
using java.sql;
 

/*
 * classe per la collezione di funzioni comuni a più pagine web
 * */

namespace BRSi
{
    public class FunzioniComuni
    {
        public static AS400JDBCRowSet connectJdbcAS400(string server, string user, string pwd)
        {
            try
            {
                DriverManager.registerDriver(new AS400JDBCDriver());
                AS400JDBCRowSet rowset = new AS400JDBCRowSet("jdbc:as400://" + server + ";translate binary=true", user, pwd);
                //rowset.setQueryTimeout(0);

                return rowset;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static String LaunchCommand(String SERVER, String USER, String PWD, String FILE)
        {
            String res = String.Empty;
            String COMMAND = "CALL PGM(PMBRS/PBRS30CL) PARM('@FILE@')";
            AS400 as400 = null;
            try
            {

                COMMAND = COMMAND.Replace("@FILE@", FILE.PadRight(18));

                // Create an AS400 object  
                as400 = new AS400(SERVER, USER, PWD);

                // Create a Command object
                CommandCall command = new CommandCall(as400);

                // Run the command.

                Boolean success = command.run(COMMAND);

                if (success)
                {
                    res = "Commando Eseguito Correttamente.";
                }
                else
                {
                    res = "Command Failed!";
                }
                AS400Message[] messageList = command.getMessageList();
                foreach (AS400Message message in messageList)
                {
                    res += message.getText() + "\n";
                }

                return res;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            finally
            {
                try
                {
                    // Make sure to disconnect   
                    as400.disconnectAllServices();
                }
                catch { }
            }
        }




    }
}

        