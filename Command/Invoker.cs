// Abu Kingly - 2016
using System.Collections.Generic;

namespace Revamped
{

    /// <summary>
    /// Stores a list of commands, which will be executed togeather by method call
    /// </summary>
    public class Invoker
    {
        private List<CommandBase> _commands = new List<CommandBase>();

        public void AddCommand(CommandBase command) {
            _commands.Add(command);
        }

        public void ClearCommands() {
            _commands.Clear();
        }

        public void ExecuteCommands() {
            for (int i = 0; i < _commands.Count; i++) {
                _commands[i].Execute();
            }
        }
    }

    /// <summary>
    /// Stores a list of commands, which takes a generic variable, commands will be executed togeather by method call
    /// </summary>
    /// <typeparam name="T">passed type</typeparam>
    class Invoker<T>
    {
        private List<CommandBase<T>> _commands = new List<CommandBase<T>>();

        public void AddCommand(CommandBase<T> command) {
            _commands.Add(command);
        }

        public void ClearCommands() {
            _commands.Clear();
        }

        public void ExecuteCommands(T tVar) {
            for (int i = 0; i < _commands.Count; i++) {
                _commands[i].Execute(tVar);
            }
        }

    }
}