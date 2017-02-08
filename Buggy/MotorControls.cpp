#include "MotorControls.h"

#include "Arduino.h"

MotorControls::MotorControls() {
  pinMode(bcc_pin, OUTPUT);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::leftOverride() {
  digitalWrite(bcc_pin, HIGH);
  delay(6);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::rightOverride() {
  digitalWrite(bcc_pin, HIGH);
  delay(8);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::go() {
  digitalWrite(bcc_pin, HIGH);
  delay(4);
  digitalWrite(bcc_pin, LOW);
}

void MotorControls::stop() {
  digitalWrite(bcc_pin, HIGH);
  delay(2);
  digitalWrite(bcc_pin, LOW);
}
