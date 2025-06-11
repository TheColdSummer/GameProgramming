/*
 * This is the interface for all states in the Finite State Machine.
 */
public interface IState
{
    void OnEnter();

    void OnExit();

    void OnUpdate();
}
