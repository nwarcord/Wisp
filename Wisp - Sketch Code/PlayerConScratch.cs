bool tookAction = false; // member variable

void Update(){
  ParseAction();
}

void ParseAction(){
  if(playerPressedMovementKey){
    Move(); // The logic is obviously more complex than this
    tookAction = true;
  }
  else if(playerPressedAttack){
    Attack();
    tookAction = true;
  }
}

public void TakeTurn(){
  StartCoroutine(WaitForAction());
}

private IEnumerator WaitForAction(){
  while(!tookAction){
    yield return null;
  }

  tookAction = false;
  EventManager.RaiseActorTurnOver();
}
