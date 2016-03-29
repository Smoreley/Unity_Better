// Abu Kingly - 2016
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Revamped
{
    // other name idea eventManager
    public class CommandManager<T> where T : IComparable, IFormattable, IConvertible
    {

        // Given an enum search through the dict and execture the commands with matching enum
        Dictionary<T, Invoker> commandDict;

        // Constructor
        public CommandManager() {
            if (!typeof(T).IsEnum) { Debug.LogError("Type must be of type enum: "); }
            commandDict = new Dictionary<T, Invoker>();     // Initialize dictionary
        }

        public void AddCommand(T en, CommandBase cmd) {
            if (commandDict.ContainsKey(en)) {
                commandDict[en].AddCommand(cmd);            // Add command to the invoker
            } else {
                Invoker newInvoker = new Invoker();         // Create new invoker
                newInvoker.AddCommand(cmd);                 // Add command to the new invoker
                commandDict.Add(en, newInvoker);            // Add invoker to dictonary 
            }
        }

        public void ClearCommands() {
            commandDict.Clear();
        }

        public void ExecuteCommand(T en) {
            try { commandDict[en].ExecuteCommands(); } catch (Exception ex) { throw; }
        }
    }

    /// <summary>
    /// Accepts one paramater for event
    /// </summary>
    /// <typeparam name="T">enum type used as key for dictonary</typeparam>
    /// <typeparam name="J">passed parameter to receivers on execute</typeparam>
    public class CommandManager<T,J> where T : IComparable, IFormattable, IConvertible
    {

        // Given an enum search through the dict and execture the commands with matching enum
        Dictionary<T, Invoker<J>> commandDict;

        // Constructor
        public CommandManager() {
            if (!typeof(T).IsEnum) { Debug.LogError("Type must be of type enum: "); }
            commandDict = new Dictionary<T, Invoker<J>>();           // Initialize dictionary
        }

        public void AddControl(T en, CommandBase<J> cmd) {
            if (commandDict.ContainsKey(en)) {
                commandDict[en].AddCommand(cmd);                            // Add command to the invoker
            } else {
                Invoker<J> newInvoker = new Invoker<J>();     // Create new invoker
                newInvoker.AddCommand(cmd);                                 // Add command to the new invoker
                commandDict.Add(en, newInvoker);                            // Add invoker to dictonary 
            }
        }

        public void ClearControls() {
            commandDict.Clear();
        }

        public void ExecuteCommand(T en, J j) {
                try { commandDict[en].ExecuteCommands(j); } catch (Exception ex) { throw; }
        }
    }
}