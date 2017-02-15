#include "Buggy.h"

void Buggy::go() {
  if (isGoing) {
    return;
  }
  motor.go();
  lastGoTime = millis();
  comms->writeXbee("GOING");
  isGoing = true;
}

void Buggy::stop() {
  if (!isGoing) {
    return;
  }
  motor.stop();
  travelledTime += millis() - lastGoTime;
  comms->writeXbee("STOPPED");
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
}
