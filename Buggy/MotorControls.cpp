#include "MotorControls.h"

#include <Arduino.h>

MotorControls::MotorControls() {
  pinMode(bcc_pin, OUTPUT);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::leftOverride() const {
  digitalWrite(bcc_pin, HIGH);
  delay(6);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::rightOverride() const {
  digitalWrite(bcc_pin, HIGH);
  delay(8);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::go() const {
  digitalWrite(bcc_pin, HIGH);
  delay(4);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::stop() const {
  digitalWrite(bcc_pin, HIGH);
  delay(2);
  digitalWrite(bcc_pin, LOW);
}
