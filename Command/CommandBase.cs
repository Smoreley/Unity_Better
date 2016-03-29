// Abu Kingly 2016
using System;

namespace Revamped
{
    #region >> Abstract Bases <<

    // void action
    public abstract class CommandBase
    {
        public CommandBase(Action act) {
            action = act;
        }

        public abstract void Execute();

        protected Action action; // Some method action to do
    }

    // action takes on parameter
    public abstract class CommandBase<T>
    {
        public CommandBase(Action<T> act) {
            action = act;
        }

        public abstract void Execute(T t);

        protected Action<T> action; // Some method action to do
    }

    #endregion

    #region >> Relizations <<

    class ConcreteCommand : CommandBase
    {
        public ConcreteCommand(Action act) : base(act) { }

        public override void Execute() {
            action();
        }
    }


    class ConcreteFloatCommand : CommandBase<float>
    {
        public ConcreteFloatCommand(Action<float> act) : base(act) { }

        public override void Execute(float t) {
            action(t);
        }
    }

    class AxisCommand : CommandBase<float>
    {
        public AxisCommand(Action<float> act) : base(act) { }

        public override void Execute(float t) {
            action(UnityEngine.Mathf.Clamp(t, -1, 1));
        }
    }

    #endregion

}