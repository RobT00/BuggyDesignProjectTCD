#include "Buggy.h"

void Buggy::go(bool silent = false) {
  if (isGoing) {
    return;
  }
  motor.go();
  lastGoTime = millis();
  if (!silent) {
    comms->writeXbee("GOING");
  }
  isGoing = true;
}

void Buggy::stop(bool silent = false) {
  if (!isGoing) {
    return;
  }
  motor.stop();
  travelledTime += millis() - lastGoTime;
  if (!silent) {
    comms->writeXbee("STOPPED");
  }
  isGoing = false;
}

unsigned long Buggy::getTravelledTime() const {
  if (isGoing) {
    return travelledTime + (millis() - lastGoTime);
  } else {
    return travelledTime;
  }
}

void Buggy::flashLED() const {
  digitalWrite(13, HIGH);
  delay(300);
  digitalWrite(13, LOW);
}

void Buggy::gantry_ISR(){
  if(!underGantry) {
    irInterrupt = true;
  }
}

void Buggy::detectGantry() {
  if (underGantry && timeTravelledSinceGantry() > 500) {
    underGantry = false;
  }
  if (irInterrupt){
    int gantry = readGantry();
    if (gantry == -1) {
      comms->writeXbee("GANTRY_INVALID");
      irInterrupt = false;
    } else {
      comms->writeXbee("GANTRY" + String(gantry));
      atGantryAt = getTravelledTime();
      //stop()
      //delay(1000);
      //go();
      underGantry = true;
      irInterrupt = false;
    }
  }
}

int Buggy::readGantry() const {
  while(digitalRead(IR_PIN) == HIGH);
  int pulse = pulseIn(IR_PIN, HIGH);

  if (pulse >= 500 && pulse <= 1500) {
    return 1;
  } else if (pulse >= 1500 && pulse <= 2500) {
    return 2;
  } else if (pulse >= 2500 && pulse <= 3500) {
    return 3;
  } else {
    return -1;
  }
}

unsigned long Buggy::timeTravelledSinceGantry() const {
  return getTravelledTime() - atGantryAt;
}

void Buggy::update() {
  detectGantry();
  updateParking();
}

void Buggy::updateParking() {
  if (!isGoing) {
    return; // Do not interfere with other go-stop functionalities
  }

  unsigned long sinceGantry = timeTravelledSinceGantry();
  if (parkingState == BEFORE_INTERSECTION && sinceGantry > parking_overrideAt) {
    if (travelDirection == CLOCKWISE) {
      motor.leftOverride();
    } else {
      motor.rightOverride();
    }
    parkingState = IN_INTERSECTION;
  } else if (parkingState == IN_INTERSECTION && sinceGantry > parking_overrideOffAt) {
    motor.go(); // Just reset the override, not Buggy::go()
    parkingState = AFTER_INTERSECTION;
  } else if (parkingState == AFTER_INTERSECTION && sinceGantry > parking_stopAt) {
    stop(true);
    comms->writeXbee("PARKED");
    parkingState = NOT_PARKING;
  }
}

void Buggy::park() {
  parkingState = BEFORE_INTERSECTION;
}
