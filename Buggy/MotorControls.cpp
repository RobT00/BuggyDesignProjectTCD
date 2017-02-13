#include "MotorControls.h"

#include <Arduino.h>

MotorControls::MotorControls() {
  pinMode(bcc_pin, OUTPUT);
  digitalWrite(bcc_pin, LOW);
  delay(20);
  stop();
}

void MotorControls::write(short duration) const {
  if (duration > 20) {
    return;
  }
  digitalWrite(bcc_pin, HIGH);
  delay(duration);
  digitalWrite(bcc_pin, LOW);
  delay(20 - duration);
}

void MotorControls::leftOverride() const {
  write(6);
}

void MotorControls::rightOverride() const {
  write(8);
}

void MotorControls::go() const {
  write(4);
}

void MotorControls::stop() const {
  write(2);
}
