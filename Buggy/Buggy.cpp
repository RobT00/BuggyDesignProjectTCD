#include "Buggy.h"

void Buggy::go() {
    if (isGoing) {
        return;
    }
    motor.go();
    lastGoTime = millis();
}

void Buggy::stop() {
    if (!isGoing) {
        return;
    }
    motor.stop();
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
