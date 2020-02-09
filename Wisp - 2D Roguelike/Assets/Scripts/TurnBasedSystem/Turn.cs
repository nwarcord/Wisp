using System.Collections;
using System.Collections.Generic;

public enum TurnActions {
    Move,
    Attack,
    Interact,
    Cede
}

public class Turn {

    private TurnActions action { get; }
    private int modifiers { get; }

    public Turn(TurnActions action, int modifiers) {
        this.action = action;
        this.modifiers = modifiers;
    }

}
