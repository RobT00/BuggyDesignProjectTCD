#include "Buggy.h"

void Buggy::go() {
    if (isGoing) {
        return;
    }
    motor.go();
    lastGoTime = millis();
}

void Buggy::stop1() {
    if (isGoing) {
        return;
    }
    motor.stop1();
    travelledTime += millis() - lastGoTime;
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
  if(!atGantry){
    irInterrupt = true;
  }
}

void Buggy::detectGantry(){
  if(atGantry && getTravelledTime() > 500){
    atGantry = false;
  }
  if (irInterrupt){
    int gantry = readGantry();
    if (gantry == -1) {
      comms.writeXbee("GANTRY_INVALID");
      irInterrupt = false;
    } else {
      comms.writeXbee("GANTRY" + String(gantry));
      irInterrupt = false;
      atGantry = true;
      stop1();
      delay(1000);
      go();
    }
  }
}

int Buggy::readGantry() const {
  while(digitalRead(IR_PIN) != LOW);
  int pulse = pulseIn(IR_PIN, HIGH);

  if(pulse >= 500 && pulse <= 1500){
    return 1;
  }else if (pulse >= 1500 && pulse <= 2500){
    return 2;
  }else if (pulse >= 2500 && pulse <= 3500){
    return 3;
  } else {
    return -1;
  }
}


