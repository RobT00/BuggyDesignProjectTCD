#include "Buggy.h"

void Buggy::go() {
    if (isGoing) {
        return;
    }
    motor.go();
    lastGoTime = millis();
    comms->writeXbee("GOING");
}

void Buggy::stop() {
    if (isGoing) {
        return;
    }
    motor.stop();
    travelledTime += millis() - lastGoTime;
    comms->writeXbee("STOPPED");
}

unsigned long Buggy::getTravelledTime() const {
    unsigned long ret = travelledTime;
    if (isGoing) {
        ret += millis() - lastGoTime;
    }
    return ret;
}

void Buggy::flashLED() const {
  digitalWrite(LED_BUILTIN, HIGH);
  delay(300);
  digitalWrite(LED_BUILTIN, LOW);
}

void Buggy::gantry_ISR(){
  if(!underGantry){
    irInterrupt = true;
  }
}

void Buggy::detectGantry(){
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
      irInterrupt = false;
      atGantryAt = getTravelledTime();
      underGantry = true;
      //stop();
      //delay(1000);
      //go();
    }
  }
}

int Buggy::readGantry() const {
  while(digitalRead(IR_PIN) != LOW);
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
