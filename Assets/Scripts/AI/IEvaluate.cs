using System.Collections;
using System.Collections.Generic;

public interface IEvaluate<T>
{
    public T Evaluate();
}

public interface IAction
{
    public void Action(Enemy enemy);
}
